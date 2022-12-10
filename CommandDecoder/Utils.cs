using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandDecoder
{
    internal class Utils
    {
        internal static long ByteToInt64WithArrayLen(byte[] raw, int len)
        {
            long result = 0;
            foreach (byte b in raw[0..len])
            {
                result = result * 0x10 + b;
            }

            return result;
        }
    }
}
