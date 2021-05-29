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
            var commentPrinterVertex = new CommentPrinterVertex(OriginalCode)
            {
                IsJumpTarget = true
            };

            var comparisonFlow = EmitHelpers.ConnectWithDirectFlow(new IConnectable[]
            {
                commentPrinterVertex, 
                Condition.Generate(context, null),
                EmitHelpers.ChangeWidthOfTopOfStack(context, conditionSize, OperandSize.Dword),
                new InstructionVertex("push", OperandSize.Dword, new IntegerOperand(0)),
                new InstructionVertex("cmp", OperandSize.Dword),
            });
            var trueFlow = IfTrue.Generate(context, this);
            var falseFlow = IfFalse?.Generate(context, this);

            var unconnectedJumps = new List<UnconnectedJump>
            {
                new UnconnectedJump
                {
                    SourceVertex = trueFlow.ControlFlow.End,
                    FlowType = FlowEdgeType.UnconditionalJump,
                    TargetType = JumpTargetType.ToAfterTarget
                }
            };
            
            // WYLO: read the Notepad++ doc
            
            comparisonFlow.ConnectTo(trueFlow, FlowEdgeType.JumpIfNotEqual);

            if (falseFlow != null)
            {
                comparisonFlow.ConnectTo(falseFlow, FlowEdgeType.JumpIfEqual);
                
                unconnectedJumps.Add(new UnconnectedJump
                {
                    SourceVertex = falseFlow.ControlFlow.End,
                    FlowType = FlowEdgeType.UnconditionalJump,
                    TargetType = JumpTargetType.ToAfterTarget
                });
            }
            else
            {
                unconnectedJumps.Add(new UnconnectedJump
                {
                    SourceVertex = comparisonFlow.End,
                    FlowType = FlowEdgeType.UnconditionalJump,
                    TargetType = JumpTargetType.ToAfterTarget
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