﻿using System;
using System.Collections.Generic;
using System.Linq;
using NLog;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions
{
    internal sealed class HardwareCallVoidInternal : TypedExpression
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public Celarix.Cix.Compiler.Parse.Models.AST.v1.HardwareCallVoidInternal ASTNode { get; set; }
        public string CallName { get; set; }
        public List<UsageTypeInfo> ParameterTypes { get; set; }

        public override UsageTypeInfo ComputeType(EmitContext context, TypedExpression parent)
        {
            ComputedType = new UsageTypeInfo
            {
                DeclaredType = new NamedTypeInfo
                {
                    Name = "void", Size = 0
                }
            };
            
            logger.Trace($"Hardware call {CallName} returns void");

            return ComputedType;
        }

        public override StartEndVertices Generate(EmitContext context, TypedExpression parent)
        {
            logger.Trace($"Generating code for hardware call {CallName}");
            
            var pushArguments = new List<IConnectable>();
            var parameterSizeSum = 0;

            foreach (var parameterType in ParameterTypes)
            {
                if (parameterType.Size == 1
                    || parameterType.Size == 2
                    || parameterType.Size == 4
                    || parameterType.Size == 8)
                {
                    var operandSize = EmitHelpers.ToOperandSize(parameterType.Size);
                    pushArguments.Add(new InstructionVertex("push", operandSize, EmitHelpers.Register(Register.EBP, isPointer: true, parameterSizeSum)));
                }
                else
                {
                    pushArguments.Add(new InstructionVertex("addl", OperandSize.Qword,
                        EmitHelpers.Register(Register.ESP), new IntegerOperand(parameterType.Size),
                        EmitHelpers.Register(Register.ESP)));
                    pushArguments.Add(new InstructionVertex("movln", OperandSize.NotUsed,
                        EmitHelpers.Register(Register.EBP, isPointer: true, parameterSizeSum),
                        EmitHelpers.Register(Register.ESP, isPointer: true, -parameterType.Size),
                        new IntegerOperand(parameterType.Size)));
                }

                parameterSizeSum += parameterType.Size;
            }

            var hwcallInstruction = new InstructionVertex("hwcall", OperandSize.NotUsed,
                new StringLiteralOperand
                {
                    Literal = CallName
                });

            if (pushArguments.Any())
            {
                var pushArgumentsFlow = EmitHelpers.ConnectWithDirectFlow(pushArguments);
                pushArgumentsFlow.End.ConnectTo(hwcallInstruction, FlowEdgeType.DirectFlow);

                return new StartEndVertices(pushArgumentsFlow.ConnectionTarget, hwcallInstruction);
            }
            else
            {
                return StartEndVertices.MakePair(hwcallInstruction);
            }
        }
    }
}