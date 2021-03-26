using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celarix.Cix.Compiler.Parse.Models.AST.v1;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    internal sealed class FunctionEmitContext
    {
        public Function Function { get; set; }
        public VirtualStack Stack { get; set; } = new VirtualStack();
        public Stack<ControlFlowVertex> BreakTargets { get; set; } = new Stack<ControlFlowVertex>();
        public Stack<ControlFlowVertex> ContinueTargets { get; set; } = new Stack<ControlFlowVertex>();
    }
}
