using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandDecoder.Models
{
    public class DecodeResult
    {
        public DecodeResult(int length, CommandTableEntry command)
        {
            Length = length;
            Command = command;
        }

        public int Length { get; set; }

        public CommandTableEntry Command { get; set; }
    }
}
