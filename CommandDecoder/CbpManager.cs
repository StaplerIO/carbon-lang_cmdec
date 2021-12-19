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
                    VariableSlotAlignment = content[0],
                    DataAlignment = content[1],
                    CommandAlignment = content[2],
                    EntryPointOffset = content[3],
                    DomainLayerCountAlignment = content[4],
                };
            }

            throw new ArgumentException("Invalid cbp package");
        }

        public static List<CommandTableEntry> DecodeCommands(byte[] content, PackageMetadata metadata)
        {
            var commands = new List<CommandTableEntry>();

            var index = 0;
            while (index <= content.Length && content[index] != 0x00)
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
                    case 0xB4:
                        result = StackCommandDecoder.PopToObjectCommand(content, metadata, index);
                        commands.Add(result.Command);
                        index += result.Length;
                        break;
                    case 0xF1:
                        result = MathCommandDecoder.DecodeCalcCommand(content, metadata, index);
                        commands.Add(result.Command);
                        index += result.Length;
                        break;
                }
            }

            return commands;
        }
    }
}
