using System;
using System.Collections.Generic;
using System.Linq;
using Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.EmitStatements
{
    internal sealed class SwitchStatement : EmitStatement
    {
        public TypedExpression Expression { get; set; }
        public List<CaseStatement> Cases { get; set; }

        public override GeneratedFlow Generate(EmitContext context, EmitStatement parent)
        {
            var expressionFlow = Expression.Generate(context, null);
            var operandSize = EmitHelpers.ToOperandSize(Expression.ComputedType.Size);

            expressionFlow.ConnectTo(
                new InstructionVertex("pop", operandSize,
                    EmitHelpers.Register(Register.EBX)), FlowEdgeType.DirectFlow);

            var switchBlockCodeAndJumps = Cases
                .Select(c =>
                {
                    var blockFlow = c.Statement.Generate(context, this);
                    blockFlow.ControlFlow.Start.IsJumpTarget = true;

                    var comparisonInstruction = new InstructionVertex("cmp", operandSize);
                    comparisonInstruction.ConnectTo(blockFlow.ControlFlow, FlowEdgeType.JumpIfEqual);

                    var literalFlow = new IConnectable[]
                    {
                        new InstructionVertex("push", operandSize, EmitHelpers.Register(Register.EBX)),
                        c.CaseLiteral.Generate(context, null),
                        comparisonInstruction
                    };

                    return new
                    {
                        BlockFlow = blockFlow, LiteralFlow = EmitHelpers.ConnectWithDirectFlow(literalFlow)
                    };
                })
                .ToList();

            var literalFlows = EmitHelpers.ConnectWithDirectFlow(switchBlockCodeAndJumps
                .Select(block => block.LiteralFlow)
                .ToList());

            literalFlows.End.JumpTargetType = JumpTargetType.ToBreakOrAfterTarget;
            expressionFlow.ConnectTo(literalFlows, FlowEdgeType.DirectFlow);

            return new GeneratedFlow
            {
                ControlFlow = new StartEndVertices
                {
                    Start = expressionFlow.Start, End = literalFlows.End
                },
                UnconnectedJumps = switchBlockCodeAndJumps.SelectMany(bcj => bcj.BlockFlow.UnconnectedJumps).ToList()
            };
        }
    }
}