using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandDecoder.Model
{
    public class CommandTableEntry
    {
        public long Location { get; set; }

        public byte[] RawData { get; set; }
    }
}
