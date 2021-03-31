using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions
{
    internal sealed class FunctionInvocation : TypedExpression
    {
        public TypedExpression Operand { get; set; }
        public List<TypedExpression> Arguments { get; set; }

        public override UsageTypeInfo ComputeType(ExpressionEmitContext context, TypedExpression parent)
        {
            var operandType = Operand.ComputeType(context, this);

            foreach (var argument in Arguments)
            {
                argument.ComputeType(context, this);
            }

            if (!(operandType.DeclaredType is FuncptrTypeInfo))
            {
                throw new InvalidOperationException("Cannot invoke non-function");
            }

            ComputedType = operandType;
            return ComputedType;
        }
    }
}