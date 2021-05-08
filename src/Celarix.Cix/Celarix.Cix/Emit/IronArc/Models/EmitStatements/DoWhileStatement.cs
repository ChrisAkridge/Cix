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

            var doComment = new CommentPrinterVertex("do {");
            var loopFlow = LoopStatement.Generate(context, this);
            doComment.ConnectTo(loopFlow.ControlFlow, FlowEdgeType.DirectFlow);

            var conditionSize = EmitHelpers.ToOperandSize(Condition.ComputedType.Size);
            var comparisonFlow = EmitHelpers.ConnectWithDirectFlow(new IConnectable[]
            {
                new CommentPrinterVertex(OriginalCode), 
                Condition.Generate(context, null),
                EmitHelpers.ChangeWidthOfTopOfStack(conditionSize, OperandSize.Dword),
                new InstructionVertex("push", OperandSize.Dword, new IntegerOperand(0)),
                new InstructionVertex("cmp", OperandSize.Dword),
            });
            
            foreach (var jump in loopFlow.UnconnectedJumps.Where(j => j.TargetType == JumpTargetType.ToContinueTarget))
            {
                jump.JumpVertex.ConnectTo(comparisonFlow.Start, FlowEdgeType.UnconditionalJump);
            }
            
            loopFlow.ConnectTo(comparisonFlow, FlowEdgeType.UnconditionalJump);
            comparisonFlow.ConnectTo(loopFlow, FlowEdgeType.JumpIfNotEqual);

            return new GeneratedFlow
            {
                ControlFlow = new StartEndVertices(doComment, loopFlow.ControlFlow.End),
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