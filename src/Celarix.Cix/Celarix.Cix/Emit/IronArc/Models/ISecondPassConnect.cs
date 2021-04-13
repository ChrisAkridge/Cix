using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    internal interface ISecondPassConnect
    {
        void ConnectToGeneratedTree(ControlFlowVertex after, EmitContext context = null);
    }
}
