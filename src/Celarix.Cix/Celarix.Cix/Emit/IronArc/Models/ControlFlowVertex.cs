using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    internal abstract class ControlFlowVertex : IConnectable
    {
        private string jumpLabel;
        
        public List<FlowEdge> OutboundEdges { get; set; } = new List<FlowEdge>();
        public bool IsJumpTarget { get; set; }
        public string JumpLabel => !IsJumpTarget ? "" : jumpLabel ??= JumpLabelGenerator.GenerateLabel();
        public JumpTargetType JumpTargetType { get; set; }
        public int VisitCount { get; set; }

        public ControlFlowVertex ConnectionTarget => this;

        public void ConnectTo(IConnectable other, FlowEdgeType flowEdgeType)
        {
            if (this is InstructionVertex instructionVertex
                && instructionVertex.Mnemonic == "mov"
                && instructionVertex.OperandSize == OperandSize.Qword
                && instructionVertex.Operand1 is IntegerOperand integerOperand
                && integerOperand.ValueBits == 0
                && instructionVertex.Operand2 is RegisterOperand registerOperand
                && registerOperand.Register == Register.ECX
                && OutboundEdges.Count(e => e.FlowEdgeType == FlowEdgeType.DirectFlow) == 1)
            {
                System.Diagnostics.Debugger.Break();
            }

            if (OutboundEdges.Count(e => e.FlowEdgeType == FlowEdgeType.DirectFlow) == 1)
            {
                return;
            }
            
            OutboundEdges.Add(new FlowEdge
            {
                Source = this,
                Destination = other.ConnectionTarget,
                FlowEdgeType = flowEdgeType
            });
        }

        public abstract string GenerateInstructionText();
    }
}