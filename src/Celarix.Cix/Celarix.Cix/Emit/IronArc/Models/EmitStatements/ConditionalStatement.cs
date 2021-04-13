using System;
using System.Collections.Generic;
using System.Linq;
using Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.EmitStatements
{
    internal sealed class ConditionalStatement : EmitStatement, ISecondPassConnect
    {
        public TypedExpression Condition { get; set; }
        public EmitStatement IfTrue { get; set; }
        public EmitStatement IfFalse { get; set; }

        public override StartEndVertices Generate(EmitContext context, EmitStatement parent) => EmitHelpers.GetUngeneratedVertex(this);

        public void ConnectToGeneratedTree(ControlFlowVertex after, EmitContext context = null)
        {
            var trueFlow = IfTrue.Generate(context, this);
            var falseFlow = IfFalse?.Generate(context, this);

            var conditionSize = EmitHelpers.ToOperandSize(Condition.ComputedType.Size);

            var comparisonFlow = EmitHelpers.ConnectWithDirectFlow(new IConnectable[]
            {
                Condition.Generate(context, null),
                EmitHelpers.ChangeWidthOfTopOfStack(conditionSize, OperandSize.Dword),
                new InstructionVertex("push", OperandSize.Dword, new IntegerOperand(0)),
                new InstructionVertex("cmp", OperandSize.Dword),
            });
            
            comparisonFlow.ConnectTo(trueFlow, FlowEdgeType.JumpIfNotEqual);

            if (falseFlow != null)
            {
                comparisonFlow.ConnectTo(falseFlow, FlowEdgeType.JumpIfEqual);
            }
            else
            {
                comparisonFlow.ConnectTo(after, FlowEdgeType.UnconditionalJump);
            }
            
            trueFlow.ConnectTo(after, FlowEdgeType.UnconditionalJump);
            falseFlow?.ConnectTo(after, FlowEdgeType.UnconditionalJump);
        }
    }
}