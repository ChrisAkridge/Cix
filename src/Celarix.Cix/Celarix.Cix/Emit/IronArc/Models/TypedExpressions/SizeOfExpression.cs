using System;
using System.Collections.Generic;
using System.Linq;
using NLog;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions
{
    internal sealed class SizeOfExpression : TypedExpression
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

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

            logger.Trace($"Size-of expression {OriginalCode} has type int");
            
            return ComputedType;
        }

        public override StartEndVertices Generate(EmitContext context, TypedExpression parent)
        {
            logger.Trace($"Generating code for size-of expression {OriginalCode}");
            
            var pushInstruction = new InstructionVertex("push", OperandSize.Dword, new IntegerOperand(Type.Size));
            context.CurrentStack.Push(new VirtualStackEntry("<sizeofType>", ComputedType));

            return StartEndVertices.MakePair(pushInstruction);
        }
    }
}