using System;
using System.Collections.Generic;
using System.Linq;
using NLog;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.EmitStatements
{
    internal sealed class BreakStatement : EmitStatement
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        
        public override GeneratedFlow Generate(EmitContext context, EmitStatement parent)
        {
            logger.Trace("Generating code for break statement...");
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
                        SourceVertex = jumpPlaceholder,
                        FlowType = FlowEdgeType.UnconditionalJump,
                        TargetType = JumpTargetType.ToBreakOrAfterTarget
                    }
                }
            };
        }
    }
}