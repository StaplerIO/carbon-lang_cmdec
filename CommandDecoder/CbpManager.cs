using CommandDecoder.Decoder;
using CommandDecoder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandDecoder
{
    public class CbpManager
    {
        public static PackageMetadata ReadMetadata(byte[] content)
        {
            if (content.Length > 5)
            {
                return new PackageMetadata
                {
                    PackageType = content[0],
                    DataAlignment = content[1],
                    DomainLayerCountAlignment = content[2],
                    DataSlotAlignment = content[3],
                    AddressAlignment = content[4],
                };
            }

            throw new ArgumentException("Invalid cbp package");
        }

        public static List<CommandTableEntry> DecodeCommands(byte[] content, PackageMetadata metadata)
        {
            var commands = new List<CommandTableEntry>();

            var index = 5;
            while (index < content.Length)
            {
                DecodeResult result;
                switch (content[index])
                {
                    case 0xA1:
                        result = ObjectCommandDecoder.CreateObjectDecoder(content, index);
                        commands.Add(result.Command);
                        index += result.Length;
                        break;
                    case 0xA2:
                        result = ObjectCommandDecoder.DestroyObjectDecoder(content, metadata, index);
                        commands.Add(result.Command);
                        index += result.Length;
                        break;

                    case 0xB1:
                        result = StackCommandDecoder.PushCommand(content, metadata, index);
                        commands.Add(result.Command);
                        index += result.Length;
                        break;
                    case 0xB2:
                        result = StackCommandDecoder.PushFromObjectCommand(content, metadata, index);
                        commands.Add(result.Command);
                        index += result.Length;
                        break;
                    case 0xB3:
                        result = StackCommandDecoder.PopCommand(content, metadata, index);
                        commands.Add(result.Command);
                        index += result.Length;
                        break;
                    case 0xB4:
                        result = StackCommandDecoder.PopToObjectCommand(content, metadata, index);
                        commands.Add(result.Command);
                        index += result.Length;
                        break;

                    case 0xC1:
                        result = DomainCommandDecoder.CreateDomainCommand(content, metadata, index);
                        commands.Add(result.Command);
                        index += result.Length;
                        break;
                    case 0xC2:
                        result = DomainCommandDecoder.DestroyDomainCommand(content, metadata, index);
                        commands.Add(result.Command);
                        index += result.Length;
                        break;

                    case 0xD1:
                        result = JumpCommandDecoder.DecodeDirectJumpCommand(content, metadata, index);
                        commands.Add(result.Command);
                        index += result.Length;
                        break;
                    case 0xD2:
                        result = JumpCommandDecoder.DecodeConditionalJumpCommand(content, metadata, index);
                        commands.Add(result.Command);
                        index += result.Length;

                        break;
                    case 0xE1:
                        result = FunctionCommandDecoder.EnterFunction(content, metadata, index);
                        commands.Add(result.Command);
                        index += result.Length;
                        break;
                    case 0xE2:
                        result = FunctionCommandDecoder.LeaveFunctionWithoutValue(content, index);
                        commands.Add(result.Command);
                        index += result.Length;
                        break;
                    case 0xE3:
                        result = FunctionCommandDecoder.LeaveFunctionWithValue(content, index);
                        commands.Add(result.Command);
                        index += result.Length;
                        break;

                    case 0xF1:
                        result = MathCommandDecoder.DecodeCalcCommand(content, index);
                        commands.Add(result.Command);
                        index += result.Length;
                        break;
                    case 0xF2:
                        result = MathCommandDecoder.DecodeLogicalCommand(content, index);
                        commands.Add(result.Command);
                        index += result.Length;
                        break;
                }
            }

            return commands;
        }
    }
}
