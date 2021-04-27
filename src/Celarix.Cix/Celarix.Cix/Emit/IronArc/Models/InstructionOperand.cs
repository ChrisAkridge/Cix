using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    internal abstract class InstructionOperand
    {
        public bool IsPointer { get; set; }
        public abstract string GenerateOperandText();
    }
}