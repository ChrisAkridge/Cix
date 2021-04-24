using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celarix.Cix.Compiler.Emit.IronArc.Models;
using Celarix.Cix.Compiler.Emit.IronArc.Models.EmitStatements;
using Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions;
using Celarix.Cix.Compiler.Parse.Models.AST.v1;
using Block = Celarix.Cix.Compiler.Parse.Models.AST.v1.Block;
using BreakStatement = Celarix.Cix.Compiler.Parse.Models.AST.v1.BreakStatement;
using CaseStatement = Celarix.Cix.Compiler.Parse.Models.AST.v1.CaseStatement;
using ConditionalStatement = Celarix.Cix.Compiler.Parse.Models.AST.v1.ConditionalStatement;
using ContinueStatement = Celarix.Cix.Compiler.Parse.Models.AST.v1.ContinueStatement;
using DoWhileStatement = Celarix.Cix.Compiler.Parse.Models.AST.v1.DoWhileStatement;
using ExpressionStatement = Celarix.Cix.Compiler.Parse.Models.AST.v1.ExpressionStatement;
using Function = Celarix.Cix.Compiler.Parse.Models.AST.v1.Function;
using ReturnStatement = Celarix.Cix.Compiler.Parse.Models.AST.v1.ReturnStatement;
using SwitchStatement = Celarix.Cix.Compiler.Parse.Models.AST.v1.SwitchStatement;
using VariableDeclaration = Celarix.Cix.Compiler.Parse.Models.AST.v1.VariableDeclaration;
using VariableDeclarationWithInitialization = Celarix.Cix.Compiler.Parse.Models.AST.v1.VariableDeclarationWithInitialization;
using WhileStatement = Celarix.Cix.Compiler.Parse.Models.AST.v1.WhileStatement;

namespace Celarix.Cix.Compiler.Emit.IronArc
{
    internal sealed class EmitStatementBuilder
    {
        private readonly EmitContext emitContext;
        private readonly TypedExpressionBuilder expressionBuilder;

        public EmitStatementBuilder(EmitContext emitContext)
        {
            this.emitContext = emitContext;
            expressionBuilder = new TypedExpressionBuilder(this.emitContext);
        }

        public EmitStatement Build(Statement statement)
        {
            return statement switch
            {
                Block block => new Models.EmitStatements.Block
                {
                    Statements = block.Statements.Select(Build).ToList()
                },
                BreakStatement _ => new Models.EmitStatements.BreakStatement(),
                CaseStatement caseStatement => new Models.EmitStatements.CaseStatement
                {
                    CaseLiteral = (Models.TypedExpressions.Literal)expressionBuilder.Build(caseStatement.CaseLiteral),
                    Statement = Build(caseStatement.Statement)
                },
                ConditionalStatement conditionalStatement => new Models.EmitStatements.ConditionalStatement
                {
                    Condition = expressionBuilder.Build(conditionalStatement.Condition),
                    IfTrue = Build(conditionalStatement.IfTrue),
                    IfFalse = (conditionalStatement.IfFalse != null) ? Build(conditionalStatement.IfFalse) : null
                },
                ContinueStatement _ => new Models.EmitStatements.ContinueStatement(),
                DoWhileStatement doWhileStatement => new Models.EmitStatements.DoWhileStatement
                {
                    Condition = expressionBuilder.Build(doWhileStatement.Condition),
                    LoopStatement = Build(doWhileStatement.LoopStatement)
                },
                ExpressionStatement expressionStatement => new Models.EmitStatements.ExpressionStatement
                {
                    Expression = expressionBuilder.Build(expressionStatement.Expression)
                },
                ReturnStatement returnStatement => new Models.EmitStatements.ReturnStatement
                {
                    ReturnValue = (returnStatement.ReturnValue != null)
                        ? expressionBuilder.Build(returnStatement.ReturnValue)
                        : null
                },
                SwitchStatement switchStatement => new Models.EmitStatements.SwitchStatement
                {
                    Expression = expressionBuilder.Build(switchStatement.Expression),
                    Cases = switchStatement.Cases.Select(Build).Cast<Models.EmitStatements.CaseStatement>().ToList()
                },
                VariableDeclarationWithInitialization variableDeclarationWithInitialization => new
                    Models.EmitStatements.VariableDeclarationWithInitialization
                    {
                        Type = UsageTypeInfo.FromTypeInfo(
                            emitContext.LookupDataType(variableDeclarationWithInitialization.Type), variableDeclarationWithInitialization.Type.PointerLevel),
                        Name = variableDeclarationWithInitialization.Name,
                        Initializer = expressionBuilder.Build(variableDeclarationWithInitialization.Initializer)
                    },
                VariableDeclaration variableDeclaration => new Models.EmitStatements.VariableDeclaration
                {
                    Type = UsageTypeInfo.FromTypeInfo(emitContext.LookupDataType(variableDeclaration.Type), variableDeclaration.Type.PointerLevel),
                    Name = variableDeclaration.Name
                },
                WhileStatement whileStatement => new Models.EmitStatements.WhileStatement
                {
                    Condition = expressionBuilder.Build(whileStatement.Condition),
                    LoopStatement = Build(whileStatement.LoopStatement)
                },
                _ => throw new InvalidOperationException("Internal compiler error: unknown statement type found")
            };
        }
    }
}
