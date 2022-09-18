using CommandDecoder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandDecoder.Decoder
{
    internal class JumpCommandDecoder
    {
        public static DecodeResult DecodeDirectJumpCommand(byte[] stream, PackageMetadata metadata, int baseIndex)
        {
            stream = stream[baseIndex..];

            return new(1 + metadata.AddressAlignment + 1, new CommandTableEntry
            {
                Location = baseIndex,
                RawData = stream[0..(1 + metadata.AddressAlignment + 1)],
                Description = "Jump unconditional"
            });
        }

        public static DecodeResult DecodeConditionalJumpCommand(byte[] stream, PackageMetadata metadata, int baseIndex)
        {
            stream = stream[baseIndex..];

            return new(1 + (metadata.AddressAlignment + 1) * 3, new CommandTableEntry
            {
                Location = baseIndex,
                RawData = stream[0..(1 + (metadata.AddressAlignment + 1) * 3)],
                Description = "Jump by condition"
            });
        }

        private static long CalculateRelativeAddress(byte[] raw)
        {
            long result = 0;
            foreach (byte b in raw[1..])
            {
                result = result * 0x10 + b;
            }

            if (raw[0] == 0x02)
            {
                result = -result;
            }

            return result;
        }
    }
}
