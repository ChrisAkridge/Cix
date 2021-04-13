using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.EmitStatements
{
    internal sealed class ContinueStatement : EmitStatement
    {
        public override StartEndVertices Generate(EmitContext context, EmitStatement parent)
        {
            // WYLO: oh, no.
            // oh, no.
            // So, generating the second pass of the control graph relies on you
            // knowing what your "after" node is, but since we're generating the
            // tree from start to end, your afters won't be generated yet, either.
            //
            // We're lucky that there aren't TOO many flows that require jumps,
            // though. Internal jumps (like in switch or if) aren't ones I care
            // about, though, just breaks, continues, ternary conditionals, and
            // jumps to after vertices.
            //
            // Break/Continue Statements:
            //  - Statements that push new break targets: case statements,
            //    do-while statements, while statements
            //  - Statements that push new continue targets: do-while statements,
            //    while statements.
            //
            // We still need to do two passes - one to generate the overall flow,
            // then one to link up all the stray targets. Here's how this works:
            //  1. Recursively call Generate to get the overall control flow graph.
            //  2. Loop through the function's statements again (and through a
            //     block's statements when you find a block).
            //      a. For each statement, call a method to connect unconnected
            //         vertices, passing in the next statement in the block as the
            //         after/break target, and the statement itself as a continue
            //         target. Functions and blocks pass to nested blocks their
            //         after target, which is what the block uses when its on the
            //         last statement in its list.
            // Remove UngeneratedVertex. Add a property to ControlFlowVertex called
            // ConnectionRequiredType, one of { NoConnectionRequired, ToBreakOrAfterTarget,
            // ToContinueTarget }. Remove ISecondPassConnect and move all the code to generate
            // stuff into Generate.
            //
            // Ternary conditionals are special, sadly. But we can use a~~n ugly hack~~
            // clever trick. Add a jump-target nop instruction immediately after the
            // false branch and make the true branch jump there. Then, have the
            // false branch just direct flow through the nop and into the rest of the
            // expression flow.
            if (!context.ContinueTargets.TryPop(out var breakTarget))
            {
                throw new InvalidOperationException("No continue target");
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