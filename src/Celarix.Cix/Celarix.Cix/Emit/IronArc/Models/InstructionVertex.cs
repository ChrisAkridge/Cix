using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    internal sealed class InstructionVertex : ControlFlowVertex
    {
        public string Mnemonic { get; set; }
        public OperandSize OperandSize { get; set; }
        public InstructionOperand Operand1 { get; set; }
        public InstructionOperand Operand2 { get; set; }
        public InstructionOperand Operand3 { get; set; }

        public InstructionVertex(string mnemonic) => Mnemonic = mnemonic;

        public InstructionVertex(string mnemonic, OperandSize operandSize, InstructionOperand operand1 = null,
            InstructionOperand operand2 = null, InstructionOperand operand3 = null)
        {
            Mnemonic = mnemonic;
            OperandSize = operandSize;
            Operand1 = operand1;
            Operand2 = operand2;
            Operand3 = operand3;
        }

        public override string GenerateInstructionText()
        {
            var jumpLabelText = (IsJumpTarget)
                ? $"{JumpLabel}:{Environment.NewLine}"
                : "";
            
            var operandSizeText = OperandSize switch
            {
                OperandSize.Byte => "BYTE ",
                OperandSize.Word => "WORD ",
                OperandSize.Dword => "DWORD ",
                OperandSize.Qword => "QWORD ",
                _ => ""
            };

            var operand1Text = Operand1?.GenerateOperandText() + " " ?? "";
            var operand2Text = Operand2?.GenerateOperandText() + " " ?? "";
            var operand3Text = Operand3?.GenerateOperandText() + " " ?? "";

            return $"{jumpLabelText}{Mnemonic} {operandSizeText}{operand1Text}{operand2Text}{operand3Text}".TrimEnd();
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() => GenerateInstructionText();
    }
}