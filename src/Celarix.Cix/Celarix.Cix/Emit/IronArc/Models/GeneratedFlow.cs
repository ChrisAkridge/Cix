using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    internal sealed class GeneratedFlow
    {
        public StartEndVertices ControlFlow { get; set; }
        public List<UnconnectedJump> UnconnectedJumps { get; set; }

        public void ConnectTo(IConnectable other, FlowEdgeType flowEdgeType)
        {
            ControlFlow.End.ConnectTo(other, flowEdgeType);
        }

        public void ConnectTo(GeneratedFlow other, FlowEdgeType flowEdgeType)
        {
            ControlFlow.End.ConnectTo(other.ControlFlow, flowEdgeType);
        }
    }
}
