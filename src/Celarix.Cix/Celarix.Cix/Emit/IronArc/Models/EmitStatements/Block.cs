using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.EmitStatements
{
    internal sealed class Block : EmitStatement
    {
        public List<EmitStatement> Statements { get; set; }

        public override StartEndVertices Generate(EmitContext context, EmitStatement parent)
        {
            var stackSizeBeforeNewScope = context.CurrentStack.Size;
            var statementFlows = Statements.Select(s => s.Generate(context, this));
            var stackSizeAfterNewScope = context.CurrentStack.Size;
            var resetStackAfterBlock = new IConnectable[]
            {
                new InstructionVertex("subl", OperandSize.Qword, EmitHelpers.Register(Register.ESP),
                    new IntegerOperand(stackSizeAfterNewScope - stackSizeBeforeNewScope),
                    EmitHelpers.Register(Register.ESP))
            };

            while (context.CurrentStack.Size > stackSizeBeforeNewScope)
            {
                context.CurrentStack.Pop();
            }

            return EmitHelpers.ConnectWithDirectFlow(statementFlows.Concat(resetStackAfterBlock));
        }
    }
}