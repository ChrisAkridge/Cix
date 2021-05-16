using System;
using System.Collections.Generic;
using System.Linq;
using Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.EmitStatements
{
    internal sealed class ExpressionStatement : EmitStatement
    {
        public TypedExpression Expression { get; set; }

        public override GeneratedFlow Generate(EmitContext context, EmitStatement parent)
        {
            Expression.ComputeType(context, null);

            var codeComment = new CommentPrinterVertex(OriginalCode);

            var stackSizeBeforeExpression = context.CurrentStack.Size;
            var expressionFlow = Expression.Generate(context, null);
            var resetStackAfterExpression = Expression.ComputedType.Size > 0;
            var removeResultFromStack = (resetStackAfterExpression)
                ? new InstructionVertex("subl", OperandSize.Qword,
                    EmitHelpers.Register(Register.ESP), new IntegerOperand(Expression.ComputedType.Size),
                    EmitHelpers.Register(Register.ESP))
                : null;
            codeComment.ConnectTo(expressionFlow, FlowEdgeType.DirectFlow);

            if (resetStackAfterExpression)
            {
                expressionFlow.ConnectTo(removeResultFromStack, FlowEdgeType.DirectFlow);

                while (context.CurrentStack.Size > stackSizeBeforeExpression)
                {
                    context.CurrentStack.Pop();
                }
            }

            return new GeneratedFlow()
            {
                ControlFlow = new StartEndVertices(codeComment, (resetStackAfterExpression) ? removeResultFromStack : expressionFlow.End),
                UnconnectedJumps = new List<UnconnectedJump>()
            };
        }
    }
}