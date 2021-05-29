using System;
using System.Collections.Generic;
using System.Linq;
using NLog;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions
{
    internal sealed class TernaryExpression : TypedExpression
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public TypedExpression Operand1 { get; set; }
        public string Operator1 { get; set; }
        public TypedExpression Operand2 { get; set; }
        public string Operator2 { get; set; }
        public TypedExpression Operand3 { get; set; }

        public override UsageTypeInfo ComputeType(EmitContext context, TypedExpression parent)
        {
            var operand1Type = Operand1.ComputeType(context, this);
            var operand2Type = Operand2.ComputeType(context, this);
            var operand3Type = Operand3.ComputeType(context, this);

            if (!ExpressionHelpers.ImplicitlyConvertibleToInt(operand1Type))
            {
                throw new InvalidOperationException("Ternary conditional operator doesn't start with condition");
            }

            ComputedType = ExpressionHelpers.GetCommonType(operand2Type, operand3Type);
            logger.Trace($"Ternary expression {OriginalCode} has type {ComputedType}");
            
            return ComputedType;
        }

        public override StartEndVertices Generate(EmitContext context, TypedExpression parent)
        {
            logger.Trace($"Generating code for ternary expression {OriginalCode}");
            
            var conditionFlow = Operand1.Generate(context, this);
            var conditionOperandSize = EmitHelpers.ToOperandSize(Operand1.ComputedType.Size);

            var conditionEvaluation = EmitHelpers.ConnectWithDirectFlow(new IConnectable[]
            {
                EmitHelpers.ZeroEAX(),
                new InstructionVertex("pop", conditionOperandSize, EmitHelpers.Register(Register.EAX)),
                new InstructionVertex("push", OperandSize.Dword, EmitHelpers.Register(Register.EAX)),
                new InstructionVertex("push", OperandSize.Dword, new IntegerOperand(0)),
                new InstructionVertex("cmp", OperandSize.Dword),
            });

            var trueBranchFlow = Operand2.Generate(context, this);
            var falseBranchFlow = Operand3.Generate(context, this);

            trueBranchFlow.Start.IsJumpTarget = true;
            falseBranchFlow.Start.IsJumpTarget = true;

            var comparisonInstruction = conditionEvaluation.End;
            comparisonInstruction.ConnectTo(trueBranchFlow, FlowEdgeType.JumpIfEqual);
            comparisonInstruction.ConnectTo(falseBranchFlow, FlowEdgeType.JumpIfNotEqual);

            var afterNop = new InstructionVertex("nop");
            trueBranchFlow.ConnectTo(afterNop, FlowEdgeType.UnconditionalJump);
            falseBranchFlow.ConnectTo(afterNop, FlowEdgeType.DirectFlow);

            conditionFlow.ConnectTo(conditionEvaluation, FlowEdgeType.DirectFlow);

            return new StartEndVertices(conditionFlow.Start, afterNop);
        }
    }
}