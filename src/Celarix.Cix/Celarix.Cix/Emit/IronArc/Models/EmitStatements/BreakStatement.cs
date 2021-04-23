using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.EmitStatements
{
    internal sealed class BreakStatement : EmitStatement
    {
        public override GeneratedFlow Generate(EmitContext context, EmitStatement parent)
        {
            var jumpPlaceholder = new JumpPlaceholderInstruction();

            return new GeneratedFlow
            {
                ControlFlow = StartEndVertices.MakePair(jumpPlaceholder),
                UnconnectedJumps = new List<UnconnectedJump>
                {
                    new UnconnectedJump
                    {
                        JumpVertex = jumpPlaceholder,
                        FlowType = FlowEdgeType.UnconditionalJump,
                        TargetType = JumpTargetType.ToBreakOrAfterTarget
                    }
                }
            };
        }
    }
}