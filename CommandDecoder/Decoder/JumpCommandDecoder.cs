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
        internal static DecodeResult DecodeDirectJumpCommand(byte[] stream, PackageMetadata metadata, int baseIndex)
        {
            stream = stream[baseIndex..];

            var addr = CalculateRelativeAddress(stream[1..], metadata.AddressAlignment);

            return new(1 + metadata.AddressAlignment + 1, new CommandTableEntry
            {
                Location = baseIndex,
                RawData = stream[0..(1 + metadata.AddressAlignment + 1)],
                Description = $"Jump unconditionally to relative address {addr}"
            });
        }

        internal static DecodeResult DecodeConditionalJumpCommand(byte[] stream, PackageMetadata metadata, int baseIndex)
        {
            stream = stream[baseIndex..];

            var segAddr = stream[1..];
            var trueLen = metadata.AddressAlignment + 1;
            var addr1 = CalculateRelativeAddress(segAddr, metadata.AddressAlignment);
            var addr2 = CalculateRelativeAddress(segAddr[trueLen..], metadata.AddressAlignment);
            var addr3 = CalculateRelativeAddress(segAddr[(trueLen * 2)..], metadata.AddressAlignment);

            return new(1 + (metadata.AddressAlignment + 1) * 3, new CommandTableEntry
            {
                Location = baseIndex,
                RawData = stream[0..(1 + (metadata.AddressAlignment + 1) * 3)],
                Description = $"Jump by condition ({addr1}, {addr2}, {addr3})"
            });
        }

        private static long CalculateRelativeAddress(byte[] raw, int addrLen)
        {
            long result = Utils.ByteToInt64WithArrayLen(raw[1..], addrLen);

            if (raw[0] == 0x0B)
            {
                result = -result;
            }

            return result;
        }
    }
}
