using System;
using System.Collections.Generic;
using System.Linq;
using NLog;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions
{
    internal sealed class FunctionInvocation : TypedExpression
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public TypedExpression Operand { get; set; }
        public List<TypedExpression> Arguments { get; set; }

        public override UsageTypeInfo ComputeType(EmitContext context, TypedExpression parent)
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

            var functionReturnType = (operandType.DeclaredType as FuncptrTypeInfo).ReturnType;
            ComputedType = functionReturnType;
            
            logger.Trace($"Function invocation {OriginalCode} has type {ComputedType}");
            
            return ComputedType;
        }

        public override StartEndVertices Generate(EmitContext context, TypedExpression parent)
        {
            // Push arguments
            logger.Trace($"Generating code for function invocation {OriginalCode}");
            
            var argumentFlows = Arguments.Select(a => a.Generate(context, this)).ToList();

            if (argumentFlows.Any())
            {
                StartEndVertices lastArgumentFlow = null;

                foreach (var argumentFlow in argumentFlows)
                {
                    lastArgumentFlow?.ConnectTo(argumentFlow, FlowEdgeType.DirectFlow);
                    lastArgumentFlow = argumentFlow;
                }
            }

            // Evaluate operand, which puts call address on the stack, after the arguments
            var operandFlow = Operand.Generate(context, this);
            argumentFlows.LastOrDefault()?.ConnectTo(operandFlow, FlowEdgeType.DirectFlow);
            
            // Pop the call address into EAX
            var popCallAddress = new InstructionVertex("pop", OperandSize.Qword, EmitHelpers.Register(Register.EAX));
            operandFlow.ConnectTo(popCallAddress, FlowEdgeType.DirectFlow);

            context.CurrentStack.Pop();

            // Call EAX
            var callInstruction = new InstructionVertex("call", OperandSize.NotUsed, EmitHelpers.Register(Register.EAX));
            popCallAddress.ConnectTo(callInstruction, FlowEdgeType.DirectFlow);

            return new StartEndVertices(
                argumentFlows.Any() ? argumentFlows.First().ConnectionTarget : operandFlow.ConnectionTarget,
                callInstruction);
        }
    }
}