using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandDecoder
{
    public class RootCommand
    {
        public const byte Object = 0xA;
        public const byte Stack = 0xB;
        public const byte Domain = 0xC;
        public const byte Jump = 0xD;
        public const byte Function = 0xE;
        public const byte Math = 0xF;
    }

    public class ObjectCommand
    {
        public const byte Create = 0x1;
        public const byte Destroy = 0x2;
    }

    public class StackCommand
    {
        public const byte Push = 0x1;
        public const byte PushFromObject = 0x2;
        public const byte Pop = 0x3;
        public const byte PopToObject = 0x4;
    }

    public class DomainCommand
    {
        public const byte Create = 0x1;
        public const byte Destroy = 0x2;
    }

    public class JumpCommand
    {
        public const byte ToOffset = 0x1;
        public const byte ByStackTop = 0x2;
    }

    public class MathCommand
    {
        public const byte Calculation = 0x1;
        public const byte Logical = 0x2;
    }

    public class MathCalcCommand
    {
        public const byte Plus = 0x1;
        public const byte Minus = 0x2;
        public const byte Times = 0x3;
        public const byte Divide = 0x4;
        public const byte Modulo = 0x5;
    }

    public class MathLogicalCommand
    {
        public const byte And = 0x1;
        public const byte Or = 0x2;
        public const byte Not = 0x3;
    }
}
