using System;
using System.Collections.Generic;
using System.Linq;
using Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.EmitStatements
{
    internal sealed class SwitchStatement : EmitStatement, ISecondPassConnect
    {
        public TypedExpression Expression { get; set; }
        public List<CaseStatement> Cases { get; set; }

        public override StartEndVertices Generate(EmitContext context, EmitStatement parent) =>
            EmitHelpers.GetUngeneratedVertex(this);

        public void ConnectToGeneratedTree(ControlFlowVertex after, EmitContext context = null)
        {
            var codeToGenerateExpression = Expression.Generate(context, null);

            var expressionFlow = Expression.Generate(context, null);
            var operandSize = EmitHelpers.ToOperandSize(Expression.ComputedType.Size);
            expressionFlow.ConnectTo(
                new InstructionVertex("pop", operandSize,
                    EmitHelpers.Register(Register.EBX)), FlowEdgeType.DirectFlow);

            var switchBlockCodeAndJumps = Cases.Select(c =>
            {
                var blockFlow = c.Statement.Generate(context, this);
                blockFlow.Start.IsJumpTarget = true;
                blockFlow.ConnectTo(after, FlowEdgeType.UnconditionalJump);

                var literalFlow = new IConnectable[]
                {
                    new InstructionVertex("push", operandSize, EmitHelpers.Register(Register.EBX)),
                    c.CaseLiteral.Generate(context, null),
                    new InstructionVertex("cmp", operandSize),
                    new InstructionVertex("je", OperandSize.NotUsed, new JumpTargetOperand(blockFlow.Start))
                };

                return new
                {
                    BlockFlow = blockFlow,
                    LiteralFlow = EmitHelpers.ConnectWithDirectFlow(literalFlow)
                };
            });

            var literalFlows = EmitHelpers.ConnectWithDirectFlow(switchBlockCodeAndJumps
                .Select(block => block.LiteralFlow)
                .ToList());
            literalFlows.ConnectTo(after, FlowEdgeType.UnconditionalJump);
        }
    }
}