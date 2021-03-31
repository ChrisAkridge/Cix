using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions
{
    internal sealed class UnaryExpression : TypedExpression
    {
        public TypedExpression Operand { get; set; }
        public string Operator { get; set; }
        public bool IsPostfix { get; set; }

        public override UsageTypeInfo ComputeType(ExpressionEmitContext context, TypedExpression parent)
        {
            Operand.ComputeType(context, this);

            IsAssignable = Operator == "&";

            return ExpressionHelpers.GetOperatorKind(Operator,
                    (IsPostfix) ? OperationKind.PostfixUnary : OperationKind.PrefixUnary) switch
                {
                    OperatorKind.Sign => ComputeTypeForSign(),
                    OperatorKind.IncrementDecrement => ComputeTypeForIncrementDecrement(),
                    OperatorKind.PointerOperation => ComputeTypeForPointerOperation(),
                    _ => throw new InvalidOperationException("Internal compiler error: unexpected unary operator")
                };
        }

        private UsageTypeInfo ComputeTypeForSign() =>
            !ExpressionHelpers.IsNumeric(Operand.ComputedType)
                ? throw new InvalidOperationException("Cannot apply operator to non-numeric value")
                : Operand.ComputedType;

        private UsageTypeInfo ComputeTypeForIncrementDecrement()
        {
            if (!Operand.IsAssignable || !ExpressionHelpers.IsNumeric(Operand.ComputedType))
            {
                throw new InvalidOperationException("Cannot increment or decrement this variable");
            }

            ComputedType = Operand.ComputedType;

            return ComputedType;
        }

        private UsageTypeInfo ComputeTypeForPointerOperation()
        {
            ComputedType = Operator switch
            {
                "*" when Operand.ComputedType.PointerLevel < 1 => throw new InvalidOperationException(
                    "Cannot dereference a non-pointer"),
                "*" => new UsageTypeInfo
                {
                    DeclaredType = Operand.ComputedType.DeclaredType,
                    PointerLevel = Operand.ComputedType.PointerLevel - 1
                },
                "->" when !Operand.IsAssignable => throw new InvalidOperationException(
                    $"Cannot get address of value expression"),
                "->" => new UsageTypeInfo
                {
                    DeclaredType = Operand.ComputedType.DeclaredType,
                    PointerLevel = Operand.ComputedType.PointerLevel + 1
                },
                _ => throw new InvalidOperationException("Internal compiler error: unrecognized pointer operator")
            };

            return ComputedType;
        }
    }
}