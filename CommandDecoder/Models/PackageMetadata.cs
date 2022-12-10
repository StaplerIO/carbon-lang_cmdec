using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CommandDecoder.Models
{
    public class PackageMetadata
    {
        public byte PackageType { get; set; }

        public byte DataAlignment { get; set; }

        public byte DomainLayerCountAlignment { get; set; }

        public byte DataSlotAlignment { get; set; }

        public int EntryPointAddress { get; set; }

        public byte AddressAlignment { get; set; }

        public Dictionary<string, string> GetMetadataList()
        {
            return new Dictionary<string, string>
            {
                { "PackageType", PackageType == 0 ? "0 (executable)" : "1 (library)" },
                { "DataAlignment", DataAlignment.ToString() },
                { "DomainLayerCountAlignment", DomainLayerCountAlignment.ToString() },
                { "DataSlotAlignment", DataSlotAlignment.ToString() },
                { "AddressAlignment", AddressAlignment.ToString() },
            };
        }
    }
}
