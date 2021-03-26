using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    internal sealed class FloatingPointOperand : InstructionOperand
    {
        public FloatSize FloatSize { get; set; }
        public ulong ValueBits { get; set; }
    }
}