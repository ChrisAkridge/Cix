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

        #region Type Computation

        public override UsageTypeInfo ComputeType(EmitContext context, TypedExpression parent)
        {
            Operand.ComputeType(context, this);

            IsAssignable = Operator == "&";

            ComputedType = ExpressionHelpers.GetOperatorKind(Operator,
                    (IsPostfix) ? OperationKind.PostfixUnary : OperationKind.PrefixUnary) switch
                {
                    OperatorKind.Sign => ComputeTypeForSign(),
                    OperatorKind.IncrementDecrement => ComputeTypeForIncrementDecrement(),
                    OperatorKind.PointerOperation => ComputeTypeForPointerOperation(),
                    OperatorKind.SizeOf => ComputeTypeForSizeOf(),
                    _ => throw new InvalidOperationException("Internal compiler error: unexpected unary operator")
                };

            return ComputedType;
        }

        private UsageTypeInfo ComputeTypeForSizeOf()
        {
            return ComputedType = new UsageTypeInfo
            {
                DeclaredType = new NamedTypeInfo
                {
                    Name = "int", Size = 4
                }
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
                "&" => new UsageTypeInfo
                {
                    DeclaredType = Operand.ComputedType.DeclaredType,
                    PointerLevel = Operand.ComputedType.PointerLevel + 1
                },
                _ => throw new InvalidOperationException("Internal compiler error: unrecognized pointer operator")
            };

            return ComputedType;
        }

        #endregion

        #region Code Generation

        public override StartEndVertices Generate(EmitContext context, TypedExpression parent)
        {
            if (Operator == "sizeof")
            {
                var pushInstruction =
                    new InstructionVertex("push", OperandSize.Dword, new IntegerOperand(Operand.ComputedType.Size));
                context.CurrentStack.Push(new VirtualStackEntry("<sizeofType>", ComputedType));

                return new StartEndVertices
                {
                    Start = pushInstruction, End = pushInstruction
                };
            }
            
            var operandSize = EmitHelpers.ToOperandSize(Operand.ComputedType.Size);
            
            if (!IsPostfix)
            {
                // WYLO: Forgot to work the virtual stack here.
                switch (Operator)
                {
                    case "+":
                    case "&":
                        return Operand.Generate(context, this);
                    case "-":
                        return EmitHelpers.ConnectWithDirectFlow(new IConnectable[]
                        {
                            Operand.Generate(context, this),
                            EmitHelpers.ZeroEAX(),
                            new InstructionVertex("pop", operandSize, EmitHelpers.Register(Register.EAX)),
                            new InstructionVertex("push", operandSize, new IntegerOperand(0)),
                            new InstructionVertex("push", operandSize, EmitHelpers.Register(Register.EAX)),
                            new InstructionVertex("sub", operandSize),
                        });
                    case "~":
                        return EmitHelpers.ConnectWithDirectFlow(new IConnectable[]
                        {
                            Operand.Generate(context, this),
                            new InstructionVertex("bwnot", operandSize),
                        });
                    case "!":
                        return EmitHelpers.ConnectWithDirectFlow(new IConnectable[]
                        {
                            Operand.Generate(context, this),
                            new InstructionVertex("lnot", operandSize),
                        });
                    case "*":
                    {
                        context.CurrentStack.Pop();
                        context.CurrentStack.Push(new VirtualStackEntry("<pointerDereference>", new UsageTypeInfo
                        {
                            DeclaredType = Operand.ComputedType.DeclaredType,
                            PointerLevel = Operand.ComputedType.PointerLevel - 1
                        }));
                    
                        if (Operand.ComputedType.PointerLevel >= 2 || EmitHelpers.IsIronArcOperandSize(Operand.ComputedType.DeclaredType.Size))
                        {
                            var declaredTypeOperandSize = EmitHelpers.ToOperandSize(Operand.ComputedType.DeclaredType.Size);
                        
                            return EmitHelpers.ConnectWithDirectFlow(new IConnectable[]
                            {
                                new InstructionVertex("pop", OperandSize.Qword, EmitHelpers.Register(Register.EAX)),
                                new InstructionVertex("push", declaredTypeOperandSize, EmitHelpers.Register(Register.EAX, isPointer: true)),
                            });
                        }
                        else
                        {
                            return EmitHelpers.ConnectWithDirectFlow(new IConnectable[]
                            {
                                new InstructionVertex("pop", OperandSize.Qword, EmitHelpers.Register(Register.EAX)),
                                new InstructionVertex("movln", OperandSize.NotUsed,
                                    EmitHelpers.Register(Register.EAX, isPointer: true),
                                    EmitHelpers.Register(Register.ESP, isPointer: true),
                                    new IntegerOperand(Operand.ComputedType.Size)),
                                new InstructionVertex("addl", OperandSize.Qword, EmitHelpers.Register(Register.ESP),
                                    new IntegerOperand(Operand.ComputedType.Size),
                                    EmitHelpers.Register(Register.ESP))
                            });
                        }
                    }
                    case "++":
                    case "--":
                    {
                        var pointerStackEntry = context.CurrentStack.Pop();
                        context.CurrentStack.Push(new VirtualStackEntry("<preIncrementDecrementResult>", ComputedType));
                        var floatSize = (ComputedType.Size == 4) ? FloatSize.Single : FloatSize.Double;

                        return !EmitHelpers.IsFloatingPointType(ComputedType.DeclaredType)
                            ? EmitHelpers.ConnectWithDirectFlow(new IConnectable[]
                            {
                                new InstructionVertex("pop", OperandSize.Qword, EmitHelpers.Register(Register.EAX)),
                                new InstructionVertex((Operator == "++") ? "incl" : "decl", operandSize,
                                    EmitHelpers.Register(Register.EAX, isPointer: true)),
                                new InstructionVertex("push", operandSize,
                                    EmitHelpers.Register(Register.EAX, isPointer: true))
                            })
                            : EmitHelpers.ConnectWithDirectFlow(new IConnectable[]
                            {
                                new InstructionVertex("push", operandSize,
                                    EmitHelpers.Register(Register.EBP, isPointer: true,
                                        pointerStackEntry.OffsetFromEBP)),
                                new InstructionVertex("push", operandSize,
                                    (floatSize == FloatSize.Single)
                                        ? FloatingPointOperand.FromSingle(1f)
                                        : FloatingPointOperand.FromDouble(1d)),
                                new InstructionVertex((Operator == "++") ? "fadd" : "fsub", operandSize),
                                new InstructionVertex("pop", operandSize,
                                    EmitHelpers.Register(Register.EBP, isPointer: true,
                                        pointerStackEntry.OffsetFromEBP)),
                                new InstructionVertex("pop", OperandSize.Qword, EmitHelpers.Register(Register.EAX)),
                                new InstructionVertex("push", operandSize,
                                    EmitHelpers.Register(Register.EAX, isPointer: true))
                            });
                    }
                }
            }
            else
            {
                context.CurrentStack.Pop();
                context.CurrentStack.Push(new VirtualStackEntry("<postIncrementDecrementResult>", ComputedType));
                var floatSize = (ComputedType.Size == 4) ? FloatSize.Single : FloatSize.Double;

                return !EmitHelpers.IsFloatingPointType(ComputedType.DeclaredType)
                    ? EmitHelpers.ConnectWithDirectFlow(new IConnectable[]
                    {
                        new InstructionVertex("pop", OperandSize.Qword, EmitHelpers.Register(Register.EAX)),
                        new InstructionVertex("push", operandSize,
                            EmitHelpers.Register(Register.EAX, isPointer: true)),
                        new InstructionVertex((Operator == "++") ? "incl" : "decl", operandSize,
                            EmitHelpers.Register(Register.EAX, isPointer: true)),
                    })
                    : EmitHelpers.ConnectWithDirectFlow(new IConnectable[]
                    {
                        new InstructionVertex("pop", OperandSize.Qword, EmitHelpers.Register(Register.EAX)),
                        new InstructionVertex("push", operandSize,
                            EmitHelpers.Register(Register.EAX, isPointer: true)),
                        new InstructionVertex("push", operandSize,
                            EmitHelpers.Register(Register.EAX, isPointer: true)),
                        new InstructionVertex("push", operandSize,
                            (floatSize == FloatSize.Single)
                                ? FloatingPointOperand.FromSingle(1f)
                                : FloatingPointOperand.FromDouble(1d)),
                        new InstructionVertex((Operator == "++") ? "fadd" : "fsub", operandSize),
                        new InstructionVertex("pop", operandSize,
                            EmitHelpers.Register(Register.EAX, isPointer: true))
                    });
            }
            
            throw new InvalidOperationException("Internal compiler error: unreachable code");
        }
        #endregion
    }
}