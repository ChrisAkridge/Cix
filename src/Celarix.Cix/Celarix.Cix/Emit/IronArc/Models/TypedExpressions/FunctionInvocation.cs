using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions
{
    internal sealed class FunctionInvocation : TypedExpression
    {
        public TypedExpression Operand { get; set; }
        public List<TypedExpression> Arguments { get; set; }

        public override UsageTypeInfo ComputeType(ExpressionEmitContext context, TypedExpression parent)
        {
            var operandType = Operand.ComputeType(context, this);

            foreach (var argument in Arguments)
            {
                argument.ComputeType(context, this);
            }

            if (!(operandType.DeclaredType is FuncptrTypeInfo))
            {
                throw new InvalidOperationException("Cannot invoke non-function");
            }

            ComputedType = operandType;
            return ComputedType;
        }

        public override StartEndVertices Generate(ExpressionEmitContext context, TypedExpression parent)
        {
            var operandFlow = Operand.Generate(context, this);
            var operandStackEntry = new VirtualStackEntry("<functionTarget>", Operand.ComputedType);
            context.CurrentStack.Push(operandStackEntry);
            
            var argumentFlows = Arguments.Select(a => a.Generate(context, this)).ToList();
            var callInstruction = new InstructionVertex("call", OperandSize.NotUsed, EmitHelpers.Register(Register.EBP, isPointer: true, operandStackEntry.OffsetFromEBP));

            if (argumentFlows.Any())
            {
                var stackArgsInstruction = new InstructionVertex("stackargs");
                operandFlow.ConnectTo(stackArgsInstruction, FlowEdgeType.DirectFlow);
                stackArgsInstruction.ConnectTo(argumentFlows.First(), FlowEdgeType.DirectFlow);
                argumentFlows.Last().ConnectTo(callInstruction, FlowEdgeType.DirectFlow);
            }
            else
            {
                operandFlow.ConnectTo(callInstruction, FlowEdgeType.DirectFlow);
            }

            context.CurrentStack.Pop();

            return new StartEndVertices
            {
                Start = operandFlow.ConnectionTarget, End = callInstruction
            };
        }
    }
}