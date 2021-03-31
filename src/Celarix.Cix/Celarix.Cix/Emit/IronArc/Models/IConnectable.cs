using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    internal interface IConnectable
    {
        public ControlFlowVertex ConnectionTarget { get; }
        public void ConnectTo(IConnectable other, FlowEdgeType flowEdgeType);
    }
}
