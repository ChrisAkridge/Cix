using System;
using System.Collections.Generic;
using System.Linq;
using Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions;
using Celarix.Cix.Compiler.Parse.Models.AST.v1;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.EmitStatements
{
    internal sealed class ReturnStatement : EmitStatement
    {
        public TypedExpression ReturnValue { get; set; }

        public override GeneratedFlow Generate(EmitContext context, EmitStatement parent)
        {
            var functionReturnsVoid = context.CurrentFunction.ReturnType is NamedDataType namedType
                && namedType.Name == "void"
                && namedType.PointerLevel == 0;
            
            if (ReturnValue == null)
            {
                if (!functionReturnsVoid)
                {
                    throw new InvalidOperationException("Function must return value");
                }

                var returnFlow = new IConnectable[]
                {
                    EmitHelpers.ResetStack(),
                    new InstructionVertex("ret", OperandSize.NotUsed)
                };

                return new GeneratedFlow
                {
                    ControlFlow = EmitHelpers.ConnectWithDirectFlow(returnFlow),
                    UnconnectedJumps = new List<UnconnectedJump>()
                };
            }
            else
            {
                ReturnValue.ComputeType(context, null);
                var returnValueFlow = ReturnValue.Generate(context, null);
                
                if (functionReturnsVoid)
                {
                    throw new InvalidOperationException("Function must not return value");
                }

                var returnValueSize = ReturnValue.ComputedType.Size;
                IConnectable[] returnFlow;

                if (EmitHelpers.IsIronArcOperandSize(returnValueSize))
                {
                    var operandSize = EmitHelpers.ToOperandSize(returnValueSize);

                    returnFlow = new IConnectable[]
                    {
                        new InstructionVertex("pop", operandSize, EmitHelpers.Register(Register.EAX)),
                        EmitHelpers.ResetStack(),
                        new InstructionVertex("push", operandSize, EmitHelpers.Register(Register.EAX)),
                        new InstructionVertex("ret", OperandSize.NotUsed)
                    };
                }
                else
                {
                    var returnValueOffsetFromEBP = context.CurrentStack.Peek().OffsetFromEBP;
                    
                    returnFlow = new IConnectable[]
                    {
                        returnValueFlow,
                        new InstructionVertex("movln", OperandSize.NotUsed,
                            EmitHelpers.Register(Register.EBP, isPointer: true, returnValueOffsetFromEBP),
                            EmitHelpers.Register(Register.EBP, isPointer: true), new IntegerOperand(returnValueSize)),
                        EmitHelpers.ResetStack(),
                        new InstructionVertex("addl", OperandSize.Qword, EmitHelpers.Register(Register.ESP),
                            new IntegerOperand(returnValueSize), EmitHelpers.Register(Register.ESP)),
                        new InstructionVertex("ret", OperandSize.NotUsed)
                    };
                }

                return new GeneratedFlow
                {
                    ControlFlow = EmitHelpers.ConnectWithDirectFlow(returnFlow),
                    UnconnectedJumps = new List<UnconnectedJump>()
                };
            }
        }
    }
}