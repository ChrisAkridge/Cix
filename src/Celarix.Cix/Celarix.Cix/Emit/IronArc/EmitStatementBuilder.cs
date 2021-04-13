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
            if (statement is Block block)
            {
                return new Models.EmitStatements.Block
                {
                    Statements = block.Statements.Select(Build).ToList()
                };
            }
            else if (statement is BreakStatement)
            {
                return new Models.EmitStatements.BreakStatement();
            }
            else if (statement is CaseStatement caseStatement)
            {
                return new Models.EmitStatements.CaseStatement
                {
                    CaseLiteral = (Models.TypedExpressions.Literal)expressionBuilder.Build(caseStatement.CaseLiteral),
                    Statement = Build(caseStatement.Statement)
                };
            }
            else if (statement is ConditionalStatement conditionalStatement)
            {
                return new Models.EmitStatements.ConditionalStatement
                {
                    Condition = expressionBuilder.Build(conditionalStatement.Condition),
                    IfTrue = Build(conditionalStatement.IfTrue),
                    IfFalse = (conditionalStatement.IfFalse != null) ? Build(conditionalStatement.IfFalse) : null
                };
            }
            else if (statement is ContinueStatement)
            {
                return new Models.EmitStatements.ContinueStatement();
            }
            else if (statement is DoWhileStatement doWhileStatement)
            {
                return new Models.EmitStatements.DoWhileStatement
                {
                    Condition = expressionBuilder.Build(doWhileStatement.Condition),
                    LoopStatement = Build(doWhileStatement.LoopStatement)
                };
            }
            else if (statement is ExpressionStatement expressionStatement)
            {
                return new Models.EmitStatements.ExpressionStatement
                {
                    Expression = expressionBuilder.Build(expressionStatement.Expression)
                };
            }
            else if (statement is ReturnStatement returnStatement)
            {
                return new Models.EmitStatements.ReturnStatement
                {
                    ReturnValue = (returnStatement.ReturnValue != null)
                        ? expressionBuilder.Build(returnStatement.ReturnValue)
                        : null
                };
            }
            else if (statement is SwitchStatement switchStatement)
            {
                return new Models.EmitStatements.SwitchStatement
                {
                    Expression = expressionBuilder.Build(switchStatement.Expression),
                    Cases = switchStatement.Cases.Select(Build).Cast<Models.EmitStatements.CaseStatement>().ToList()
                };
            }
            else if (statement is VariableDeclarationWithInitialization variableDeclarationWithInitialization)
            {
                return new Models.EmitStatements.VariableDeclarationWithInitialization
                {
                    Type = emitContext.LookupDataType(variableDeclarationWithInitialization.Type),
                    Name = variableDeclarationWithInitialization.Name,
                    Initializer = expressionBuilder.Build(variableDeclarationWithInitialization.Initializer)
                };
            }
            else if (statement is VariableDeclaration variableDeclaration)
            {
                return new Models.EmitStatements.VariableDeclaration
                {
                    Type = emitContext.LookupDataType(variableDeclaration.Type),
                    Name = variableDeclaration.Name
                };
            }
            else if (statement is WhileStatement whileStatement)
            {
                return new Models.EmitStatements.WhileStatement
                {
                    Condition = expressionBuilder.Build(whileStatement.Condition),
                    LoopStatement = Build(whileStatement.LoopStatement)
                };
            }
            else
            {
                throw new InvalidOperationException("Internal compiler error: unknown statement type found");
            }
        }
    }
}
