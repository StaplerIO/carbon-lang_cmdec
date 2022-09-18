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

            string dataType;
            switch (stream[2])
            {
                case 0x00:
                    dataType = "an instant value";
                    break;
                case 0x01:
                    dataType = "a local variable";
                    break;
                case 0x03:
                    dataType = "a string from constant pool";
                    break;
                default:
                    dataType = "<unknown>";
                    break;
            }

            return new(1 + 1 + metadata.DataAlignment, new CommandTableEntry
            {
                Location = baseIndex,
                RawData = stream[0..(1 + 1 + metadata.DataAlignment)],
                Description = $"Push {dataType} to stack top"
            });
        }

        public static DecodeResult PushFromObjectCommand(byte[] stream, PackageMetadata metadata, int baseIndex)
        {
            stream = stream[baseIndex..];

            return new(2 + metadata.DataSlotAlignment, new CommandTableEntry
            {
                Location = baseIndex,
                RawData = stream[0..(2 + metadata.DataSlotAlignment)],
                Description = $"Push data from variable"
            });
        }

        public static DecodeResult PopToObjectCommand(byte[] stream, PackageMetadata metadata, int baseIndex)
        {
            stream = stream[baseIndex..];

            return new(2 + metadata.DataSlotAlignment, new CommandTableEntry
            {
                Location = baseIndex,
                RawData = stream[0..(2 + metadata.DataSlotAlignment)],
                Description = $"Pop data from stack to data slot"
            });
        }

        public static DecodeResult PopCommand(byte[] stream, PackageMetadata metadata, int baseIndex)
        {
            stream = stream[baseIndex..];

            return new(1, new CommandTableEntry
            {
                Location = baseIndex,
                RawData = stream[0..1],
                Description = $"Pop data from stack to air"
            });
        }
    }
}
