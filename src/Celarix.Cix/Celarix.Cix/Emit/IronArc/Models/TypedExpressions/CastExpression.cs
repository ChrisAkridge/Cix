using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions
{
    internal sealed class CastExpression : TypedExpression
    {
        public UsageTypeInfo ToType { get; set; }
        public TypedExpression Expression { get; set; }

        public override UsageTypeInfo ComputeType(ExpressionEmitContext context, TypedExpression parent)
        {
            var expressionType = Expression.ComputeType(context, this);

            if (ExpressionHelpers.IsStruct(expressionType, context.DeclaredTypes)
                || ExpressionHelpers.IsStruct(ToType, context.DeclaredTypes))
            {
                throw new InvalidOperationException("Structs are not convertible");
            }
            else if (expressionType.DeclaredType is FuncptrTypeInfo && ToType.DeclaredType is FuncptrTypeInfo)
            {
                throw new InvalidOperationException("Function pointer types can't be directly converted into each other");
            }
            
            ComputedType = ToType;
            return ToType;
        }

        public override StartEndVertices Generate(ExpressionEmitContext context, TypedExpression parent) =>
            Expression.ComputedType.Size != ToType.Size
                ? EmitHelpers.ConnectWithDirectFlow(new IConnectable[]
                {
                    EmitHelpers.ChangeWidthOfTopOfStack(EmitHelpers.ToOperandSize(Expression.ComputedType.Size),
                        EmitHelpers.ToOperandSize(ToType.Size))
                })
                : null;
    }
}