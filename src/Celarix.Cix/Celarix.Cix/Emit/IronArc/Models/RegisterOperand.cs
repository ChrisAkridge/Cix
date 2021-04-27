using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    internal sealed class RegisterOperand : InstructionOperand
    {
        public Register Register { get; set; }
        public int Offset { get; set; }

        public override string GenerateOperandText()
        {
            var registerText = $"{((IsPointer) ? "*" : "")}{Register}";

            if (Offset == 0) { return registerText; }

            string offsetText = Offset.ToString();

            if (Offset >= 1) { offsetText = "+" + offsetText; }

            return $"{registerText}{offsetText}";
        }
    }
}