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
            
            if (EmitHelpers.IsIronArcOperandSize(Type.Size))
            {
                return new GeneratedFlow
                {
                    ControlFlow =
                        StartEndVertices.MakePair(new InstructionVertex("push", EmitHelpers.ToOperandSize(Type.Size),
                            new IntegerOperand(0))),
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

                var pushInstructions =
                    pushOperands.Select(o => new InstructionVertex("push", o, new IntegerOperand(0)));

                return new GeneratedFlow
                {
                    ControlFlow = EmitHelpers.ConnectWithDirectFlow(pushInstructions),
                    UnconnectedJumps = new List<UnconnectedJump>()
                };
            }
        }
    }
}