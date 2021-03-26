using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    internal sealed class StartEndVertices
    {
        public ControlFlowVertex Start { get; set; }
        public ControlFlowVertex End { get; set; }
    }
}