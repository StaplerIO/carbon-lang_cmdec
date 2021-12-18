using CommandDecoder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandDecoder
{
    public class CbpManager
    {
        public static PackageMetadata? ReadMetadata(byte[] content)
        {
            if (content.Length > 5)
            {
                return new PackageMetadata
                {
                    VariableSlotAlignment = content[0],
                    DataAlignment = content[1],
                    CommandAlignment = content[2],
                    EntryPointOffset = content[3],
                    DomainLayerCountAlignment = content[4],
                };
            }

            return null;
        }
    }
}
