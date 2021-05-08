using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.EmitStatements
{
    internal class VariableDeclaration : EmitStatement
    {
        public UsageTypeInfo Type { get; set; }
        public string Name { get; set; }

        public override GeneratedFlow Generate(EmitContext context, EmitStatement parent)
        {
            context.CurrentStack.Push(new VirtualStackEntry(Name, Type));
            
            var codeComment = new CommentPrinterVertex(OriginalCode);
            
            if (EmitHelpers.IsIronArcOperandSize(Type.Size))
            {
                var declareVariableFlow = new InstructionVertex("push", EmitHelpers.ToOperandSize(Type.Size),
                    new IntegerOperand(0));

                codeComment.ConnectTo(declareVariableFlow, FlowEdgeType.DirectFlow);
                
                return new GeneratedFlow
                {
                    ControlFlow = new StartEndVertices(codeComment, declareVariableFlow),
                    UnconnectedJumps = new List<UnconnectedJump>()
                };
            }
            else
            {
                var pushOperands = new List<OperandSize>();
                var remainingTypeBytes = Type.Size;

                while (remainingTypeBytes > 0)
                {
                    if (remainingTypeBytes >= 8)
                    {
                        pushOperands.Add(OperandSize.Qword);
                        remainingTypeBytes -= 8;
                    }
                    else if (remainingTypeBytes >= 4)
                    {
                        pushOperands.Add(OperandSize.Dword);
                        remainingTypeBytes -= 4;
                    }
                    else if (remainingTypeBytes >= 2)
                    {
                        pushOperands.Add(OperandSize.Word);
                        remainingTypeBytes -= 2;
                    }
                    else
                    {
                        pushOperands.Add(OperandSize.Byte);
                        remainingTypeBytes -= 1;
                    }
                }

                var pushFlow = EmitHelpers.ConnectWithDirectFlow(pushOperands.Select(s =>
                    new InstructionVertex("push", s, new IntegerOperand(0))));
                
                codeComment.ConnectTo(pushFlow, FlowEdgeType.DirectFlow);

                return new GeneratedFlow
                {
                    ControlFlow = new StartEndVertices(codeComment, pushFlow.End),
                    UnconnectedJumps = new List<UnconnectedJump>()
                };
            }
        }
    }
}