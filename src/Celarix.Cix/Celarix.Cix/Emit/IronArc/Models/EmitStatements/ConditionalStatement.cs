using System;
using System.Collections.Generic;
using System.Linq;
using Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.EmitStatements
{
    internal sealed class ConditionalStatement : EmitStatement
    {
        public TypedExpression Condition { get; set; }
        public EmitStatement IfTrue { get; set; }
        public EmitStatement IfFalse { get; set; }

        public override GeneratedFlow Generate(EmitContext context, EmitStatement parent)
        {
            Condition.ComputeType(context, null);
            
            var conditionSize = EmitHelpers.ToOperandSize(Condition.ComputedType.Size);
            var comparisonFlow = EmitHelpers.ConnectWithDirectFlow(new IConnectable[]
            {
                new CommentPrinterVertex(OriginalCode), 
                Condition.Generate(context, null),
                EmitHelpers.ChangeWidthOfTopOfStack(conditionSize, OperandSize.Dword),
                new InstructionVertex("push", OperandSize.Dword, new IntegerOperand(0)),
                new InstructionVertex("cmp", OperandSize.Dword),
            });
            var trueFlow = IfTrue.Generate(context, this);
            var falseFlow = IfFalse?.Generate(context, this);

            var unconnectedJumps = new List<UnconnectedJump>
            {
                new UnconnectedJump
                {
                    JumpVertex = trueFlow.ControlFlow.End,
                    FlowType = FlowEdgeType.UnconditionalJump,
                    TargetType = JumpTargetType.ToBreakOrAfterTarget
                }
            };
            
            comparisonFlow.ConnectTo(trueFlow, FlowEdgeType.JumpIfNotEqual);

            if (falseFlow != null)
            {
                comparisonFlow.ConnectTo(falseFlow, FlowEdgeType.JumpIfEqual);
                
                unconnectedJumps.Add(new UnconnectedJump
                {
                    JumpVertex = falseFlow.ControlFlow.End,
                    FlowType = FlowEdgeType.UnconditionalJump,
                    TargetType = JumpTargetType.ToBreakOrAfterTarget
                });
            }
            else
            {
                unconnectedJumps.Add(new UnconnectedJump
                {
                    JumpVertex = comparisonFlow.End,
                    FlowType = FlowEdgeType.UnconditionalJump,
                    TargetType = JumpTargetType.ToBreakOrAfterTarget
                });
            }

            return new GeneratedFlow
            {
                ControlFlow = comparisonFlow,
                UnconnectedJumps = unconnectedJumps
                    .Concat(trueFlow.UnconnectedJumps)
                    .Concat(falseFlow?.UnconnectedJumps ?? Enumerable.Empty<UnconnectedJump>())
                    .ToList()
            };
        }
    }
}