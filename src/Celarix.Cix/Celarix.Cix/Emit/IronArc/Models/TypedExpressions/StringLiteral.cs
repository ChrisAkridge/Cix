using System;
using System.Collections.Generic;
using System.Linq;
using NLog;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions
{
    internal sealed class StringLiteral : Literal
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

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
            
            logger.Trace($"String literal {OriginalCode} has type byte*");

            return ComputedType;
        }

        public override StartEndVertices Generate(EmitContext context, TypedExpression parent)
        {
            logger.Trace($"Generating code for string literal {OriginalCode}");
            
            var pushInstruction = new InstructionVertex("push", OperandSize.NotUsed, new StringLiteralOperand
            {
                Literal = LiteralValue
            });
            
            context.CurrentStack.Push(new VirtualStackEntry("<stringLiteral>", ComputedType));

            return StartEndVertices.MakePair(pushInstruction);
        }
    }
}