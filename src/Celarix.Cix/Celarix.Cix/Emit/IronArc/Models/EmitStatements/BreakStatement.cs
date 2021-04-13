using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.EmitStatements
{
    internal sealed class BreakStatement : EmitStatement
    {
        public override StartEndVertices Generate(EmitContext context, EmitStatement parent)
        {
            if (!context.BreakTargets.TryPop(out var breakTarget))
            {
                throw new InvalidOperationException("No break target");
            }

            breakTarget.IsJumpTarget = true;
            var jumpToTarget = new InstructionVertex("jmp", OperandSize.NotUsed, new JumpTargetOperand(breakTarget));

            return new StartEndVertices
            {
                Start = jumpToTarget, End = jumpToTarget
            };
        }
    }
}