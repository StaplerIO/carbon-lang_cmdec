using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandDecoder.Models
{
    public class PackageMetadata
    {
        public byte VariableSlotAlignment { get; set; }
        
        public byte DataAlignment { get; set; }

        public byte CommandAlignment { get; set; }

        public byte EntryPointOffset { get; set; }

        public byte DomainLayerCountAlignment { get; set; }
    }
}
