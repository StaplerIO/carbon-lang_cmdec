using CommandDecoder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandDecoder.Decoder
{
    public class StackCommandDecoder
    {
        public static DecodeResult PushCommand(byte[] stream, PackageMetadata metadata, int baseIndex)
        {
            stream = stream[baseIndex..];

            return new(1 + metadata.DataAlignment, new CommandTableEntry
            {
                Location = baseIndex,
                RawData = stream[0..(metadata.DataAlignment + 1)],
                Description = $"Push data to stack"
            });
        }

        public static DecodeResult PopToObjectCommand(byte[] stream, PackageMetadata metadata, int baseIndex)
        {
            stream = stream[baseIndex..];

            return new(2 + metadata.VariableSlotAlignment, new CommandTableEntry
            {
                Location = baseIndex,
                RawData = stream[0..(2 + metadata.VariableSlotAlignment)],
                Description = $"Pop data from stack to data slot"
            });
        }
    }
}
