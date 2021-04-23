using System;
using System.Collections.Generic;
using System.Linq;
using Celarix.Cix.Compiler.Common;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.EmitStatements
{
    internal sealed class Block : EmitStatement
    {
        public List<EmitStatement> Statements { get; set; }

        public override GeneratedFlow Generate(EmitContext context, EmitStatement parent)
        {
            var stackSizeBeforeNewScope = context.CurrentStack.Size;
            var statementFlows = Statements.Select(s => s.Generate(context, this)).ToList();
            var stackSizeAfterNewScope = context.CurrentStack.Size;
            var resetStackAfterBlock = new IConnectable[]
            {
                new InstructionVertex("subl", OperandSize.Qword, EmitHelpers.Register(Register.ESP),
                    new IntegerOperand(stackSizeAfterNewScope - stackSizeBeforeNewScope),
                    EmitHelpers.Register(Register.ESP))
            };
            
            var statementWindowedEnumerator = new WindowedEnumerator<GeneratedFlow>(statementFlows.GetEnumerator());

            while (statementWindowedEnumerator.MoveNext())
            {
                var currentTriplet = statementWindowedEnumerator.Current;
                var currentJumps = currentTriplet.Current.UnconnectedJumps;
                var breakAfterTarget = currentTriplet.Next?.ControlFlow?.Start ?? (ControlFlowVertex)resetStackAfterBlock[0];

                foreach (var jump in currentJumps.Where(j => j.TargetType == JumpTargetType.ToBreakOrAfterTarget))
                {
                    breakAfterTarget.IsJumpTarget = true;
                    jump.JumpVertex.ConnectTo(breakAfterTarget, jump.FlowType);
                }

                currentJumps.RemoveAll(j => j.TargetType == JumpTargetType.ToBreakOrAfterTarget);
            }

            while (context.CurrentStack.Size > stackSizeBeforeNewScope)
            {
                context.CurrentStack.Pop();
            }

            return new GeneratedFlow
            {
                ControlFlow =
                    EmitHelpers.ConnectWithDirectFlow(statementFlows.Select(f => f.ControlFlow)
                        .Concat(resetStackAfterBlock)),
                UnconnectedJumps = statementFlows.SelectMany(f => f.UnconnectedJumps).ToList()
            };
        }
    }
}