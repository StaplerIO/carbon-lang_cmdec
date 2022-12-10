using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandDecoder.Models
{
    public class ConstantPoolEntry
    {
        public long Slot { get; set; }

        public byte[] Content { get; set; } = ""u8.ToArray();
    }
}
