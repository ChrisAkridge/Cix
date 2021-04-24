using System;
using System.Collections.Generic;
using System.Linq;
using NLog;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.EmitStatements
{
    internal sealed class BreakStatement : EmitStatement
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        
        public override GeneratedFlow Generate(EmitContext context, EmitStatement parent)
        {
            logger.Trace("Generating code for break statement...");
            var jumpPlaceholder = new JumpPlaceholderInstruction();

            return new GeneratedFlow
            {
                ControlFlow = StartEndVertices.MakePair(jumpPlaceholder),
                UnconnectedJumps = new List<UnconnectedJump>
                {
                    new UnconnectedJump
                    {
                        JumpVertex = jumpPlaceholder,
                        FlowType = FlowEdgeType.UnconditionalJump,
                        TargetType = JumpTargetType.ToBreakOrAfterTarget
                    }
                }
            };
        }
    }
}