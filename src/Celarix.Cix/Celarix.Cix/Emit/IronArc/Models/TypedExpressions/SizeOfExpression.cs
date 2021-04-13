using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions
{
    internal sealed class SizeOfExpression : TypedExpression
    {
        public UsageTypeInfo Type { get; set; }

        public override UsageTypeInfo ComputeType(EmitContext context, TypedExpression parent)
        {
            ComputedType = new UsageTypeInfo
            {
                DeclaredType = new NamedTypeInfo
                {
                    Name = "int", Size = 4
                }
            };

            return ComputedType;
        }

        public override StartEndVertices Generate(EmitContext context, TypedExpression parent)
        {
            var pushInstruction = new InstructionVertex("push", OperandSize.Dword, new IntegerOperand(Type.Size));
            context.CurrentStack.Push(new VirtualStackEntry("<sizeofType>", ComputedType));

            return new StartEndVertices
            {
                Start = pushInstruction, End = pushInstruction
            };
        }
    }
}