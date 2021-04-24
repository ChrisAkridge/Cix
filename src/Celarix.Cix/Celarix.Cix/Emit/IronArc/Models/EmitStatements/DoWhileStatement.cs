using System;
using System.Collections.Generic;
using System.Linq;
using Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.EmitStatements
{
    internal sealed class DoWhileStatement : EmitStatement
    {
        public TypedExpression Condition { get; set; }
        public EmitStatement LoopStatement { get; set; }

        public override GeneratedFlow Generate(EmitContext context, EmitStatement parent)
        {
            Condition.ComputeType(context, null);
            
            var conditionSize = EmitHelpers.ToOperandSize(Condition.ComputedType.Size);
            var comparisonFlow = EmitHelpers.ConnectWithDirectFlow(new IConnectable[]
            {
                Condition.Generate(context, null),
                EmitHelpers.ChangeWidthOfTopOfStack(conditionSize, OperandSize.Dword),
                new InstructionVertex("push", OperandSize.Dword, new IntegerOperand(0)),
                new InstructionVertex("cmp", OperandSize.Dword),
            });
            
            var loopFlow = LoopStatement.Generate(context, this);

            foreach (var jump in loopFlow.UnconnectedJumps.Where(j => j.TargetType == JumpTargetType.ToContinueTarget))
            {
                jump.JumpVertex.ConnectTo(comparisonFlow.Start, FlowEdgeType.UnconditionalJump);
            }
            
            loopFlow.ConnectTo(comparisonFlow, FlowEdgeType.UnconditionalJump);
            comparisonFlow.ConnectTo(loopFlow, FlowEdgeType.JumpIfNotEqual);

            return new GeneratedFlow
            {
                ControlFlow = loopFlow.ControlFlow,
                UnconnectedJumps = loopFlow.UnconnectedJumps
                    .Append(new UnconnectedJump
                    {
                        JumpVertex = comparisonFlow.End,
                        FlowType = FlowEdgeType.JumpIfEqual,
                        TargetType = JumpTargetType.ToBreakOrAfterTarget
                    })
                    .ToList()
            };
        }
    }
}