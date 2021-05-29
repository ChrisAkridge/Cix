using System;
using System.Collections.Generic;
using System.Linq;
using Celarix.Cix.Compiler.Extensions;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    internal sealed class StringLiteralOperand : InstructionOperand
    {
        public string Literal { get; set; }
        public override string GenerateOperandText() => $"\"{Literal.ToLiteral()}\"";
    }
}