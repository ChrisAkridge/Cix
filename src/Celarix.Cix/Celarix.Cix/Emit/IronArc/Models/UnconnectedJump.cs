using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    internal sealed class UnconnectedJump
    {
        public ControlFlowVertex JumpVertex { get; set; }
        public FlowEdgeType FlowType { get; set; }
        public JumpTargetType TargetType { get; set; }
    }
}
