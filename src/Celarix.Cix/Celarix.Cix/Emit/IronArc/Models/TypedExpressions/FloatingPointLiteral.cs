using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions
{
    internal sealed class FloatingPointLiteral : Literal
    {
        public ulong ValueBits { get; set; }
        public NumericLiteralType NumericLiteralType { get; set; }

        public override UsageTypeInfo ComputeType(EmitContext context, TypedExpression parent)
        {
            ComputedType = (NumericLiteralType == NumericLiteralType.Single)
                ? new UsageTypeInfo
                {
                    DeclaredType = new NamedTypeInfo
                    {
                        Name = "float", Size = 4
                    }
                }
                : new UsageTypeInfo
                {
                    DeclaredType = new NamedTypeInfo
                    {
                        Name = "double", Size = 4
                    }
                };
            return ComputedType;
        }

        public override StartEndVertices Generate(EmitContext context, TypedExpression parent)
        {
            context.CurrentStack.Push(new VirtualStackEntry("<floatingPointLiteral>", ComputedType));
            
            return EmitHelpers.ConnectWithDirectFlow(new IConnectable[]
            {
                new InstructionVertex("push",
                    (NumericLiteralType == NumericLiteralType.Single) ? OperandSize.Dword : OperandSize.Qword,
                    new IntegerOperand(ValueBits))
            });
        }
    }
}