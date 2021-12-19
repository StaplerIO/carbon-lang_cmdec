using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandDecoder.Models
{
    public class CommandTableEntry
    {
        public int Location { get; set; }

        public byte[] RawData { get; set; }

        public string Description { get; set; }
    }
}
