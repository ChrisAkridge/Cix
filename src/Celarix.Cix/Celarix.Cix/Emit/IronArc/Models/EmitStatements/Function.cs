using System;
using System.Collections.Generic;
using System.Linq;
using Celarix.Cix.Compiler.Common;
using Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions;
using Celarix.Cix.Compiler.Parse.Models.AST.v1;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.EmitStatements
{
    internal sealed class Function : EmitStatement
    {
        public Parse.Models.AST.v1.Function ASTFunction { get; set; }

        public override GeneratedFlow Generate(EmitContext context, EmitStatement parent)
        {
            if (ASTFunction.ReturnType is NamedDataType namedType && namedType.Name == "void")
            {
                ASTFunction.Statements.Add(new Parse.Models.AST.v1.ReturnStatement());
            }
            else if (!ASTFunction.Statements.Any())
            {
                throw new InvalidOperationException("Function doesn't return");
            }
            else
            {
                var lastStatement = ASTFunction.Statements.Last();
                var isLastStatementAReturn = lastStatement is Parse.Models.AST.v1.ReturnStatement;

                if (isLastStatementAReturn)
                {
                    var returnStatement = lastStatement as Parse.Models.AST.v1.ReturnStatement;

                    if (returnStatement.ReturnValue == null)
                    {
                        throw new InvalidOperationException("Function doesn't end in return");
                    }
                }
            }
            
            context.CurrentFunction = ASTFunction;
            foreach (var argument in ASTFunction.Parameters)
            {
                context.CurrentStack.Entries.Push(new VirtualStackEntry(argument.Name, context.LookupDataTypeWithPointerLevel(argument.Type)));
            }
            
            var statementBuilder = new EmitStatementBuilder(context);
            var emitStatements = ASTFunction.Statements.Select(f => statementBuilder.Build(f)).ToList();
            var statementFlows = emitStatements.Select(s => s.Generate(context, this)).ToList();
            var statementWindowedEnumerator = new WindowedEnumerator<GeneratedFlow>(statementFlows.GetEnumerator());

            while (statementWindowedEnumerator.MoveNext())
            {
                var currentTriplet = statementWindowedEnumerator.Current;
                var currentJumps = currentTriplet.Current.UnconnectedJumps;

                var breakAfterTarget = currentTriplet.Next?.ControlFlow?.Start;

                foreach (var jump in currentJumps.Where(j => j.TargetType == JumpTargetType.ToBreakOrAfterTarget))
                {
                    if (breakAfterTarget == null)
                    {
                        throw new InvalidOperationException("Statement broke out to a place after the end of the function");
                    }
                    
                    breakAfterTarget.IsJumpTarget = true;
                    jump.JumpVertex.ConnectTo(breakAfterTarget, jump.FlowType);
                }

                currentJumps.RemoveAll(j => j.TargetType == JumpTargetType.ToBreakOrAfterTarget);
            }

            return new GeneratedFlow
            {
                ControlFlow = EmitHelpers.ConnectWithDirectFlow(statementFlows.Select(sf => sf.ControlFlow))
            };

            // WYLO: Okay, we're on the right track.
            //
            // A statement generates its code, noting which of its vertices jump
            // to break/after/continue targets, marking them accordingly. It then
            // passes it back up to its caller, who wires up the targets if it
            // knows the target, or passes it up again if it doesn't.
            //
            // Check BreakStatement for an example of a generation of an unconnected
            // jump and this method for an example of the connection of the jumps.
            //
            // For each statement type:
            // Blocks: block =DF=> after
            // Conditionals (no else):
            //       Compute condition
            // =DF=> Compare with 0 =EQ=> after
            // =NE=> True branch =DF=> after
            // Conditionals (with else):
            //       Compute condition
            // =DF=> Compare with 0 =EQ=> False branch => after
            // =NE=> True branch =DF=> after
            // Do-while statements:
            //       Loop body   <======+
            // =DF=> Compute condition  |
            // =DF=> Compare with 0 =NE=+
            //       after <=========EQ=+
            // Switch statement:
            //       Compute expression
            // =DF=> Compare to case 0 =EQ=> Case 0 +
            // =NE=> Compare to case 1 =EQ=> Case 1 +
            // =NE=> Compare to case 2 =EQ=> Case 2 +
            // =NE=> Default   <=DF=================+
            //       after
            // While statement:
            //       Compute condition <=========+
            // =DF=> Compare with 0 =EQ=> after  |
            // =NE=> Loop body =DF===============+
            //
            // Expressions never branch to anywhere in the method, which is quite convenient.
        }
    }
}