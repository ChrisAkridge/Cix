using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    internal abstract class ControlFlowVertex : IConnectable
    {
        public List<FlowEdge> OutboundEdges { get; set; } = new List<FlowEdge>();
        public bool IsJumpTarget { get; set; }
        public JumpTargetType JumpTargetType { get; set; }

        public ControlFlowVertex ConnectionTarget => this;

        public void ConnectTo(IConnectable other, FlowEdgeType flowEdgeType)
        {
            OutboundEdges.Add(new FlowEdge
            {
                Source = this,
                Destination = other.ConnectionTarget,
                FlowEdgeType = flowEdgeType
            });
        }
    }
}