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
            
            return new GeneratedFlow()
            {
                ControlFlow = Expression.Generate(context, null), UnconnectedJumps = new List<UnconnectedJump>()
            };
        }
    }
}