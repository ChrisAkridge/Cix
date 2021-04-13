using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions
{
    internal sealed class StringLiteral : Literal
    {
        public string LiteralValue { get; set; }

        public override UsageTypeInfo ComputeType(EmitContext context, TypedExpression parent)
        {
            ComputedType = new UsageTypeInfo
            {
                DeclaredType = new NamedTypeInfo
                {
                    Name = "byte", Size = 1
                },
                PointerLevel = 1
            };

            return ComputedType;
        }

        public override StartEndVertices Generate(EmitContext context, TypedExpression parent)
        {
            var pushInstruction = new InstructionVertex("push", OperandSize.NotUsed, new StringLiteralOperand
            {
                Literal = LiteralValue
            });
            
            context.CurrentStack.Push(new VirtualStackEntry("<stringLiteral>", ComputedType));

            return new StartEndVertices
            {
                Start = pushInstruction, End = pushInstruction
            };
        }
    }
}