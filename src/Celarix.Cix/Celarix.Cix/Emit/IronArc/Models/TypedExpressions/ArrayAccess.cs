using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions
{
    internal sealed class ArrayAccess : TypedExpression
    {
        public TypedExpression Operand { get; set; }
        public TypedExpression Index { get; set; }

        public override UsageTypeInfo ComputeType(TypeComputationContext context, TypedExpression parent)
        {
            // TArray* x;
            // int y;
            //
            // TArray result = x[y];

            var operandType = Operand.ComputeType(context, this);
            var indexType = Operand.ComputeType(context, this);

            if (operandType.PointerLevel < 1)
            {
                throw new InvalidOperationException("Cannot perform array access on non-pointer type");
            }

            if (!ExpressionHelpers.ImplicitlyConvertibleToInt(indexType))
            {
                throw new InvalidOperationException("Array access indexer is not int or a type convertible to int");
            }

            ComputedType = new UsageTypeInfo
            {
                DeclaredType = operandType.DeclaredType,
                PointerLevel = operandType.PointerLevel - 1
            };

            return ComputedType;
        }
    }
}