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

        private static readonly Dictionary<string, (ulong flagMask, ulong shiftAmount)> comparisonOperands =
            new Dictionary<string, (ulong flagMask, ulong shiftAmount)>
            {
                {
                    "==", (0x4000000000000000UL, 62UL)
                },
                {
                    "!=", (0x2000000000000000UL, 61UL)
                },
                {
                    "<", (0x1000000000000000UL, 60UL)
                },
                {
                    ">", (0x0800000000000000UL, 59UL)
                },
                {
                    "<=", (0x0400000000000000UL, 58UL)
                },
                {
                    ">=", (0x0200000000000000UL, 57UL)
                },
            };

        public TypedExpression Left { get; set; }
        public TypedExpression Right { get; set; }
        public string Operator { get; set; }

        #region Type Computation
        public override UsageTypeInfo ComputeType(ExpressionEmitContext context, TypedExpression parent)
        {
            Left.ComputeType(context, this);
            Right.ComputeType(context, this);

            IsAssignable = Operator == "." || Operator == "->";

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
        #endregion

        #region Instruction Emit
        public override StartEndVertices Generate(ExpressionEmitContext context, TypedExpression parent)
        {
            var computedTypeIsFloatingPoint = ComputedType.DeclaredType is NamedTypeInfo namedType
                && (namedType.Name == "float" || namedType.Name == "double");
            var computedTypeOperandSize = EmitHelpers.ToOperandSize(ComputedType.Size);
            var rightSize = Right.ComputedType.Size;
            var rightOperandSize = EmitHelpers.ToOperandSize(rightSize);

            var operandFlows = new List<IConnectable>
            {
                Left.Generate(context, this)
            };

            if (!Left.ComputedType.Equals(ComputedType))
            {
                operandFlows.Add(EmitHelpers.ChangeWidthOfTopOfStack(EmitHelpers.ToOperandSize(Left.ComputedType.Size),
                    computedTypeOperandSize));
            }
            
            operandFlows.Add(Right.Generate(context, this));

            if (!Right.ComputedType.Equals(ComputedType) && !ConvertRightHandSideToResultType())
            {
                operandFlows.Add(EmitHelpers.ChangeWidthOfTopOfStack(rightOperandSize,
                    computedTypeOperandSize));
            }

            IConnectable[] computationFlow;
            switch (Operator)
            {
                case "+":
                case "-":
                {
                    if (Left.ComputedType.PointerLevel > 0)
                    {
                        computationFlow = new IConnectable[]
                        {
                            EmitHelpers.ChangeWidthOfTopOfStack(rightOperandSize, OperandSize.Dword),
                            new InstructionVertex("push", OperandSize.Dword, new IntegerOperand(rightSize)),
                            new InstructionVertex("mult", OperandSize.Dword),
                            EmitHelpers.ChangeWidthOfTopOfStack(OperandSize.Dword, OperandSize.Qword),
                            new InstructionVertex((Operator == "+") ? "add" : "sub", OperandSize.Qword)
                        };
                    }
                    else
                    {
                        var mnemonic = (computedTypeIsFloatingPoint)
                            ? (Operator == "+")
                                ? "add"
                                : "sub"
                            : (Operator == "+")
                                ? "fadd"
                                : "fsub";

                        computationFlow = new IConnectable[]
                        {
                            new InstructionVertex(mnemonic, computedTypeOperandSize)
                        };
                    }

                    break;
                }
                case "*":
                    computationFlow = new IConnectable[]
                    {
                        new InstructionVertex((computedTypeIsFloatingPoint) ? "fmult" : "mult", computedTypeOperandSize)
                    };

                    break;
                case "/":
                    computationFlow = new IConnectable[]
                    {
                        new InstructionVertex((computedTypeIsFloatingPoint) ? "div" : "fdiv", computedTypeOperandSize)
                    };

                    break;
                case "%":
                    computationFlow = new IConnectable[]
                    {
                        new InstructionVertex((computedTypeIsFloatingPoint) ? "mod" : "fmod", computedTypeOperandSize)
                    };

                    break;
                case "&":
                    computationFlow = new IConnectable[]
                    {
                        new InstructionVertex("bwand", computedTypeOperandSize)
                    };

                    break;
                case "|":
                    computationFlow = new IConnectable[]
                    {
                        new InstructionVertex("bwor", computedTypeOperandSize)
                    };

                    break;
                case "^":
                    computationFlow = new IConnectable[]
                    {
                        new InstructionVertex("bwxor", computedTypeOperandSize)
                    };

                    break;
                case "<<":
                case ">>":
                    computationFlow = new IConnectable[]
                    {
                        EmitHelpers.ChangeWidthOfTopOfStack(rightOperandSize, OperandSize.Dword),
                        new InstructionVertex((Operator == "<<") ? "lshift" : "rshift", computedTypeOperandSize)
                    };

                    break;
                case ".":
                case "->":
                    computationFlow = !EmitHelpers.ExpressionPerformsAssignment(parent):
                        ? new IConnectable[]
                        {
                            new InstructionVertex("add", OperandSize.Qword), EmitHelpers.ZeroEAX(),
                            new InstructionVertex("pop", OperandSize.Qword, EmitHelpers.Register(Register.EAX)),
                            new InstructionVertex("push", EmitHelpers.ToOperandSize(ComputedType.Size),
                                EmitHelpers.Register(Register.EBP, isPointer: true)),
                        }
                        : new IConnectable[]
                        {
                            new InstructionVertex("add", OperandSize.Qword)
                        };

                    break;
                default:
                {
                    computationFlow = comparisonOperands.TryGetValue(Operator, out var operands)
                        ? new IConnectable[]
                        {
                            new InstructionVertex("cmp", computedTypeOperandSize),
                            new InstructionVertex("push", OperandSize.Qword, EmitHelpers.Register(Register.EFLAGS)),
                            new InstructionVertex("push", OperandSize.Qword, new IntegerOperand(operands.flagMask)),
                            new InstructionVertex("bwand", OperandSize.Qword),
                            new InstructionVertex("push", OperandSize.Qword, new IntegerOperand(operands.shiftAmount)),
                            new InstructionVertex("rshift", OperandSize.Qword),
                            EmitHelpers.ChangeWidthOfTopOfStack(OperandSize.Qword, OperandSize.Dword)
                        }
                        : Operator switch
                        {
                            "&&" => new IConnectable[]
                            {
                                new InstructionVertex("land", computedTypeOperandSize),
                            },
                            "||" => new IConnectable[]
                            {
                                new InstructionVertex("lor", computedTypeOperandSize),
                            },
                            _ => throw new InvalidOperationException("Internal compiler error: unrecognized operator")
                        };

                    break;
                }
            }

            return EmitHelpers.ConnectWithDirectFlow(operandFlows.Concat(computationFlow));
        }

        private bool ConvertRightHandSideToResultType()
        {
            var leftIsPointer = Left.ComputedType.PointerLevel > 0;

            var rightHandSideConvertedToOtherType = ((Operator == "+" || Operator == "-") && leftIsPointer)
                || Operator == "<<"
                || Operator == ">>";

            return !rightHandSideConvertedToOtherType;
        }
        #endregion
    }
}