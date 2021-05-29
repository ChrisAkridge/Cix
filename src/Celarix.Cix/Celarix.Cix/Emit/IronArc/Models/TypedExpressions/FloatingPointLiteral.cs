using System;
using System.Collections.Generic;
using System.Linq;
using NLog;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions
{
    internal sealed class FloatingPointLiteral : Literal
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

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
            
            logger.Trace($"Floating point literal {OriginalCode} has type {ComputedType}");
            
            return ComputedType;
        }

        public override StartEndVertices Generate(EmitContext context, TypedExpression parent)
        {
            logger.Trace($"Generating code for {OriginalCode}");
            
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