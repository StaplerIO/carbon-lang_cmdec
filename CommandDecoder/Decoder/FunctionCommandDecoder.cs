using CommandDecoder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandDecoder.Decoder
{
    internal class FunctionCommandDecoder
    {
        public static DecodeResult EnterFunction(byte[] stream, PackageMetadata metadata, int baseIndex)
        {
            stream = stream[baseIndex..];

            return new(1 + 1 + metadata.AddressAlignment + 1, new CommandTableEntry
            {
                Location = baseIndex,
                RawData = stream[0..(1 + metadata.AddressAlignment)],
                Description = "Enter function"
            });
        }

        public static DecodeResult LeaveFunctionWithoutValue(byte[] stream, int baseIndex)
        {
            stream = stream[baseIndex..];

            return new(1, new CommandTableEntry
            {
                Location = baseIndex,
                RawData = stream[0..1],
                Description = "Leave current function without value"
            });
        }

        public static DecodeResult LeaveFunctionWithValue(byte[] stream, int baseIndex)
        {
            stream = stream[baseIndex..];

            return new(1, new CommandTableEntry
            {
                Location = baseIndex,
                RawData = stream[0..1],
                Description = "Leave current function with value on stack top"
            });
        }
    }
}
