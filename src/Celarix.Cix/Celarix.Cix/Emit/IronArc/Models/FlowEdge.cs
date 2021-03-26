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
    }
}