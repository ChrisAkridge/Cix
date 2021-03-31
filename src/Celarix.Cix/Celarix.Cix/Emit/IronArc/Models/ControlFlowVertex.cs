using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    internal abstract class ControlFlowVertex : IConnectable
    {
        public FlowEdge OutboundEdge { get; set; }
        public bool IsJumpTarget { get; set; }

        public ControlFlowVertex ConnectionTarget => this;

        public void ConnectTo(IConnectable other, FlowEdgeType flowEdgeType)
        {
            OutboundEdge = new FlowEdge
            {
                Source = this,
                Destination = other.ConnectionTarget,
                FlowEdgeType = flowEdgeType
            };
        }
    }
}