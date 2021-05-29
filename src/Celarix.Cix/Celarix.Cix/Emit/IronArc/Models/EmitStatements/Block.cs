using System;
using System.Collections.Generic;
using System.Linq;
using Celarix.Cix.Compiler.Common;
using Celarix.Cix.Compiler.Extensions;
using NLog;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.EmitStatements
{
    internal sealed class Block : EmitStatement
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        
        public List<EmitStatement> Statements { get; set; }

        public override GeneratedFlow Generate(EmitContext context, EmitStatement parent)
        {
            logger.Trace($"Generating code for block ({Statements.Count} statement(s))");
            
            var stackSizeBeforeNewScope = context.CurrentStack.Size;
            var statementFlows = Statements.Select(s => s.Generate(context, this)).ToList();
            var stackSizeAfterNewScope = context.CurrentStack.Size;
            var resetStackAfterBlock = new IConnectable[]
            {
                new InstructionVertex("subl", OperandSize.Qword, EmitHelpers.Register(Register.ESP),
                    new IntegerOperand(stackSizeAfterNewScope - stackSizeBeforeNewScope),
                    EmitHelpers.Register(Register.ESP))
            };

            foreach (var (current, next) in statementFlows.Pairwise())
            {
                var currentJumps = current.UnconnectedJumps;
                var breakAfterTarget = (ControlFlowVertex)resetStackAfterBlock[0];

                foreach (var jump in currentJumps.Where(j => j.TargetType == JumpTargetType.ToBreakOrAfterTarget))
                {
                    logger.Trace($"Connected break statement inside block");
                    breakAfterTarget.IsJumpTarget = true;
                    jump.SourceVertex.ConnectTo(breakAfterTarget, jump.FlowType);
                }

                currentJumps.RemoveAll(j => j.TargetType == JumpTargetType.ToBreakOrAfterTarget);
            }

            while (context.CurrentStack.Size > stackSizeBeforeNewScope)
            {
                context.CurrentStack.Pop();
            }

            var unconnectedJumps = statementFlows.SelectMany(f => f.UnconnectedJumps).ToList();

            if (parent is CaseStatement)
            {
                unconnectedJumps.Add(new UnconnectedJump
                {
                    FlowType = FlowEdgeType.UnconditionalJump,
                    TargetType = JumpTargetType.ToBreakOrAfterTarget 
                });
            }

            return new GeneratedFlow
            {
                ControlFlow =
                    EmitHelpers.ConnectWithDirectFlow(statementFlows.Select(f => f.ControlFlow)
                        .Concat(resetStackAfterBlock)),
                UnconnectedJumps = unconnectedJumps
            };
        }
    }
}