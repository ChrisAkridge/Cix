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
                var currentAfterJumps =
                    current.UnconnectedJumps.Where(j => j.TargetType == JumpTargetType.ToAfterTarget);
                var currentBreakJumps =
                    current.UnconnectedJumps.Where(j => j.TargetType == JumpTargetType.ToBreakTarget);
                
                // After targets
                var afterTarget = next?.ControlFlow.Start ?? (ControlFlowVertex)resetStackAfterBlock[0];
                foreach (var jump in currentAfterJumps)
                {
                    logger.Trace($"Connected after target inside block");
                    afterTarget.IsJumpTarget = true;
                    jump.SourceVertex.ConnectTo(afterTarget, jump.FlowType);
                }

                current.UnconnectedJumps.RemoveAll(j => j.TargetType == JumpTargetType.ToAfterTarget);

                // Break targets
                if (!(current.ControlFlow.Start is CommentPrinterVertex currentCodeComment))
                {
                    throw new InvalidOperationException("Internal compiler error: control flow not commented");
                }

                var commentText = currentCodeComment.CommentText;
                if (!commentText.StartsWith("while", StringComparison.Ordinal)
                    && !commentText.StartsWith("do", StringComparison.Ordinal)
                    && !commentText.StartsWith("switch", StringComparison.Ordinal))
                {
                    continue;
                    // by the way, ew
                }
                
                var breakTarget = next?.ControlFlow.Start;
                if (breakTarget == null)
                {
                    continue;
                }

                foreach (var jump in currentBreakJumps)
                {
                    logger.Trace($"Connected break statement inside block");
                    breakTarget.IsJumpTarget = true;
                    jump.SourceVertex.ConnectTo(breakTarget, jump.FlowType);
                }

                current.UnconnectedJumps.RemoveAll(j => j.TargetType == JumpTargetType.ToBreakTarget);
            }

            while (context.CurrentStack.Size > stackSizeBeforeNewScope)
            {
                context.CurrentStack.Pop();
            }

            var unconnectedJumps = statementFlows.SelectMany(f => f.UnconnectedJumps).ToList();

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