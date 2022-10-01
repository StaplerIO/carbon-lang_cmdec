using CommandDecoder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandDecoder.Decoder
{
    internal class MathCommandDecoder
    {
        internal static DecodeResult DecodeCalcCommand(byte[] stream, int baseIndex)
        {
            stream = stream[baseIndex..];

            switch (stream[1])
            {
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
                case 0x04:
                    return new(2, new CommandTableEntry
                    {
                        Location = baseIndex,
                        RawData = stream[0..2],
                        Description = "Divide 2 values on the top of the stack"
                    });
                case 0x05:
                    return new(2, new CommandTableEntry
                    {
                        Location = baseIndex,
                        RawData = stream[0..2],
                        Description = "Modulo 2 values on the top of the stack"
                    });
                case 0x06:
                    return new(2, new CommandTableEntry
                    {
                        Location = baseIndex,
                        RawData = stream[0..2],
                        Description = "Inverse the top value of the stack"
                    });
                default:
                    throw new InvalidOperationException("No such math command");
            }
        }

        internal static DecodeResult DecodeLogicalCommand(byte[] stream, int baseIndex)
        {
            stream = stream[baseIndex..];

            switch (stream[1])
            {
                case 0x01:
                    return new(2, new CommandTableEntry
                    {
                        Location = baseIndex,
                        RawData = stream[0..2],
                        Description = "Do AND logical calculation"
                    });
                case 0x02:
                    return new(2, new CommandTableEntry
                    {
                        Location = baseIndex,
                        RawData = stream[0..2],
                        Description = "Do OR logical calculation"
                    });
                case 0x03:
                    return new(2, new CommandTableEntry
                    {
                        Location = baseIndex,
                        RawData = stream[0..2],
                        Description = "Do NOT logical calculation"
                    });
                default:
                    throw new InvalidOperationException("No such math command");
            }
        }
    }
}
