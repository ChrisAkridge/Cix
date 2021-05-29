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

            if (!context.BreakContexts.TryPeek(out var breakContext))
            {
                throw new InvalidOperationException("Cannot break from this statement");
            }

            var stackSubtrahend = context.CurrentStack.Size - breakContext.StackSizeAtStart;
            var resetStack = new InstructionVertex("subl", OperandSize.Qword, EmitHelpers.Register(Register.ESP),
                new IntegerOperand(stackSubtrahend), EmitHelpers.Register(Register.ESP));

            return new GeneratedFlow
            {
                ControlFlow = EmitHelpers.ConnectWithDirectFlow(new IConnectable[]
                {
                    codeComment,
                    resetStack
                }),
                UnconnectedJumps = new List<UnconnectedJump>
                {
                    new UnconnectedJump
                    {
                        SourceVertex = resetStack,
                        FlowType = FlowEdgeType.UnconditionalJump,
                        TargetType = JumpTargetType.ToBreakTarget
                    }
                }
            };
        }
    }
}