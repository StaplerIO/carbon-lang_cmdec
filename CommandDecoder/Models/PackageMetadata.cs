using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandDecoder.Models
{
    public class PackageMetadata
    {
        public byte PackageType { get; set; }

        public byte DataAlignment { get; set; }

        public byte DomainLayerCountAlignment { get; set; }

        public byte DataSlotAlignment { get; set; }

        public byte EntryPointAddress { get; set; }

        public byte AddressAlignment { get; set; }
    }
}
