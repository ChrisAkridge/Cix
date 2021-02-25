using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Tree;
using Celarix.Cix.Compiler.Common.Models;
using Celarix.Cix.Compiler.Parse.AST;
using Celarix.Cix.Compiler.Parse.Models.AST;

namespace Celarix.Cix.Compiler.Parse.ANTLR
{
    public sealed class ASTGenerator
    {
        public object GenerateSourceFile(CixParser.SourceFileContext sourceFile)
        {
            var structs = sourceFile.@struct().Select(GenerateStruct).ToList();

            var globalVariableDeclarations =
                sourceFile.globalVariableDeclaration().Select(GenerateGlobalVariableDeclaration).ToList();

            var functions = sourceFile.function().Select(GenerateFunction).ToList();

            return new { Structs = structs, GlobalVariableDeclarations = globalVariableDeclarations, Functions = functions };
        }

        public object GenerateStruct(CixParser.StructContext @struct)
        {
            var name = @struct.Identifier().GetText();
            var members = @struct.structMember().Select(GenerateStructMember).ToList();
            
            return new { Name = name, Members = members };
        }

        public object GenerateStructMember(CixParser.StructMemberContext structMember)
        {
            var type = GenerateTypeName(structMember.typeName());
            var name = structMember.Identifier().GetText();

            var structArraySize = (structMember.structArraySize() != null)
                ? GenerateStructArraySize(structMember.structArraySize())
                : 1;

            return new { Type = type, Name = name, StructArraySize = structArraySize };
        }

        public object GenerateTypeName(CixParser.TypeNameContext typeName)
        {
            var pointerLevel = 0;
            var pointerAsteriskList = typeName.pointerAsteriskList();
            if (pointerAsteriskList != null)
            {
                pointerLevel = pointerAsteriskList.Asterisk().Length;
            }
            
            if (typeName.funcptrTypeName() != null)
            {
                return new { Type = GenerateFuncptrTypeName(typeName.funcptrTypeName()), PointerLevel = pointerLevel };
            }
            else
            {
                var typeIdentifierName = (typeName.primitiveType() != null)
                    ? typeName.primitiveType().GetText()
                    : typeName.Identifier().GetText();

                return new { TypeName = typeIdentifierName, PointerLevel = pointerLevel };
            }
        }

        public object GenerateFuncptrTypeName(CixParser.FuncptrTypeNameContext funcptrTypeName) =>
            new { Types = GenerateTypeNameList(funcptrTypeName.typeNameList()) };

        public object GenerateTypeNameList(CixParser.TypeNameListContext typeNameList)
        {
            var carCdr = GenerateTypeNameListCdr(typeNameList);

            return carCdr.ToList();
        }

        public CarCdr<object> GenerateTypeNameListCdr(CixParser.TypeNameListContext typeNameList)
        {
            var typeListCAR = GenerateTypeName(typeNameList.typeName());
            CarCdr<object> typeListCDR = null;

            if (typeNameList.typeNameList() != null)
            {
                typeListCDR = GenerateTypeNameListCdr(typeNameList.typeNameList());
            }

            return new CarCdr<object>() { Car = typeListCAR, Cdr = typeListCDR };
        }

        public object GenerateStructArraySize(CixParser.StructArraySizeContext structArraySize)
        {
            return new { Size = structArraySize.Integer().GetText() };
        }

        public object GenerateGlobalVariableDeclaration(CixParser.GlobalVariableDeclarationContext globalVariableDeclaration) =>
            globalVariableDeclaration.variableDeclarationStatement() != null
                ? GenerateVariableDeclarationStatement(globalVariableDeclaration.variableDeclarationStatement())
                : GenerateVariableDeclarationWithInitializationStatement(globalVariableDeclaration
                    .variableDeclarationWithInitializationStatement());

        public object GenerateFunction(CixParser.FunctionContext function)
        {
            var returnType = GenerateTypeName(function.typeName());
            var name = function.Identifier().GetText();

            var parameters = (function.functionParameterList() != null)
                ? GenerateFunctionParameterList(function.functionParameterList())
                : new List<object>();

            var statements = function?.statement().Select(GenerateStatement).ToList() ?? new List<object>();

            return new { ReturnType = returnType, Name = name, Parameters = parameters, Statements = statements };
        }

        public object GenerateFunctionParameterList(CixParser.FunctionParameterListContext functionParameterList)
        {
            var functionParameterCAR = GenerateFunctionParameter(functionParameterList.functionParameter());
            var functionParameterCDR = (functionParameterList.functionParameterList() != null)
                ? GenerateFunctionParameterList(functionParameterList.functionParameterList())
                : null;

            return new { CAR = functionParameterCAR, CDR = functionParameterCDR };
        }

        public object GenerateFunctionParameter(CixParser.FunctionParameterContext functionParameter)
        {
            return new
            {
                Type = GenerateTypeName(functionParameter.typeName()),
                Name = functionParameter.Identifier().GetText()
            };
        }

        public object GenerateStatement(CixParser.StatementContext statement)
        {
            if (statement.block() != null) { return GenerateBlock(statement.block()); }
            else if (statement.breakStatement() != null) { return new object(); /* BreakStatement */}
            else if (statement.conditionalStatement() != null) { return GenerateConditionalStatement(statement.conditionalStatement()); }
            else if (statement.continueStatement() != null) { return new object(); /* ContinueStatement */ }
            else if (statement.doWhileStatement() != null) { return GenerateDoWhileStatement(statement.doWhileStatement()); }
            else if (statement.expressionStatement() != null) { return GenerateExpressionStatement(statement.expressionStatement()); }
            else if (statement.forStatement() != null) { return GenerateForStatement(statement.forStatement()); }
            else if (statement.returnStatement() != null) { return GenerateReturnStatement(statement.returnStatement()); }
            else if (statement.switchStatement() != null) { return GenerateSwitchStatement(statement.switchStatement()); }
            else if (statement.variableDeclarationStatement() != null) { return GenerateVariableDeclarationStatement(statement.variableDeclarationStatement()); }
            else if (statement.variableDeclarationWithInitializationStatement() != null) { return GenerateVariableDeclarationWithInitializationStatement(statement.variableDeclarationWithInitializationStatement()); }
            else if (statement.whileStatement() != null) { return GenerateWhileStatement(statement.whileStatement()); }
            else { throw new ArgumentOutOfRangeException(); }
        }

        public object GenerateBlock(CixParser.BlockContext block)
        {
            return new { Statements = block.statement().Select(GenerateStatement).ToList() };
        }

        public object GenerateConditionalStatement(CixParser.ConditionalStatementContext conditionalStatement)
        {
            var expression = GenerateExpression(conditionalStatement.expression());
            var statement = GenerateStatement(conditionalStatement.statement());

            object elseStatement = (conditionalStatement.elseStatement() != null)
                ? GenerateStatement(conditionalStatement.elseStatement().statement())
                : null;

            return new { Expression = expression, Statement = statement, ElseStatement = elseStatement };
        }

        public object GenerateDoWhileStatement(CixParser.DoWhileStatementContext doWhileStatement)
        {
            var statement = GenerateStatement(doWhileStatement.statement());
            var whileExpression = GenerateExpression(doWhileStatement.expression());

            return new { Statement = statement, WhileExpression = whileExpression };
        }

        public object GenerateExpressionStatement(CixParser.ExpressionStatementContext expressionStatement) =>
            new { Expression = GenerateExpression(expressionStatement.expression()) };

        public object GenerateForStatement(CixParser.ForStatementContext forStatement)
        {
            return new
            {
                Initializer = GenerateExpression(forStatement.expression(0)),
                Condition = GenerateExpression(forStatement.expression(1)),
                Iterator = GenerateExpression(forStatement.expression(2)),
                Statement = GenerateStatement(forStatement.statement())
            };
        }

        public object GenerateReturnStatement(CixParser.ReturnStatementContext returnStatement)
        {
            return new
            {
                Expression = (returnStatement.expression() != null)
                    ? GenerateExpression(returnStatement.expression())
                    : null
            };
        }

        public object GenerateSwitchStatement(CixParser.SwitchStatementContext switchStatement)
        {
            return new
            {
                Expression = GenerateExpression(switchStatement.expression()),
                Cases = switchStatement.caseStatement().Select(GenerateCaseStatement).ToList()
            };
        }

        public object GenerateCaseStatement(CixParser.CaseStatementContext caseStatement)
        {
            if (caseStatement.literalCaseStatement() != null)
            {
                var literalCase = caseStatement.literalCaseStatement();

                return new { CaseLiteral = literalCase.Integer()?.GetText() ?? literalCase.StringLiteral().GetText() };
            }
            else
            {
                return new object(); /* DefaultCase */
            }
        }

        public object GenerateWhileStatement(CixParser.WhileStatementContext whileStatement)
        {
            return new
            {
                Expression = GenerateExpression(whileStatement.expression()),
                Statement = GenerateStatement(whileStatement.statement())
            };
        }

        public object GenerateVariableDeclarationStatement(CixParser.VariableDeclarationStatementContext variableDeclarationStatement)
        {
            return new
            {
                Type = GenerateTypeName(variableDeclarationStatement.typeName()),
                Name = variableDeclarationStatement.Identifier().GetText()
            };
        }

        public object GenerateVariableDeclarationWithInitializationStatement(CixParser.VariableDeclarationWithInitializationStatementContext variableDeclarationWithInitializationStatementContext)
        {
            return new
            {
                Type = GenerateTypeName(variableDeclarationWithInitializationStatementContext.typeName()),
                Name = variableDeclarationWithInitializationStatementContext.Identifier().GetText(),
                Initializer = GenerateExpression(variableDeclarationWithInitializationStatementContext.expression())
            };
        }

        public object GenerateExpression(CixParser.ExpressionContext expression)
        {
            return GenerateAssignmentExpression(expression.assignmentExpression());
        }

        public object GenerateAssignmentExpression(CixParser.AssignmentExpressionContext assignmentExpression)
        {
            if (assignmentExpression.conditionalExpression() != null)
            {
                return GenerateConditionalExpression(assignmentExpression.conditionalExpression());
            }

            return new
            {
                Left = GenerateUnaryExpression(assignmentExpression.unaryExpression()),
                Operator = assignmentExpression.assignmentOperator().GetText(),
                Right = GenerateAssignmentExpression(assignmentExpression.assignmentExpression())
            };
        }

        public object GenerateConditionalExpression(CixParser.ConditionalExpressionContext conditionalExpression)
        {
            return conditionalExpression.expression() == null
                ? GenerateLogicalOrExpression(conditionalExpression.logicalOrExpression())
                : new
                {
                    Condition = GenerateLogicalOrExpression(conditionalExpression.logicalOrExpression()),
                    IfTrue = GenerateExpression(conditionalExpression.expression()),
                    IfFalse = GenerateConditionalExpression(conditionalExpression.conditionalExpression())
                };
        }

        public object GenerateLogicalOrExpression(CixParser.LogicalOrExpressionContext logicalOrExpression)
        {
            return logicalOrExpression.logicalOrExpression() == null
                ? GenerateLogicalAndExpression(logicalOrExpression.logicalAndExpression())
                : new
                {
                    Left = GenerateLogicalOrExpression(logicalOrExpression.logicalOrExpression()),
                    Operator = "||",
                    Right = GenerateLogicalAndExpression(logicalOrExpression.logicalAndExpression())
                };
        }

        public object GenerateLogicalAndExpression(CixParser.LogicalAndExpressionContext logicalAndExpression)
        {
            return logicalAndExpression.logicalAndExpression() == null
                ? GenerateInclusiveOrExpression(logicalAndExpression.inclusiveOrExpression())
                : new
                {
                    Left = GenerateLogicalAndExpression(logicalAndExpression.logicalAndExpression()),
                    Operator = "&&",
                    Right = GenerateInclusiveOrExpression(logicalAndExpression.inclusiveOrExpression())
                };
        }

        public object GenerateInclusiveOrExpression(CixParser.InclusiveOrExpressionContext inclusiveOrExpression)
        {
            return inclusiveOrExpression.inclusiveOrExpression() == null
                ? GenerateExclusiveOrExpression(inclusiveOrExpression.exclusiveOrExpression())
                : new
                {
                    Left = GenerateInclusiveOrExpression(inclusiveOrExpression.inclusiveOrExpression()),
                    Operator = "|",
                    Right = GenerateExclusiveOrExpression(inclusiveOrExpression.exclusiveOrExpression())
                };
        }

        public object GenerateExclusiveOrExpression(CixParser.ExclusiveOrExpressionContext exclusiveOrExpression)
        {
            return exclusiveOrExpression.exclusiveOrExpression() == null
                ? GenerateAndExpression(exclusiveOrExpression.andExpression())
                : new
                {
                    Left = GenerateExclusiveOrExpression(exclusiveOrExpression.exclusiveOrExpression()),
                    Operator = "^",
                    Right = GenerateAndExpression(exclusiveOrExpression.andExpression())
                };
        }

        public object GenerateAndExpression(CixParser.AndExpressionContext andExpression)
        {
            return (andExpression.andExpression() == null)
                ? GenerateEqualityExpression(andExpression.equalityExpression())
                : new
                {
                    Left = GenerateAndExpression(andExpression.andExpression()),
                    Operator = "&",
                    Right = GenerateEqualityExpression(andExpression.equalityExpression())
                };
        }

        public object GenerateEqualityExpression(CixParser.EqualityExpressionContext equalityExpressionContext)
        {
            if (equalityExpressionContext.equalityExpression() == null)
            {
                return GenerateRelationalExpression(equalityExpressionContext.relationalExpression());
            }

            return new
            {
                Left = GenerateEqualityExpression(equalityExpressionContext.equalityExpression()),
                Operator = (equalityExpressionContext.Equals() != null) ? "==" : "!=",
                Right = GenerateRelationalExpression(equalityExpressionContext.relationalExpression())
            };
        }

        public object GenerateRelationalExpression(CixParser.RelationalExpressionContext relationalExpression)
        {
            if (relationalExpression.shiftExpression() != null)
            {
                return GenerateShiftExpression(relationalExpression.shiftExpression());
            }

            string operatorSymbol;

            if (relationalExpression.LessThan() != null) { operatorSymbol = "<"; }
            if (relationalExpression.LessThanOrEqualTo() != null) { operatorSymbol = "<="; }
            if (relationalExpression.GreaterThan() != null) { operatorSymbol = ">"; }
            if (relationalExpression.GreaterThanOrEqualTo() != null) { operatorSymbol = ">="; }
            else { throw new ArgumentOutOfRangeException(); }

            return new
            {
                Left = GenerateRelationalExpression(relationalExpression.relationalExpression()),
                Operator = operatorSymbol,
                Right = GenerateShiftExpression(relationalExpression.shiftExpression())
            };
        }

        public object GenerateShiftExpression(CixParser.ShiftExpressionContext shiftExpression)
        {
            if (shiftExpression.shiftExpression() == null)
            {
                return GenerateAdditiveExpression(shiftExpression.additiveExpression());
            }

            return new
            {
                Left = GenerateShiftExpression(shiftExpression.shiftExpression()),
                Operator = (shiftExpression.ShiftLeft() != null) ? "<<" : ">>",
                Right = GenerateAdditiveExpression(shiftExpression.additiveExpression())
            };
        }

        public object GenerateAdditiveExpression(CixParser.AdditiveExpressionContext additiveExpression)
        {
            if (additiveExpression.additiveExpression() == null)
            {
                return GenerateMultiplicativeExpression(additiveExpression.multiplicativeExpression());
            }

            return new
            {
                Left = GenerateAdditiveExpression(additiveExpression.additiveExpression()),
                Operator = (additiveExpression.Plus() != null) ? "+" : "-",
                Right = GenerateMultiplicativeExpression(additiveExpression.multiplicativeExpression())
            };
        }

        public object GenerateMultiplicativeExpression(CixParser.MultiplicativeExpressionContext multiplicativeExpression)
        {
            if (multiplicativeExpression.multiplicativeExpression() == null)
            {
                return GenerateCastExpression(multiplicativeExpression.castExpression());
            }

            string operatorSymbol;

            if (multiplicativeExpression.Asterisk() != null) { operatorSymbol = "*"; }
            else if (multiplicativeExpression.Divide() != null) { operatorSymbol = "/"; }
            else if (multiplicativeExpression.Modulus() != null) { operatorSymbol = "%"; }
            else { throw new ArgumentOutOfRangeException(); }

            return new
            {
                Left = GenerateMultiplicativeExpression(multiplicativeExpression.multiplicativeExpression()),
                Operator = operatorSymbol,
                Right = GenerateCastExpression(multiplicativeExpression.castExpression())
            };
        }

        public object GenerateCastExpression(CixParser.CastExpressionContext castExpression)
        {
            if (castExpression.unaryExpression() != null)
            {
                return GenerateUnaryExpression(castExpression.unaryExpression());
            }

            return new
            {
                Type = GenerateTypeName(castExpression.typeName()),
                Expression = GenerateCastExpression(castExpression.castExpression())
            };
        }

        public object GenerateUnaryExpression(CixParser.UnaryExpressionContext unaryExpression)
        {
            if (unaryExpression.postfixExpression() != null)
            {
                return GeneratePostfixExpression(unaryExpression.postfixExpression());
            }
            else if (unaryExpression.Increment() != null || unaryExpression.Decrement() != null)
            {
                return new
                {
                    Operator = (unaryExpression.Increment() != null) ? "++" : "--",
                    Expression = GenerateUnaryExpression(unaryExpression.unaryExpression())
                };
            }
            else if (unaryExpression.unaryOperator() != null)
            {
                var operatorMappings = new Dictionary<int, string>
                {
                    { CixParser.Plus, "+" },
                    { CixParser.Minus, "-" },
                    { CixParser.BitwiseNot, "~" },
                    { CixParser.LogicalNot, "!" },
                    { CixParser.Ampersand, "&" },
                    { CixParser.Asterisk, "*" }
                };

                var operatorContext = unaryExpression.unaryOperator();

                var operatorTerminals = new ITerminalNode[]
                {
                    operatorContext.Plus(), operatorContext.Minus(), operatorContext.BitwiseNot(), operatorContext.LogicalNot(), operatorContext.Ampersand(), operatorContext.Asterisk()
                };
                var operatorTerminal = operatorTerminals.First(t => t != null);

                return new
                {
                    Operator = operatorMappings[operatorTerminal.Symbol.Type],
                    Expression = GenerateCastExpression(unaryExpression.castExpression())
                };
            }
            else
            {
                return unaryExpression.unaryExpression() != null
                    ? (object)new
                    {
                        Operator = "sizeof", Expression = GenerateUnaryExpression(unaryExpression.unaryExpression())
                    }
                    : new /* SizeOfExpression */ { Type = GenerateTypeName(unaryExpression.typeName()) };
            }
        }

        public object GeneratePostfixExpression(CixParser.PostfixExpressionContext postfixExpression)
        {
            if (postfixExpression.primaryExpression() != null)
            {
                return GeneratePrimaryExpression(postfixExpression.primaryExpression());
            }
            else if (postfixExpression.expression() != null)
            {
                return new /* ArrayAccess */
                {
                    Expression = GeneratePostfixExpression(postfixExpression.postfixExpression()),
                    Indexer = GenerateExpression(postfixExpression.expression())
                };
            }
            else if (postfixExpression.Increment() != null || postfixExpression.Decrement() != null)
            {
                return new
                {
                    Operator = (postfixExpression.Increment() != null) ? "++" : "--",
                    Expression = GeneratePostfixExpression(postfixExpression.postfixExpression())
                };
            }
            else if (postfixExpression.DirectMemberAccess() != null || postfixExpression.PointerMemberAccess() != null)
            {
                return new
                {
                    Left = GeneratePostfixExpression(postfixExpression.postfixExpression()),
                    Operator = (postfixExpression.DirectMemberAccess() != null) ? "." : "->",
                    Right = postfixExpression.Identifier().GetText()
                };
            }
            else
            {
                var arguments = (postfixExpression.argumentExpressionList() != null)
                    ? GenerateArgumentExpressionList(postfixExpression.argumentExpressionList())
                    : new List<object>();

                return new /* FunctionInvocation */
                {
                    Expression = GeneratePostfixExpression(postfixExpression.postfixExpression()),
                    Arguments = arguments
                };
            }
        }

        public object GenerateArgumentExpressionList(CixParser.ArgumentExpressionListContext argumentExpressionList)
        {
            if (argumentExpressionList.argumentExpressionList() == null)
            {
                return GenerateAssignmentExpression(argumentExpressionList.assignmentExpression());
            }

            var car = GenerateAssignmentExpression(argumentExpressionList.assignmentExpression());
            var cdr = GenerateArgumentExpressionList(argumentExpressionList.argumentExpressionList());

            return new { ArgumentCAR = car, ArgumentCDR = cdr };
        }

        public object GeneratePrimaryExpression(CixParser.PrimaryExpressionContext primaryExpression)
        {
            if (primaryExpression.Identifier() != null) { return primaryExpression.Identifier().GetText(); }
            else if (primaryExpression.StringLiteral() != null) { return primaryExpression.StringLiteral().GetText(); }
            else if (primaryExpression.number() != null) { return GenerateNumber(primaryExpression.number()); }
            else
            {
                return GenerateExpression(primaryExpression.expression());
            }
        }

        public object GenerateNumber(CixParser.NumberContext number)
        {
            if (number.Integer() != null) { return number.Integer().GetText(); }

            return number.FloatingPoint().GetText();
        }
    }
}
