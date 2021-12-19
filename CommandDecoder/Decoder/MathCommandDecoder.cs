using CommandDecoder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandDecoder.Decoder
{
    public class MathCommandDecoder
    {
        public static DecodeResult DecodeCalcCommand(byte[] stream, PackageMetadata metadata, int baseIndex)
        {
            stream = stream[baseIndex..];

            switch (stream[1]){
                case 0x01:
                    return new(2, new CommandTableEntry
                    {
                        Location = baseIndex,
                        RawData = stream[0..2],
                        Description = "Sum 2 values on the top of the stack"
                    });
                case 0x02:
                    return new(2, new CommandTableEntry
                    {
                        Location = baseIndex,
                        RawData = stream[0..2],
                        Description = "Subtract 2 values on the top of the stack"
                    });
                case 0x03:
                    return new(2, new CommandTableEntry
                    {
                        Location = baseIndex,
                        RawData = stream[0..2],
                        Description = "Multiply 2 values on the top of the stack"
                    });
            }

            throw new InvalidOperationException("No such math command");
        }
    }
}
