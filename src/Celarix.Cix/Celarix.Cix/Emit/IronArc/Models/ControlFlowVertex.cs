using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    internal abstract class ControlFlowVertex
    {
        public FlowEdge OutboundEdge { get; set; }
        public bool IsJumpTarget { get; set; }

        public void ConnectTo(ControlFlowVertex other, FlowEdgeType flowEdgeType)
        {
            OutboundEdge = new FlowEdge
            {
                Source = this,
                Destination = other,
                FlowEdgeType = flowEdgeType
            };
        }
    }
}