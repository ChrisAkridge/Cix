using System;
using System.Collections.Generic;
using System.Linq;
using Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.EmitStatements
{
    internal sealed class Function : EmitStatement
    {
        public Parse.Models.AST.v1.Function ASTFunction { get; set; }

        public override StartEndVertices Generate(EmitContext context, EmitStatement parent)
        {
            context.CurrentFunction = ASTFunction;
            foreach (var argument in ASTFunction.Parameters)
            {
                context.CurrentStack.Entries.Push(new VirtualStackEntry(argument.Name, UsageTypeInfo.FromTypeInfo(
                    Helpers.GetDeclaredType(argument.Type, context.DeclaredTypes),
                    argument.Type.PointerLevel)));
            }
            
            var statementBuilder = new EmitStatementBuilder(context);
            var emitStatements = ASTFunction.Statements.Select(f => statementBuilder.Build(f)).ToList();

            // Attach an implicit, valueless return to the end of the function.
            // If we need to return a value here, the compiler will complain with an error.
            emitStatements.Add(new ReturnStatement());

            var statementFlows = emitStatements.Select(s => s.Generate(context, this));
        }
    }
}