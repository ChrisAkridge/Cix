using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    internal sealed class RegisterOperand : InstructionOperand
    {
        public Register Register { get; set; }
        public int Offset { get; set; }
    }
}