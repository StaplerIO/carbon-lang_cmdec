using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandDecoder.Models
{
    public class DecodedPackage
    {
        public PackageMetadata Metadata { get; set; }

        public string[] StringPool { get; set; }

        public FunctionTableEntry[] FunctionTable { get; set; }

        public CommandTableEntry[] CommandTable { get; set; }
    }
}
