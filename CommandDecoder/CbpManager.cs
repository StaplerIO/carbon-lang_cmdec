using CommandDecoder.Decoder;
using CommandDecoder.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandDecoder
{
    public class CbpManager
    {
        public static DecodedPackage DecodePackage(byte[] stream, bool decodeStringPool = true, bool decodeFunctionTable = true)
        {
            var result = new DecodedPackage { };
            var streamIndex = 0;

            var metadataResult = ReadMetadata(stream);
            result.Metadata = metadataResult.Item1;
            streamIndex += metadataResult.Item2;

            if (decodeStringPool)
            {
                var stringPoolResult = ReadStringPool(stream[streamIndex..], result.Metadata);
                result.StringPool = stringPoolResult.Item1;
                streamIndex += stringPoolResult.Item2;
            }

            if (decodeFunctionTable)
            {
                var functionTableResult = ReadFunctionTable(stream[streamIndex..], result.Metadata);
                result.FunctionTable = functionTableResult.Item1;
                streamIndex += functionTableResult.Item2;
            }

            // Decode commands
            var commands = DecodeCommands(stream[streamIndex..], result.Metadata);
            result.CommandTable = commands.ToArray();

            return result;
        }

        public static (PackageMetadata, int) ReadMetadata(byte[] content)
        {
            if (content.Length > 5)
            {
                var result = new PackageMetadata
                {
                    PackageType = content[0],
                    DataAlignment = content[1],
                    DomainLayerCountAlignment = content[2],
                    DataSlotAlignment = content[3],
                    AddressAlignment = content[4],
                };

                if (result.PackageType == 0)
                {
                    // This is an executable, read entry point
                    result.EntryPointAddress = (int)Utils.ByteToInt64WithArrayLen(content[5..], result.AddressAlignment);
                    return (result, 5 + result.AddressAlignment);
                }

                return (result, 5);
            }

            throw new ArgumentException("Invalid cbp package");
        }

        public static (FunctionTableEntry[], int) ReadFunctionTable(byte[] content, PackageMetadata metadata)
        {
            var tableSize = (int)Utils.ByteToInt64WithArrayLen(content, metadata.AddressAlignment);
            var result = new List<FunctionTableEntry>(tableSize);

            content = content[metadata.AddressAlignment..];
            var currentIndex = 0;
            for (int i = 0; i < tableSize; i++)
            {
                int slot = (int)Utils.ByteToInt64WithArrayLen(content[currentIndex..], metadata.AddressAlignment);
                var entryPointAddress = Utils.ByteToInt64WithArrayLen(content[(currentIndex + metadata.AddressAlignment)..], metadata.AddressAlignment);
                result.Add(new FunctionTableEntry { FunctionId = slot, EntryPointAddress = entryPointAddress });

                currentIndex += metadata.AddressAlignment * 2;
            }

            return (result.ToArray(), currentIndex + metadata.AddressAlignment);
        }

        public static (string[], int) ReadStringPool(byte[] content, PackageMetadata metadata)
        {
            var poolSize = (int)Utils.ByteToInt64WithArrayLen(content, metadata.DataSlotAlignment);
            var result = new List<string>(poolSize);

            content = content[metadata.DataSlotAlignment..];
            var currentIndex = 0;
            for (int i = 0; i < poolSize; i++)
            {
                var stringLength = (int)Utils.ByteToInt64WithArrayLen(content[currentIndex..], metadata.DataSlotAlignment);
                currentIndex += metadata.DataSlotAlignment;
                var iterResult = Encoding.ASCII.GetString(content[currentIndex..], 0, stringLength);

                result.Add(iterResult);

                currentIndex += stringLength;
            }

            return (result.ToArray(), currentIndex + metadata.DataSlotAlignment);
        }

        public static List<CommandTableEntry> DecodeCommands(byte[] content, PackageMetadata metadata)
        {
            var commands = new List<CommandTableEntry>();

            // TODO: Split them into sub functions, current code is too ugly
            DecodeResult result = new(0, new CommandTableEntry());
            for (var index = 0; index < content.Length; index += result.Length)
            {
                switch (content[index])
                {
                    case 0xA1:
                        result = ObjectCommandDecoder.CreateObjectDecoder(content, index);
                        commands.Add(result.Command);
                        break;
                    case 0xA2:
                        result = ObjectCommandDecoder.DestroyObjectDecoder(content, metadata, index);
                        commands.Add(result.Command);
                        break;

                    case 0xB1:
                        result = StackCommandDecoder.PushCommand(content, metadata, index);
                        commands.Add(result.Command);
                        break;
                    case 0xB2:
                        result = StackCommandDecoder.PushFromObjectCommand(content, metadata, index);
                        commands.Add(result.Command);
                        break;
                    case 0xB3:
                        result = StackCommandDecoder.PopCommand(content, metadata, index);
                        commands.Add(result.Command);
                        break;
                    case 0xB4:
                        result = StackCommandDecoder.PopToObjectCommand(content, metadata, index);
                        commands.Add(result.Command);
                        break;

                    case 0xC1:
                        result = DomainCommandDecoder.CreateDomainCommand(content, metadata, index);
                        commands.Add(result.Command);
                        break;
                    case 0xC2:
                        result = DomainCommandDecoder.DestroyDomainCommand(content, metadata, index);
                        commands.Add(result.Command);
                        break;

                    case 0xD1:
                        result = JumpCommandDecoder.DecodeDirectJumpCommand(content, metadata, index);
                        commands.Add(result.Command);
                        break;
                    case 0xD2:
                        result = JumpCommandDecoder.DecodeConditionalJumpCommand(content, metadata, index);
                        commands.Add(result.Command);

                        break;
                    case 0xE1:
                        result = FunctionCommandDecoder.EnterFunction(content, metadata, index);
                        commands.Add(result.Command);
                        break;
                    case 0xE2:
                        result = FunctionCommandDecoder.LeaveFunctionWithoutValue(content, index);
                        commands.Add(result.Command);
                        break;
                    case 0xE3:
                        result = FunctionCommandDecoder.LeaveFunctionWithValue(content, index);
                        commands.Add(result.Command);
                        break;

                    case 0xF1:
                        result = MathCommandDecoder.DecodeCalcCommand(content, index);
                        commands.Add(result.Command);
                        index += result.Length;
                        break;
                    case 0xF2:
                        result = MathCommandDecoder.DecodeLogicalCommand(content, index);
                        commands.Add(result.Command);
                        break;
                }
            }

            return commands;
        }
    }
}
