using CommandDecoder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandDecoder.Decoder
{
    public class ObjectCommandDecoder
    {
        public static DecodeResult CreateObjectDecoder(byte[] stream, int baseIndex)
        {
            stream = stream[baseIndex..];

            return new(2, new CommandTableEntry
            {
                Location = baseIndex,
                RawData = stream[0..2],
                Description = $"Create an object"
            });
        }

        public static DecodeResult DestroyObjectDecoder(byte[] stream, PackageMetadata metadata, int baseIndex)
        {
            stream = stream[baseIndex..];

            if (stream[1] == 0x10)
            {
                // Data is saved on public area
                return new(2 + metadata.DataSlotAlignment, new CommandTableEntry
                {
                    Location = baseIndex,
                    RawData = stream[0..(2 + metadata.DataSlotAlignment)],
                    Description = $"Destroy a public object on slot 0x{BitConverter.ToString(stream[2..(metadata.DataSlotAlignment + 2)]).Replace("-", string.Empty)}"
                });
            }
            else
            {
                // This is a private data
                return new(2 + metadata.DomainLayerCountAlignment + metadata.DataSlotAlignment, new CommandTableEntry
                {
                    Location = baseIndex,
                    RawData = stream[0..(2 + metadata.DataSlotAlignment)],
                    Description = $"Destroy a private object on slot 0x{BitConverter.ToString(stream[2..(metadata.DataSlotAlignment + 2)]).Replace("-", string.Empty)}"
                });
            }
        }
    }
}
