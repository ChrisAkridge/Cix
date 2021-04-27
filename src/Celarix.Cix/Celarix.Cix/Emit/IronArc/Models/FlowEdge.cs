using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    internal sealed class FlowEdge
    {
        public ControlFlowVertex Source { get; set; }
        public ControlFlowVertex Destination { get; set; }
        public FlowEdgeType FlowEdgeType { get; set; }

        public string GenerateInstructionText()
        {
            var mnemonic = FlowEdgeType switch
            {
                FlowEdgeType.UnconditionalJump => "jmp",
                FlowEdgeType.JumpIfEqual => "je",
                FlowEdgeType.JumpIfNotEqual => "jne",
                FlowEdgeType.JumpIfLessThan => "jlt",
                FlowEdgeType.JumpIfGreaterThan => "jgt",
                FlowEdgeType.JumpIfLessThanOrEqualTo => "jlte",
                FlowEdgeType.JumpIfGreaterThanOrEqualTo => "jgte",
                _ => throw new InvalidOperationException("Internal compiler error: Wrong flow edge type")
            };

            return $"{mnemonic} {Destination.JumpLabel}";
        }
    }
}