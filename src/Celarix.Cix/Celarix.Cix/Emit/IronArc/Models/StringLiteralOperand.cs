using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    internal sealed class StringLiteralOperand : InstructionOperand
    {
        public string Literal { get; set; }
    }
}