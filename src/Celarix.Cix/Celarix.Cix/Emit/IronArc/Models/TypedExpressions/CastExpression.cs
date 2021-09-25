using System;
using System.Collections.Generic;
using System.Linq;
using NLog;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions
{
    internal sealed class CastExpression : TypedExpression
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public UsageTypeInfo ToType { get; set; }
        public TypedExpression Expression { get; set; }

        public override UsageTypeInfo ComputeType(EmitContext context, TypedExpression parent)
        {
            var expressionType = Expression.ComputeType(context, this);

            if (ExpressionHelpers.IsStruct(expressionType, context.DeclaredTypes)
                || ExpressionHelpers.IsStruct(ToType, context.DeclaredTypes))
            {
                throw new InvalidOperationException("Structs are not convertible");
            }
            else if (expressionType.DeclaredType is FuncptrTypeInfo && ToType.DeclaredType is FuncptrTypeInfo)
            {
                throw new InvalidOperationException("Function pointer types can't be directly converted into each other");
            }
            
            ComputedType = ToType;
            
            logger.Trace($"Cast expression {OriginalCode} has type {ComputedType}");
            
            return ToType;
        }

        public override StartEndVertices Generate(EmitContext context, TypedExpression parent)
        {
            logger.Trace($"Generating code for {OriginalCode}");

            var expressionFlow = Expression.Generate(context, this);

            var oldEntry = context.CurrentStack.Pop();
            context.CurrentStack.Push(new VirtualStackEntry(oldEntry.Name, ComputedType));

            if (Expression.ComputedType.Size == ToType.Size) { return expressionFlow; }

            var castFlow = EmitHelpers.ConnectWithDirectFlow(new IConnectable[]
            {
                EmitHelpers.ChangeWidthOfTopOfStack(context,
                    EmitHelpers.ToOperandSize(Expression.ComputedType.Size),
                    EmitHelpers.ToOperandSize(ToType.Size))
            });
                
            expressionFlow.ConnectTo(castFlow, FlowEdgeType.DirectFlow);
            return new StartEndVertices(expressionFlow.Start, castFlow.End);
        }
    }
}