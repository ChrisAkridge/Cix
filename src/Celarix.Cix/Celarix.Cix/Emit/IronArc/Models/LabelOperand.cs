using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    internal sealed class LabelOperand : InstructionOperand
    {
        public string Label { get; set; }

        public LabelOperand(string label) => Label = label;
        public override string GenerateOperandText() => $"{((IsPointer) ? "*" : "")}{Label}";
    }
}
