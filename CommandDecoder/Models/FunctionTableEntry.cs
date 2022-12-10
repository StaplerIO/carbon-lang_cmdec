using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandDecoder.Models
{
    public class FunctionTableEntry
    {
        public int FunctionId { get; set; }

        public long EntryPointAddress { get; set; }
    }
}
