using CommandDecoder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandDecoder.Decoder
{
    internal class DomainCommandDecoder
    {
        internal static DecodeResult CreateDomainCommand(byte[] stream, PackageMetadata metadata, int baseIndex)
        {
            stream = stream[baseIndex..];

            return new(1, new CommandTableEntry
            {
                Location = baseIndex,
                RawData = stream[0..1],
                Description = "Create domain"
            });
        }

        internal static DecodeResult DestroyDomainCommand(byte[] stream, PackageMetadata metadata, int baseIndex)
        {
            stream = stream[baseIndex..];

            return new(1, new CommandTableEntry
            {
                Location = baseIndex,
                RawData = stream[0..1],
                Description = "Destroy domain"
            });
        }
    }
}
