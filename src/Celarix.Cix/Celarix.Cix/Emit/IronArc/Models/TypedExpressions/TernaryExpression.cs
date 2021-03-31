using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions
{
    internal sealed class TernaryExpression : TypedExpression
    {
        public TypedExpression Operand1 { get; set; }
        public string Operator1 { get; set; }
        public TypedExpression Operand2 { get; set; }
        public string Operator2 { get; set; }
        public TypedExpression Operand3 { get; set; }

        public override UsageTypeInfo ComputeType(ExpressionEmitContext context, TypedExpression parent)
        {
            var operand1Type = Operand1.ComputeType(context, this);
            var operand2Type = Operand2.ComputeType(context, this);
            var operand3Type = Operand3.ComputeType(context, this);

            if (!ExpressionHelpers.ImplicitlyConvertibleToInt(operand1Type))
            {
                throw new InvalidOperationException("Ternary conditional operator doesn't start with condition");
            }

            ComputedType = ExpressionHelpers.GetCommonType(operand2Type, operand3Type);
            return ComputedType;
        }
    }
}