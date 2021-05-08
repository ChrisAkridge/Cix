using System;
using System.Collections.Generic;
using System.Linq;
using Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.EmitStatements
{
    internal sealed class WhileStatement : EmitStatement
    {
        public TypedExpression Condition { get; set; }
        public EmitStatement LoopStatement { get; set; }

        public override GeneratedFlow Generate(EmitContext context, EmitStatement parent)
        {
            Condition.ComputeType(context, null);
            
            var conditionFlow = Condition.Generate(context, null);
            conditionFlow.Start.IsJumpTarget = true;

            var comparisonFlow = EmitHelpers.ConnectWithDirectFlow(new IConnectable[]
            {
                new CommentPrinterVertex(OriginalCode), 
                conditionFlow,
                EmitHelpers.ChangeWidthOfTopOfStack(
                    EmitHelpers.ToOperandSize(Condition.ComputedType.Size),
                    OperandSize.Dword),
                new InstructionVertex("push", OperandSize.Dword, new IntegerOperand(0)),
                new InstructionVertex("cmp", OperandSize.Dword),
            });

            var loopFlow = LoopStatement.Generate(context, this);

            foreach (var jump in loopFlow.UnconnectedJumps.Where(j => j.TargetType == JumpTargetType.ToContinueTarget))
            {
                jump.JumpVertex.ConnectTo(conditionFlow, jump.FlowType);
            }

            loopFlow.UnconnectedJumps.RemoveAll(j => j.TargetType == JumpTargetType.ToContinueTarget);
            
            comparisonFlow.ConnectTo(loopFlow, FlowEdgeType.JumpIfNotEqual);
            loopFlow.ConnectTo(comparisonFlow, FlowEdgeType.UnconditionalJump);
            var jumpToAfter = new UnconnectedJump
            {
                JumpVertex = loopFlow.ControlFlow.End,
                FlowType = FlowEdgeType.JumpIfEqual,
                TargetType = JumpTargetType.ToBreakOrAfterTarget
            };

            return new GeneratedFlow
            {
                ControlFlow = comparisonFlow,
                UnconnectedJumps = loopFlow.UnconnectedJumps
            };
        }
    }
}