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

            if (!context.BreakContexts.TryPeek(out var breakContext) || !breakContext.SupportsContinue)
            {
                throw new InvalidOperationException("Cannot continue in this statement");
            }

            var stackSubtrahend = context.CurrentStack.Size - breakContext.StackSizeAtStart;
            var resetStack = new InstructionVertex("subl", OperandSize.Qword, EmitHelpers.Register(Register.ESP),
                new IntegerOperand(stackSubtrahend), EmitHelpers.Register(Register.ESP));

            return new GeneratedFlow
            {
                ControlFlow = EmitHelpers.ConnectWithDirectFlow(new IConnectable[]
                {
                    codeComment
                }),
                UnconnectedJumps = new List<UnconnectedJump>
                {
                    new UnconnectedJump
                    {
                        SourceVertex = resetStack,
                        FlowType = FlowEdgeType.UnconditionalJump,
                        TargetType = JumpTargetType.ToContinueTarget
                    }
                }
            };
        }
    }
}