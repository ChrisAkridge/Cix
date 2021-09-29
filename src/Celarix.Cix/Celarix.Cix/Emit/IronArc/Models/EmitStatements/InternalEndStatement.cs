using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.EmitStatements
{
    internal sealed class InternalEndStatement : EmitStatement
    {
        public override GeneratedFlow Generate(EmitContext context, EmitStatement parent) =>
            new GeneratedFlow
            {
                ControlFlow = EmitHelpers.ConnectWithDirectFlow(new IConnectable[]
                {
                    new CommentPrinterVertex(OriginalCode),
                    new InstructionVertex("end", OperandSize.NotUsed)
                }),
                UnconnectedJumps = new List<UnconnectedJump>()
            };
    }
}
