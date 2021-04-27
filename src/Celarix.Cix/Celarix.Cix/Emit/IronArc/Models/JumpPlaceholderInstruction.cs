using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    /// <summary>
    /// A vertex returned from statements that only perform a jump to somewhere
    /// else. Since jump instructions are represented as edges on the graph,
    /// we use this vertex to hold an edge out to the jump target. This vertex
    /// emits no instructions.
    /// </summary>
    internal sealed class JumpPlaceholderInstruction : ControlFlowVertex
    {
        public override string GenerateInstructionText() => "";
    }
}
