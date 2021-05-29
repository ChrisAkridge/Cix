using System;
using System.Collections.Generic;
using System.Linq;
using Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.EmitStatements
{
    internal sealed class WhileStatement : EmitStatement
    {
        public TypedExpression Condition { get; set; }
        public EmitStatement LoopStatement { get; set; }

        public override GeneratedFlow Generate(EmitContext context, EmitStatement parent)
        {
            context.BreakContexts.Push(new BreakContext
            {
                StackSizeAtStart = context.CurrentStack.Size,
                SupportsContinue = true
            });
            
            Condition.ComputeType(context, null);
            
            var conditionFlow = Condition.Generate(context, null);
            var commentPrinterVertex = new CommentPrinterVertex(OriginalCode)
            {
                IsJumpTarget = true
            };

            var comparisonFlow = EmitHelpers.ConnectWithDirectFlow(new IConnectable[]
            {
                commentPrinterVertex, 
                conditionFlow,
                EmitHelpers.ChangeWidthOfTopOfStack(context, EmitHelpers.ToOperandSize(Condition.ComputedType.Size),
                    OperandSize.Dword),
                new InstructionVertex("push", OperandSize.Dword, new IntegerOperand(0)),
                new InstructionVertex("cmp", OperandSize.Dword),
            });

            context.CurrentStack.Pop();

            var loopFlow = LoopStatement.Generate(context, this);

            foreach (var jump in loopFlow.UnconnectedJumps.Where(j => j.TargetType == JumpTargetType.ToContinueTarget))
            {
                jump.SourceVertex.ConnectTo(comparisonFlow, jump.FlowType);
            }

            loopFlow.UnconnectedJumps.RemoveAll(j => j.TargetType == JumpTargetType.ToContinueTarget);
            
            comparisonFlow.ConnectTo(loopFlow, FlowEdgeType.JumpIfNotEqual);
            loopFlow.ConnectTo(comparisonFlow, FlowEdgeType.UnconditionalJump);
            var jumpToAfter = new UnconnectedJump
            {
                SourceVertex = comparisonFlow.End,
                FlowType = FlowEdgeType.JumpIfEqual,
                TargetType = JumpTargetType.ToAfterTarget
            };

            return new GeneratedFlow
            {
                ControlFlow = comparisonFlow,
                UnconnectedJumps = loopFlow.UnconnectedJumps.Append(jumpToAfter).ToList()
            };
        }
    }
}