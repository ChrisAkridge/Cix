using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions
{
    internal sealed class BinaryExpression : TypedExpression
    {
        private static readonly UsageTypeInfo intType =
            new UsageTypeInfo
            {
                DeclaredType = new NamedTypeInfo
                {
                    Name = "int", Size = 4
                }
            };

        public TypedExpression Left { get; set; }
        public TypedExpression Right { get; set; }
        public string Operator { get; set; }

        public override UsageTypeInfo ComputeType(TypeComputationContext context, TypedExpression parent)
        {
            Left.ComputeType(context, this);
            Right.ComputeType(context, this);

            return ExpressionHelpers.GetOperatorKind(Operator, OperationKind.Binary) switch
            {
                OperatorKind.Arithmetic => ComputeTypeForArithmetic(),
                OperatorKind.Bitwise => ComputeTypeForNumericOperation(),
                OperatorKind.Logical => ComputeTypeForLogical(),
                OperatorKind.Shift => ComputeTypeForShift(),
                OperatorKind.Comparison => ComputeTypeForComparison(),
                OperatorKind.Assignment => ComputeTypeForAssignment(),
                OperatorKind.ArithmeticAssignment => ComputeTypeForAssignment(),
                OperatorKind.BitwiseAssignment => ComputeTypeForAssignment(),
                OperatorKind.ShiftAssignment => ComputeTypeForShiftAssignment(),
                OperatorKind.MemberAccess => ComputeTypeForMemberAccess(),
                OperatorKind.IncrementDecrement => throw new InvalidOperationException("Unrecognized operator"),
                OperatorKind.PointerOperation => throw new InvalidOperationException("Unrecognized operator"),
                OperatorKind.Conditional => throw new InvalidOperationException("Unrecognized operator"),
                _ => throw new InvalidOperationException("Unrecognized operator")
            };
        }

        private UsageTypeInfo ComputeTypeForArithmetic()
        {
            if ((Operator == "+" || Operator == "-")
                && ((Left.ComputedType.PointerLevel > 0 && ExpressionHelpers.ImplicitlyConvertibleToInt(Right.ComputedType))
                    || (Right.ComputedType.PointerLevel > 0 && ExpressionHelpers.ImplicitlyConvertibleToInt(Left.ComputedType))))
            {
                return ComputeTypeForPointerArithmetic();
            }

            return ComputeTypeForNumericOperation();
        }

        private UsageTypeInfo ComputeTypeForNumericOperation()
        {
            var commonType = ExpressionHelpers.GetCommonType(Left.ComputedType, Right.ComputedType);

            ThrowIfNotNumeric(commonType);

            ComputedType = commonType;
            return ComputedType;
        }

        private UsageTypeInfo ComputeTypeForLogical()
        {
            var commonType = ExpressionHelpers.GetCommonType(Left.ComputedType, Right.ComputedType);

            ThrowIfNotNumeric(commonType);

            ComputedType = intType;
            return ComputedType;
        }

        private UsageTypeInfo ComputeTypeForShift()
        {
            ThrowIfNotNumeric(Left.ComputedType);

            if (!ExpressionHelpers.ImplicitlyConvertibleToInt(Right.ComputedType))
            {
                throw new InvalidOperationException("Cannot shift by operand of this type");
            }

            ComputedType = Left.ComputedType;
            return ComputedType;
        }

        private UsageTypeInfo ComputeTypeForComparison()
        {
            if (ExpressionHelpers.IsNumeric(Left.ComputedType) && ExpressionHelpers.IsNumeric(Right.ComputedType))
            {
                ExpressionHelpers.GetCommonType(Left.ComputedType, Right.ComputedType);
                ComputedType = intType;
                return ComputedType;
            }
            else if (Left.ComputedType.PointerLevel > 0 && Right.ComputedType.PointerLevel > 0)
            {
                ComputedType = intType;
                return ComputedType;
            }
            else
            {
                throw new InvalidOperationException("Cannot compare operands of these types");
            }
        }

        private UsageTypeInfo ComputeTypeForAssignment()
        {
            if (!Left.IsAssignable)
            {
                throw new InvalidOperationException("Cannot assign to this value");
            }

            ExpressionHelpers.GetCommonType(Left.ComputedType, Right.ComputedType);

            ComputedType = Left.ComputedType;
            return Left.ComputedType;
        }

        private UsageTypeInfo ComputeTypeForShiftAssignment()
        {
            if (!Left.IsAssignable) { throw new InvalidOperationException("Cannot assign to this value"); }
            else if (!ExpressionHelpers.ImplicitlyConvertibleToInt(Right.ComputedType))
            {
                throw new InvalidOperationException("Right-hand side must be int");
            }

            ComputedType = Left.ComputedType;

            return Left.ComputedType;
        }

        private UsageTypeInfo ComputeTypeForMemberAccess()
        {
            var isDirectMemberAccess = Operator == ".";
            if (!(Right is Identifier rightIdentifier)) { throw new InvalidOperationException("Cannot access non-identifier member of struct"); }
            else if (!Left.IsAssignable) { throw new InvalidOperationException("Cannot access members of value expression"); }
            else if (!isDirectMemberAccess)
            {
                if (Left.ComputedType.PointerLevel < 1)
                {
                    throw new InvalidOperationException("Left-hand side of pointer member access was not a pointer");
                }
            }

            if (!(Left.ComputedType.DeclaredType is StructInfo leftStruct))
            {
                throw new InvalidOperationException("Cannot access members of non-struct");
            }

            var matchingMember = leftStruct.MemberInfos.FirstOrDefault(m => m.Name == rightIdentifier.Name);

            if (matchingMember == null) { throw new InvalidOperationException("Struct doesn't have this member"); }

            ComputedType = new UsageTypeInfo
            {
                DeclaredType = matchingMember.UnderlyingType,
                PointerLevel = (matchingMember.ArraySize == 1)
                    ? matchingMember.PointerLevel
                    : matchingMember.PointerLevel + 1
            };
            rightIdentifier.ReferentStructMember = matchingMember;
            rightIdentifier.ReferentKind = IdentifierReferentKind.StructMember;

            return ComputedType;
        }

        private static void ThrowIfNotNumeric(UsageTypeInfo commonType)
        {
            if (!ExpressionHelpers.IsNumeric(commonType))
            {
                throw new InvalidOperationException("Bitwise expression operands aren't numbers");
            }
        }

        private UsageTypeInfo ComputeTypeForPointerArithmetic()
        {
            var pointerOperand = (Left.ComputedType.PointerLevel > 0) ? Left : Right;

            ComputedType = new UsageTypeInfo
            {
                DeclaredType = pointerOperand.ComputedType.DeclaredType,
                PointerLevel = pointerOperand.ComputedType.PointerLevel - 1
            };
            IsAssignable = true;

            return ComputedType;
        }
    }
}