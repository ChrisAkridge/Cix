using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    internal sealed class StartEndVertices : IConnectable
    {
        public ControlFlowVertex Start { get; set; }
        public ControlFlowVertex End { get; set; }

        public ControlFlowVertex ConnectionTarget => Start;

        public void ConnectTo(IConnectable other, FlowEdgeType flowEdgeType)
        {
            End.ConnectTo(other, flowEdgeType);
        }
    }
}