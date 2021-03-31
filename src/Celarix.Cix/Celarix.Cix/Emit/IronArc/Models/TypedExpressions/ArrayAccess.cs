using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions
{
    internal sealed class ArrayAccess : TypedExpression
    {
        public TypedExpression Operand { get; set; }
        public TypedExpression Index { get; set; }

        public override UsageTypeInfo ComputeType(ExpressionEmitContext context, TypedExpression parent)
        {
            // TArray* x;
            // int y;
            //
            // TArray result = x[y];
            
            // TArray* x;
            // int y;
            // TArray z;
            //
            // x[y] = z;

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

            IsAssignable = true;
            ComputedType = new UsageTypeInfo
            {
                DeclaredType = operandType.DeclaredType,
                PointerLevel = operandType.PointerLevel - 1
            };

            return ComputedType;
        }

        public override StartEndVertices Generate(ExpressionEmitContext context, TypedExpression parent)
        {
            var getElementPointer = new List<IConnectable>
            {
                Operand.Generate(context, this),
                Index.Generate(context, this),
                EmitHelpers.ChangeWidthOfTopOfStack(EmitHelpers.ToOperandSize(Index.ComputedType.Size), OperandSize.Dword),
                new InstructionVertex("push", OperandSize.Dword, new IntegerOperand(Operand.ComputedType.DeclaredType.Size)),
                new InstructionVertex("mult", OperandSize.Dword),
                EmitHelpers.ChangeWidthOfTopOfStack(OperandSize.Dword, OperandSize.Qword),
                new InstructionVertex("add", OperandSize.Qword)
            };

            if (parent is BinaryExpression binaryExpression)
            {
                var operatorKind = ExpressionHelpers.GetOperatorKind(binaryExpression.Operator, OperationKind.Binary);

                if (operatorKind == OperatorKind.Assignment || operatorKind == OperatorKind.MemberAccess)
                {
                    return EmitHelpers.ConnectWithDirectFlow(getElementPointer);
                }
            }

            var getElementValue = new List<IConnectable>
            {
                EmitHelpers.ZeroEAX(),
                new InstructionVertex("pop", OperandSize.Qword, EmitHelpers.Register(Register.EAX)),
                new InstructionVertex("push", EmitHelpers.ToOperandSize(Operand.ComputedType.DeclaredType.Size),
                    EmitHelpers.Register(Register.EAX, isPointer: true))
            };

            return EmitHelpers.ConnectWithDirectFlow(getElementPointer.Concat(getElementPointer));
        }
    }
}