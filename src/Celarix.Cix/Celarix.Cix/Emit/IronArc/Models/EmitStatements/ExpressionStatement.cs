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
            var expressionFlow = Expression.Generate(context, null);
            codeComment.ConnectTo(expressionFlow, FlowEdgeType.DirectFlow);

            return new GeneratedFlow()
            {
                ControlFlow = new StartEndVertices(codeComment, expressionFlow.End),
                UnconnectedJumps = new List<UnconnectedJump>()
            };
        }
    }
}