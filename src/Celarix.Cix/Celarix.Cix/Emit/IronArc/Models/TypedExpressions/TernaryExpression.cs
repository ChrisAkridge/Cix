using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions
{
    internal sealed class TernaryExpression : TypedExpression, ISecondPassConnect
    {
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
            return ComputedType;
        }

        public override StartEndVertices Generate(EmitContext context, TypedExpression parent)
        {
            var ungeneratedVertex = new UngeneratedVertex
            {
                NodeToGenerateFor = this
            };
            
            return new StartEndVertices
            {
                Start = ungeneratedVertex, End = ungeneratedVertex
            };
        }

        public void ConnectToGeneratedTree(ControlFlowVertex after, EmitContext context)
        {
            var conditionFlow = Operand1.Generate(context, this);
            var conditionOperandSize = EmitHelpers.ToOperandSize(Operand1.ComputedType.Size);

            var conditionEvaluation = new IConnectable[]
            {
                EmitHelpers.ZeroEAX(),
                new InstructionVertex("pop", conditionOperandSize, EmitHelpers.Register(Register.EAX)),
                new InstructionVertex("push", OperandSize.Dword, EmitHelpers.Register(Register.EAX)),
                new InstructionVertex("push", OperandSize.Dword, new IntegerOperand(0)),
                new InstructionVertex("cmp", OperandSize.Dword),
            };

            var trueBranchFlow = Operand2.Generate(context, this);
            var falseBranchFlow = Operand3.Generate(context, this);

            trueBranchFlow.Start.IsJumpTarget = true;
            falseBranchFlow.Start.IsJumpTarget = true;

            var comparisonInstruction = (ControlFlowVertex)(conditionEvaluation.Last());
            comparisonInstruction.ConnectTo(falseBranchFlow, FlowEdgeType.JumpIfNotEqual);
            comparisonInstruction.ConnectTo(trueBranchFlow, FlowEdgeType.JumpIfEqual);
            
            trueBranchFlow.ConnectTo(after, FlowEdgeType.UnconditionalJump);
            falseBranchFlow.ConnectTo(after, FlowEdgeType.UnconditionalJump);

            conditionFlow.ConnectTo(conditionEvaluation.First(), FlowEdgeType.DirectFlow);
        }
    }
}