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

            // <generate left>  [left]
            var operandFlows = new List<IConnectable>
            {
                Left.Generate(context, this)
            };

            if (!Left.ComputedType.Equals(ComputedType))
            {
                operandFlows.Add(EmitHelpers.ChangeWidthOfTopOfStack(EmitHelpers.ToOperandSize(Left.ComputedType.Size),
                    computedTypeOperandSize));
            }
            
            // <generate right> [left right]
            operandFlows.Add(Right.Generate(context, this));

            if (!Right.ComputedType.Equals(ComputedType) && !ConvertRightHandSideToResultType())
            {
                operandFlows.Add(EmitHelpers.ChangeWidthOfTopOfStack(rightOperandSize,
                    computedTypeOperandSize));
            }

            IConnectable[] computationFlow;
            VirtualStackEntry resultEntry;
            switch (Operator)
            {
                case "+":
                case "-":
                {
                    if (Left.ComputedType.PointerLevel > 0)
                    {
                        computationFlow = GeneratePointerArithmetic(rightOperandSize, rightSize);
                        resultEntry = GetPointerStackEntry();
                    }
                    else
                    {
                        computationFlow = GenerateAddSubtract(computedTypeIsFloatingPoint, computedTypeOperandSize);
                        resultEntry = GetValueStackEntry();
                    }

                    break;
                }
                case "*":
                    computationFlow = GenerateMultiply(computedTypeIsFloatingPoint, computedTypeOperandSize);
                    resultEntry = GetValueStackEntry();
                    break;
                case "/":
                    computationFlow = GenerateDivide(computedTypeIsFloatingPoint, computedTypeOperandSize);
                    resultEntry = GetValueStackEntry();
                    break;
                case "%":
                    computationFlow = GenerateModulusDivide(computedTypeIsFloatingPoint, computedTypeOperandSize);
                    resultEntry = GetValueStackEntry();
                    break;
                case "&":
                    computationFlow = GenerateBitwiseAND(computedTypeOperandSize);
                    resultEntry = GetValueStackEntry();
                    break;
                case "|":
                    computationFlow = GenerateBitwiseOR(computedTypeOperandSize);
                    resultEntry = GetValueStackEntry();
                    break;
                case "^":
                    computationFlow = GenerateBitwiseXOR(computedTypeOperandSize);
                    resultEntry = GetValueStackEntry();
                    break;
                case "<<":
                case ">>":
                    computationFlow = GenerateShift(rightOperandSize, computedTypeOperandSize);
                    resultEntry = GetValueStackEntry();
                    break;
                case ".":
                case "->":
                    if (!EmitHelpers.ExpressionRequiresPointer(parent))
                    {
                        computationFlow = GenerateGetMemberValue();
                        resultEntry = GetPointerStackEntry();
                    }
                    else
                    {
                        computationFlow = GenerateGetMemberPointer();
                        resultEntry = GetValueStackEntry();
                    }

                    break;
                case "=":
                    computationFlow = GenerateAssignment(context);
                    resultEntry = GetValueStackEntry();

                    break;
                case "+=":
                case "-=":
                case "*=":
                case "/=":
                case "%=":
                case "&=":
                case "|=":
                case "^=":
                case "<<=":
                case ">>=":
                    computationFlow = GenerateOperationAssignment(context);
                    resultEntry = GetValueStackEntry();
                    
                    break;
                default:
                {
                    computationFlow = comparisonOperands.TryGetValue(Operator, out var operands)
                        ? GenerateComparison(computedTypeOperandSize, operands)
                        : Operator switch
                        {
                            "&&" => GenerateLogicalAND(computedTypeOperandSize),
                            "||" => GenerateLogicalOR(computedTypeOperandSize),
                            _ => throw new InvalidOperationException("Internal compiler error: unrecognized operator")
                        };

                    resultEntry = new VirtualStackEntry("<comparisonResult>", intType);
                    break;
                }
            }

            context.CurrentStack.Pop();
            context.CurrentStack.Pop();
            context.CurrentStack.Push(resultEntry);
            return EmitHelpers.ConnectWithDirectFlow(operandFlows.Concat(computationFlow));
        }

        private IConnectable[] GenerateAssignment(ExpressionEmitContext context)
        {
            var resultStackEntry = context.CurrentStack.Peek();
            var destinationPointerOffsetFromEBP = resultStackEntry.OffsetFromEBP - 8;
            
            if (ExpressionHelpers.IsStruct(ComputedType, context.DeclaredTypes))
            {
                // mov QWORD <destinationPointer> EAX                               [destinationPointer result]
                // movln <result> *EAX <sizeof(result)>                             [destinationPointer result]
                // movln <result> <destinationPointer> <sizeof(result)>             [result garbage]
                // subl QWORD ESP 8 ESP                                             [result]
                return new IConnectable[]
                {
                    new InstructionVertex("mov", OperandSize.Qword, EmitHelpers.Register(Register.EBP, isPointer: true, destinationPointerOffsetFromEBP),
                        EmitHelpers.Register(Register.EAX)),
                    new InstructionVertex("movln", OperandSize.NotUsed, EmitHelpers.Register(Register.EBP, isPointer: true, resultStackEntry.OffsetFromEBP),
                        EmitHelpers.Register(Register.EAX, isPointer: true), new IntegerOperand(ComputedType.Size)),
                    new InstructionVertex("movln", OperandSize.NotUsed, EmitHelpers.Register(Register.EBP, isPointer: true, resultStackEntry.OffsetFromEBP),
                        EmitHelpers.Register(Register.EBP, isPointer: true, destinationPointerOffsetFromEBP), new IntegerOperand(ComputedType.Size)),
                    new InstructionVertex("subl", OperandSize.Qword, EmitHelpers.Register(Register.ESP), new IntegerOperand(8),
                        EmitHelpers.Register(Register.ESP))
                };
            }
            else
            {
                // mov QWORD <destinationPointer> EAX                               [destinationPointer result]
                // mov <sizeof(result)> <result> *EAX                               [destinationPointer result]
                // mov <sizeof(result)> <result> <destinationPointer>               [result garbage]
                // subl QWORD ESP 8 ESP                                             [result]
                var computedTypeOperandSize = EmitHelpers.ToOperandSize(ComputedType.Size);
                
                return new IConnectable[]
                {
                    new InstructionVertex("mov", OperandSize.Qword,
                        EmitHelpers.Register(Register.EBP, isPointer: true, destinationPointerOffsetFromEBP),
                        EmitHelpers.Register(Register.EAX)),
                    new InstructionVertex("mov", computedTypeOperandSize,
                        EmitHelpers.Register(Register.EBP, isPointer: true, resultStackEntry.OffsetFromEBP),
                        EmitHelpers.Register(Register.EAX, isPointer: true), new IntegerOperand(ComputedType.Size)),
                    new InstructionVertex("mov", computedTypeOperandSize,
                        EmitHelpers.Register(Register.EBP, isPointer: true, resultStackEntry.OffsetFromEBP),
                        EmitHelpers.Register(Register.EBP, isPointer: true, destinationPointerOffsetFromEBP)), 
                    new InstructionVertex("subl", OperandSize.Qword, EmitHelpers.Register(Register.ESP),
                        new IntegerOperand(8),
                        EmitHelpers.Register(Register.ESP)),
                };
            }
        }

        private IConnectable[] GenerateOperationAssignment(ExpressionEmitContext context)
        {
            // mov QWORD <destinationPointer> EAX                               [destinationPointer right]
            // mov QWORD 0 EDX                                                  [destinationPointer right]
            // pop <sizeof(right)> EDX                                          [destinationPointer]
            // push <sizeof(destinationOldValue)> *EAX                          [destinationPointer destinationOldValue]
            // push <sizeof(right)> EDX                                         [destinationPointer destinationOldValue right]
            // <operation>                                                      [destinationPointer result]
            // mov <sizeof(result)> *EAX                                        [destinationPointer result]
            // mov <sizeof(result)> <result> <destinationPointer>               [result garbage]
            // subl QWORD ESP 8 ESP                                             [result]

            // Operations
            // +=: add/fadd <sizeof(result)>
            // -=: sub/fsub <sizeof(result)>
            // *=: mult/fmult <sizeof(result)>
            // /=: div/fdiv <sizeof(result)>
            // %=: mod/fmod <sizeof(result)>
            // &=: bwand <sizeof(result)>
            // |=: bwor <sizeof(result)>
            // ^=: bwxor <sizeof(result)>
            // <<=: lshift <sizeof(result)>
            // >>=: rshift <sizeof(result)>

            var rightStackEntry = context.CurrentStack.Peek();
            var leftOperandSize = EmitHelpers.ToOperandSize(Left.ComputedType.DeclaredType.Size);
            var rightOperandSize = EmitHelpers.ToOperandSize(rightStackEntry.UsageType.Size);
            var computedTypeOperandSize = EmitHelpers.ToOperandSize(ComputedType.Size);
            var computedTypeIsFloatingPoint = ComputedType.DeclaredType is NamedTypeInfo namedType
                && (namedType.Name == "float" || namedType.Name == "double");

            var setupValues = new IConnectable[]
            {
                new InstructionVertex("mov", OperandSize.Qword, EmitHelpers.Register(Register.EBP, isPointer: true, rightStackEntry.OffsetFromEBP - 8)),
                new InstructionVertex("mov", OperandSize.Qword, new IntegerOperand(0), EmitHelpers.Register(Register.EDX)),
                new InstructionVertex("pop", rightOperandSize, EmitHelpers.Register(Register.EDX)),
                new InstructionVertex("push", leftOperandSize, EmitHelpers.Register(Register.EAX, isPointer: true)),
                new InstructionVertex("push", rightOperandSize, EmitHelpers.Register(Register.EDX))
            };

            if (Left.ComputedType.PointerLevel > 0 && (Operator == "+=" || Operator == "-="))
            {
                // multl EDX <sizeof(left)> EDX [destinationPointer destinationOldValue right]
                setupValues = setupValues.Concat(new IConnectable[]
                    {
                        new InstructionVertex("multl", OperandSize.Qword, EmitHelpers.Register(Register.EDX),
                            new IntegerOperand(Left.ComputedType.Size), EmitHelpers.Register(Register.EDX)),
                    })
                    .ToArray();
            }

            var operationMnenonic = !computedTypeIsFloatingPoint
                ? Operator switch
                {
                    "+=" => "add",
                    "-=" => "sub",
                    "*=" => "mult",
                    "/=" => "div",
                    "%=" => "mod",
                    "&=" => "bwand",
                    "|=" => "bwor",
                    "^=" => "bwxor",
                    "<<=" => "lshift",
                    ">>=" => "rshift",
                    _ => throw new InvalidOperationException(
                        "Internal compiler error: unrecognized assignment operator")
                }
                : Operator switch
                {
                    "+=" => "fadd",
                    "-=" => "fsub",
                    "*=" => "fmult",
                    "/=" => "fdiv",
                    "%=" => "fmod",
                    _ => throw new InvalidOperationException(
                        "Operator cannot be used on floating point values")
                };

            return setupValues.Concat(new IConnectable[]
                {
                    new InstructionVertex(operationMnenonic, computedTypeOperandSize),
                    new InstructionVertex("mov", computedTypeOperandSize, EmitHelpers.Register(Register.EAX, isPointer: true)),
                    new InstructionVertex("mov", computedTypeOperandSize,
                        EmitHelpers.Register(Register.EBP, isPointer: true, rightStackEntry.OffsetFromEBP),
                        EmitHelpers.Register(Register.EBP, isPointer: true, rightStackEntry.OffsetFromEBP - 8)),
                    new InstructionVertex("subl", OperandSize.Qword, EmitHelpers.Register(Register.ESP),
                        new IntegerOperand(8),
                        EmitHelpers.Register(Register.ESP)),
                })
                .ToArray();
        }

        private static IConnectable[] GenerateComparison(OperandSize computedTypeOperandSize, (ulong flagMask, ulong shiftAmount) operands)
        {
            /*
             * cmp sizeof(result)       []
             * push QWORD EFLAGS        [EFLAGS]
             * push QWORD <flagMask>    [EFLAGS flagMask]
             * bwand QWORD              [maskedEFLAGS]
             * push QWORD <shiftAmount> [maskedEFLAGS shiftAmount]
             * rshift QWORD             [comparisonResultWasNonZero]
             */

            var (flagMask, shiftAmount) = operands;

            return new IConnectable[]
            {
                new InstructionVertex("cmp", computedTypeOperandSize),
                new InstructionVertex("push", OperandSize.Qword, EmitHelpers.Register(Register.EFLAGS)),
                new InstructionVertex("push", OperandSize.Qword, new IntegerOperand(flagMask)),
                new InstructionVertex("bwand", OperandSize.Qword),
                new InstructionVertex("push", OperandSize.Qword, new IntegerOperand(shiftAmount)),
                new InstructionVertex("rshift", OperandSize.Qword),
                EmitHelpers.ChangeWidthOfTopOfStack(OperandSize.Qword, OperandSize.Dword)
            };
        }

        private static IConnectable[] GenerateLogicalOR(OperandSize computedTypeOperandSize)
        {
            return new IConnectable[]
            {
                new InstructionVertex("lor", computedTypeOperandSize),
            };
        }

        private static IConnectable[] GenerateLogicalAND(OperandSize computedTypeOperandSize)
        {
            return new IConnectable[]
            {
                new InstructionVertex("land", computedTypeOperandSize),
            };
        }

        private static IConnectable[] GenerateGetMemberPointer()
        {
            IConnectable[] computationFlow;

            computationFlow = new IConnectable[]
            {
                new InstructionVertex("add", OperandSize.Qword)
            };

            return computationFlow;
        }

        private IConnectable[] GenerateGetMemberValue()
        {
            IConnectable[] computationFlow;

            computationFlow = new IConnectable[]
            {
                new InstructionVertex("add", OperandSize.Qword), EmitHelpers.ZeroEAX(),
                new InstructionVertex("pop", OperandSize.Qword, EmitHelpers.Register(Register.EAX)), new InstructionVertex(
                    "push", EmitHelpers.ToOperandSize(ComputedType.Size),
                    EmitHelpers.Register(Register.EBP, isPointer: true)),
            };

            return computationFlow;
        }

        private IConnectable[] GenerateShift(OperandSize rightOperandSize, OperandSize computedTypeOperandSize)
        {
            IConnectable[] computationFlow;

            computationFlow = new IConnectable[]
            {
                EmitHelpers.ChangeWidthOfTopOfStack(rightOperandSize, OperandSize.Dword),
                new InstructionVertex((Operator == "<<") ? "lshift" : "rshift", computedTypeOperandSize)
            };

            return computationFlow;
        }

        private static IConnectable[] GenerateBitwiseXOR(OperandSize computedTypeOperandSize)
        {
            IConnectable[] computationFlow;

            computationFlow = new IConnectable[]
            {
                new InstructionVertex("bwxor", computedTypeOperandSize)
            };

            return computationFlow;
        }

        private static IConnectable[] GenerateBitwiseOR(OperandSize computedTypeOperandSize)
        {
            IConnectable[] computationFlow;

            computationFlow = new IConnectable[]
            {
                new InstructionVertex("bwor", computedTypeOperandSize)
            };

            return computationFlow;
        }

        private static IConnectable[] GenerateBitwiseAND(OperandSize computedTypeOperandSize)
        {
            IConnectable[] computationFlow;

            computationFlow = new IConnectable[]
            {
                new InstructionVertex("bwand", computedTypeOperandSize)
            };

            return computationFlow;
        }

        private static IConnectable[] GenerateModulusDivide(bool computedTypeIsFloatingPoint,
            OperandSize computedTypeOperandSize)
        {
            IConnectable[] computationFlow;

            computationFlow = new IConnectable[]
            {
                new InstructionVertex((computedTypeIsFloatingPoint) ? "mod" : "fmod", computedTypeOperandSize)
            };

            return computationFlow;
        }

        private static IConnectable[] GenerateDivide(bool computedTypeIsFloatingPoint, OperandSize computedTypeOperandSize)
        {
            IConnectable[] computationFlow;

            computationFlow = new IConnectable[]
            {
                new InstructionVertex((computedTypeIsFloatingPoint) ? "div" : "fdiv", computedTypeOperandSize)
            };

            return computationFlow;
        }

        private IConnectable[] GenerateMultiply(bool computedTypeIsFloatingPoint, OperandSize computedTypeOperandSize)
        {
            IConnectable[] computationFlow;

            computationFlow = new IConnectable[]
            {
                new InstructionVertex((computedTypeIsFloatingPoint) ? "fmult" : "mult", computedTypeOperandSize)
            };

            return computationFlow;
        }

        private IConnectable[] GenerateAddSubtract(bool computedTypeIsFloatingPoint, OperandSize computedTypeOperandSize)
        {
            var mnemonic = (computedTypeIsFloatingPoint)
                ? (Operator == "+")
                    ? "add"
                    : "sub"
                : (Operator == "+")
                    ? "fadd"
                    : "fsub";

            var computationFlow = new IConnectable[]
            {
                new InstructionVertex(mnemonic, computedTypeOperandSize)
            };

            return computationFlow;
        }

        private IConnectable[] GeneratePointerArithmetic(OperandSize rightOperandSize, int rightSize)
        {
            /*
             * <convert right to DWORD>         [left (int)right]
             * push DWORD sizeof(right)         [left (int)right sizeof(right)]
             * mult DWORD                       [left offsetInBytes]
             * <convert offsetInBytes to QWORD> [left (long)offsetInBytes]
             * {add/sub} QWORD                  [left{+/-}offsetInBytes]
             */
            return new IConnectable[]
            {
                EmitHelpers.ChangeWidthOfTopOfStack(rightOperandSize, OperandSize.Dword),
                new InstructionVertex("push", OperandSize.Dword, new IntegerOperand(rightSize)),
                new InstructionVertex("mult", OperandSize.Dword),
                EmitHelpers.ChangeWidthOfTopOfStack(OperandSize.Dword, OperandSize.Qword),
                new InstructionVertex((Operator == "+") ? "add" : "sub", OperandSize.Qword)
            };
        }

        private bool ConvertRightHandSideToResultType()
        {
            var leftIsPointer = Left.ComputedType.PointerLevel > 0;

            var rightHandSideConvertedToOtherType = ((Operator == "+" || Operator == "-") && leftIsPointer)
                || Operator == "<<"
                || Operator == ">>";

            return !rightHandSideConvertedToOtherType;
        }
        
        private VirtualStackEntry GetPointerStackEntry() => new VirtualStackEntry("<binaryResult>", ComputedType.WithPointerLevel(ComputedType.PointerLevel + 1));
        private VirtualStackEntry GetValueStackEntry() => new VirtualStackEntry("<binaryResult>", ComputedType);
        #endregion
    }
}