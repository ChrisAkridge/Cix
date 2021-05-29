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
            context.BreakContexts.Push(new BreakContext
            {
                StackSizeAtStart = context.CurrentStack.Size,
                SupportsContinue = false
            });
            
            Expression.ComputeType(context, null);
            var codeComment = new CommentPrinterVertex(OriginalCode);
            var expressionFlow = Expression.Generate(context, null);
            var operandSize = EmitHelpers.ToOperandSize(Expression.ComputedType.Size);

            var setUpComparisonRegister = new InstructionVertex("pop", operandSize,
                EmitHelpers.Register(Register.EBX));
            context.CurrentStack.Pop();

            codeComment.ConnectTo(expressionFlow, FlowEdgeType.DirectFlow);
            expressionFlow.ConnectTo(setUpComparisonRegister, FlowEdgeType.DirectFlow);

            var switchBlockCodeAndJumps = Cases
                .Select(c =>
                {
                    var blockFlow = c.Statement.Generate(context, this);
                    blockFlow.ControlFlow.Start.IsJumpTarget = true;

                    if (c.CaseLiteral != null)
                    {
                        c.CaseLiteral.ComputeType(context, null);
                        
                        var comparisonInstruction = new InstructionVertex("cmp", operandSize);
                        comparisonInstruction.ConnectTo(blockFlow.ControlFlow, FlowEdgeType.JumpIfEqual);

                        var literalFlow = new IConnectable[]
                        {
                            new InstructionVertex("push", operandSize, EmitHelpers.Register(Register.EBX)),
                            c.CaseLiteral.Generate(context, null),
                            comparisonInstruction
                        };

                        context.CurrentStack.Pop();

                        return new
                        {
                            BlockFlow = blockFlow, LiteralFlow = EmitHelpers.ConnectWithDirectFlow(literalFlow)
                        };
                    }
                    else
                    {
                        var jumpToDefault = new JumpPlaceholderInstruction();
                        jumpToDefault.ConnectTo(blockFlow.ControlFlow, FlowEdgeType.UnconditionalJump);

                        return new
                        {
                            BlockFlow = blockFlow, LiteralFlow = StartEndVertices.MakePair(jumpToDefault)
                        };
                    }
                })
                .ToList();

            var literalFlows = EmitHelpers.ConnectWithDirectFlow(switchBlockCodeAndJumps
                .Select(block => block.LiteralFlow)
                .ToList());

            literalFlows.End.JumpTargetType = JumpTargetType.ToAfterTarget;
            setUpComparisonRegister.ConnectTo(literalFlows, FlowEdgeType.DirectFlow);

            context.BreakContexts.Pop();

            return new GeneratedFlow
            {
                ControlFlow = new StartEndVertices(codeComment, literalFlows.End),
                UnconnectedJumps = switchBlockCodeAndJumps.SelectMany(bcj => bcj.BlockFlow.UnconnectedJumps).ToList()
            };
        }
    }
}