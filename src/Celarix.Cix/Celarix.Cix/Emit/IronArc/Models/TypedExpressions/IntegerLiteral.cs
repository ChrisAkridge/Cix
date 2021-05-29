using System;
using System.Collections.Generic;
using System.Linq;
using NLog;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions
{
    internal sealed class IntegerLiteral : Literal
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public ulong ValueBits { get; set; }
        public NumericLiteralType LiteralType { get; set; }

        public override UsageTypeInfo ComputeType(EmitContext context, TypedExpression parent)
        {
            ComputedType = LiteralType switch
            {
                NumericLiteralType.Integer => new UsageTypeInfo
                {
                    DeclaredType = new NamedTypeInfo
                    {
                        Name = "int", Size = 4
                    }
                },
                NumericLiteralType.UnsignedInteger => new UsageTypeInfo
                {
                    DeclaredType = new NamedTypeInfo
                    {
                        Name = "uint", Size = 4
                    }
                },
                NumericLiteralType.Long => new UsageTypeInfo
                {
                    DeclaredType = new NamedTypeInfo
                    {
                        Name = "long", Size = 8
                    }
                },
                NumericLiteralType.UnsignedLong => new UsageTypeInfo
                {
                    DeclaredType = new NamedTypeInfo
                    {
                        Name = "ulong", Size = 8
                    }
                },
                _ => throw new InvalidOperationException("Internal compiler error: wrong literal type assigned")
            };
            
            logger.Trace($"Integer literal {OriginalCode} has type {ComputedType}");

            return ComputedType;
        }

        public override StartEndVertices Generate(EmitContext context, TypedExpression parent)
        {
            logger.Trace($"Generating code for integer literal {OriginalCode}");
            
            var operandSize = EmitHelpers.ToOperandSize(ComputedType.Size);
            var pushInstruction = new InstructionVertex("push", operandSize, new IntegerOperand(ValueBits));
            context.CurrentStack.Push(new VirtualStackEntry("<integerLiteral>", ComputedType));

            return StartEndVertices.MakePair(pushInstruction);
        }
    }
}