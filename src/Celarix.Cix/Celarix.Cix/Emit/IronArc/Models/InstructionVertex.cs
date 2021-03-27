﻿using System;
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

        public InstructionVertex(string mnemonic)
        {
            Mnemonic = mnemonic;
        }
        
        public InstructionVertex(string mnemonic, OperandSize operandSize, InstructionOperand operand1 = null,
            InstructionOperand operand2 = null, InstructionOperand operand3 = null)
        {
            Mnemonic = mnemonic;
            OperandSize = operandSize;
            Operand1 = operand1;
            Operand2 = operand2;
            Operand3 = operand3;
        }
    }
}