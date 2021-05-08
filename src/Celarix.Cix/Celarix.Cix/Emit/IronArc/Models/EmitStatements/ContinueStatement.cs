using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.EmitStatements
{
    internal sealed class ContinueStatement : EmitStatement
    {
        public override GeneratedFlow Generate(EmitContext context, EmitStatement parent)
        {
            var codeComment = new CommentPrinterVertex(OriginalCode);
            var jumpPlaceholder = new JumpPlaceholderInstruction();

            return new GeneratedFlow
            {
                ControlFlow = EmitHelpers.ConnectWithDirectFlow(new IConnectable[]
                {
                    codeComment,
                    jumpPlaceholder
                }),
                UnconnectedJumps = new List<UnconnectedJump>
                {
                    new UnconnectedJump
                    {
                        JumpVertex = jumpPlaceholder,
                        FlowType = FlowEdgeType.UnconditionalJump,
                        TargetType = JumpTargetType.ToContinueTarget
                    }
                }
            };
        }
    }
}