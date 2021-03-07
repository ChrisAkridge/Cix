using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Antlr4.Runtime.Tree;
using Celarix.Cix.Compiler.Common.Models;
using Celarix.Cix.Compiler.Parse.AST;
using Celarix.Cix.Compiler.Parse.Models.AST.v1;
using NLog;

namespace Celarix.Cix.Compiler.Parse.ANTLR
{
    internal static class ASTGenerator
    {
        // TODO: Add line number to AST nodes (https://stackoverflow.com/a/19799925)
        // TODO: Throw ErrorFoundExceptions instead of ArgumentOutOfRangeExceptions

        private const int CurrentASTVersion = 1;

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        
        public static SourceFileRoot GenerateSourceFile(CixParser.SourceFileContext sourceFile)
        {
            logger.Trace("Converting ANTLR4 parse tree into Cix AST...");
            
            var structs = sourceFile.@struct().Select(GenerateStruct).ToList();

            var globalVariableDeclarations =
                sourceFile.globalVariableDeclaration().Select(GenerateGlobalVariableDeclaration).ToList();

            var functions = sourceFile.function().Select(GenerateFunction).ToList();

            return new SourceFileRoot
            {
                ASTVersion = CurrentASTVersion,
                Structs = structs,
                GlobalVariableDeclarations = globalVariableDeclarations,
                Functions = functions
            };
        }

        private static Struct GenerateStruct(CixParser.StructContext @struct)
        {
            var name = @struct.Identifier().GetText();
            var members = @struct.structMember().Select(GenerateStructMember).ToList();

            logger.Trace($"Found struct {name}");
            
            return new Struct
            {
                Name = name,
                Members = members
            };
        }

        private static StructMember GenerateStructMember(CixParser.StructMemberContext structMember)
        {
            var type = GenerateTypeName(structMember.typeName());
            var name = structMember.Identifier().GetText();

            var structArraySize = (structMember.structArraySize() != null)
                ? GenerateStructArraySize(structMember.structArraySize())
                : 1;

            return new StructMember
            {
                Type = type,
                Name = name,
                StructArraySize = structArraySize
            };
        }

        private static DataType GenerateTypeName(CixParser.TypeNameContext typeName)
        {
            var pointerLevel = 0;
            var pointerAsteriskList = typeName.pointerAsteriskList();
            if (pointerAsteriskList != null)
            {
                pointerLevel = pointerAsteriskList.Asterisk().Length;
            }
            
            if (typeName.funcptrTypeName() != null)
            {
                return new FuncptrDataType
                {
                    Types = GenerateTypeNameList(typeName.funcptrTypeName().typeNameList()),
                    PointerLevel = pointerLevel
                };
            }
            else
            {
                var typeIdentifierName = (typeName.primitiveType() != null)
                    ? typeName.primitiveType().GetText()
                    : typeName.Identifier().GetText();

                return new NamedDataType() { Name = typeIdentifierName, PointerLevel = pointerLevel };
            }
        }

        private static List<DataType> GenerateTypeNameList(CixParser.TypeNameListContext typeNameList) =>
            GenerateTypeNameListCdr(typeNameList).ToList();

        private static CarCdr<DataType> GenerateTypeNameListCdr(CixParser.TypeNameListContext typeNameList)
        {
            var typeListCAR = GenerateTypeName(typeNameList.typeName());
            CarCdr<DataType> typeListCDR = null;

            if (typeNameList.typeNameList() != null)
            {
                typeListCDR = GenerateTypeNameListCdr(typeNameList.typeNameList());
            }

            return new CarCdr<DataType>() { Car = typeListCAR, Cdr = typeListCDR };
        }

        private static int GenerateStructArraySize(CixParser.StructArraySizeContext structArraySize) =>
            (int)GenerateIntegerLiteral(structArraySize.Integer()).ValueBits;

        private static IntegerLiteral GenerateIntegerLiteral(ITerminalNode integer)
        {
            var integerText = integer.GetText();
            
            int suffixLength = 0;
            if (integerText.EndsWith("u", StringComparison.InvariantCultureIgnoreCase)
                || integerText.EndsWith("l", StringComparison.InvariantCultureIgnoreCase))
            {
                suffixLength = 1;
            }
            else if (integerText.EndsWith("ul", StringComparison.InvariantCultureIgnoreCase))
            {
                suffixLength = 2;
            }

            string integerPart = integerText.Substring(0, integerText.Length - suffixLength);
            string suffix = integerText.Substring(integerText.Length - suffixLength).ToLowerInvariant();

            return new IntegerLiteral
            {
                ValueBits = integerPart.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase)
                    ? ulong.Parse(integerPart.Substring(2), NumberStyles.AllowHexSpecifier)
                    : ulong.Parse(integerPart),
                LiteralType = (suffix == "u")
                    ? NumericLiteralType.UnsignedInteger
                    : (suffix == "l")
                        ? NumericLiteralType.Long
                        : (suffix == "ul")
                            ? NumericLiteralType.UnsignedLong
                            : NumericLiteralType.Integer
            };
        }

        private static GlobalVariableDeclaration GenerateGlobalVariableDeclaration(CixParser.GlobalVariableDeclarationContext globalVariableDeclaration)
        {
            if (globalVariableDeclaration.variableDeclarationStatement() != null)
            {
                var declaration =
                    GenerateVariableDeclarationStatement(globalVariableDeclaration.variableDeclarationStatement());

                logger.Trace($"Found global variable {declaration.Name}");
                
                return new GlobalVariableDeclaration
                {
                    Type = declaration.Type,
                    Name = declaration.Name
                };
            }
            else
            {
                var declaration = GenerateVariableDeclarationWithInitializationStatement(globalVariableDeclaration
                    .variableDeclarationWithInitializationStatement());

                logger.Trace($"Found global variable {declaration.Name}");

                return new GlobalVariableDeclarationWithInitialization
                {
                    Type = declaration.Type,
                    Name = declaration.Name,
                    Initializer = declaration.Initializer
                };
            }
        }

        private static Function GenerateFunction(CixParser.FunctionContext function)
        {
            var returnType = GenerateTypeName(function.typeName());
            var name = function.Identifier().GetText();

            var parameters = (function.functionParameterList() != null)
                ? GenerateFunctionParameterList(function.functionParameterList())
                : new List<FunctionParameter>();

            var statements = function.statement()?.Select(GenerateStatement).ToList() ?? new List<Statement>();

            logger.Trace($"Found function {name}");
            
            return new Function
            {
                ReturnType = returnType, 
                Name = name,
                Parameters = parameters,
                Statements = statements
            };
        }

        private static List<FunctionParameter> GenerateFunctionParameterList(CixParser.FunctionParameterListContext functionParameterList) =>
            GenerateFunctionParameterListCDR(functionParameterList).ToList();

        private static CarCdr<FunctionParameter> GenerateFunctionParameterListCDR(CixParser.FunctionParameterListContext functionParameterList)
        {
            if (functionParameterList == null) { return null; }
            var car = GenerateFunctionParameter(functionParameterList.functionParameter());
            var cdr = GenerateFunctionParameterListCDR(functionParameterList.functionParameterList());

            return new CarCdr<FunctionParameter> { Car = car, Cdr = cdr };
        }

        private static FunctionParameter GenerateFunctionParameter(CixParser.FunctionParameterContext functionParameter) =>
            new FunctionParameter
            {
                Type = GenerateTypeName(functionParameter.typeName()),
                Name = functionParameter.Identifier().GetText()
            };

        private static Statement GenerateStatement(CixParser.StatementContext statement)
        {
            if (statement.block() != null) { return GenerateBlock(statement.block()); }
            else if (statement.breakStatement() != null) { return new BreakStatement(); /* BreakStatement */ }
            else if (statement.conditionalStatement() != null) { return GenerateConditionalStatement(statement.conditionalStatement()); }
            else if (statement.continueStatement() != null) { return new ContinueStatement(); /* ContinueStatement */ }
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

        private static Block GenerateBlock(CixParser.BlockContext block) => new Block { Statements = block.statement().Select(GenerateStatement).ToList() };

        private static ConditionalStatement GenerateConditionalStatement(CixParser.ConditionalStatementContext conditionalStatement)
        {
            var expression = GenerateExpression(conditionalStatement.expression());
            var statement = GenerateStatement(conditionalStatement.statement());

            var elseStatement = (conditionalStatement.elseStatement() != null)
                ? GenerateStatement(conditionalStatement.elseStatement().statement())
                : null;

            return new ConditionalStatement
            {
                Condition = expression,
                IfTrue = statement,
                IfFalse = elseStatement
            };
        }

        private static DoWhileStatement GenerateDoWhileStatement(CixParser.DoWhileStatementContext doWhileStatement)
        {
            var statement = GenerateStatement(doWhileStatement.statement());
            var whileExpression = GenerateExpression(doWhileStatement.expression());

            return new DoWhileStatement { LoopStatement = statement, Condition = whileExpression };
        }

        private static ExpressionStatement GenerateExpressionStatement(CixParser.ExpressionStatementContext expressionStatement) =>
            new ExpressionStatement { Expression = GenerateExpression(expressionStatement.expression()) };

        private static ForStatement GenerateForStatement(CixParser.ForStatementContext forStatement) =>
            new ForStatement
            {
                Initializer = GenerateExpression(forStatement.expression(0)),
                Condition = GenerateExpression(forStatement.expression(1)),
                Iterator = GenerateExpression(forStatement.expression(2)),
                LoopStatement = GenerateStatement(forStatement.statement())
            };

        private static ReturnStatement GenerateReturnStatement(CixParser.ReturnStatementContext returnStatement) =>
            new ReturnStatement
            {
                ReturnValue = (returnStatement.expression() != null)
                    ? GenerateExpression(returnStatement.expression())
                    : null
            };

        private static SwitchStatement GenerateSwitchStatement(CixParser.SwitchStatementContext switchStatement) =>
            new SwitchStatement
            {
                Expression = GenerateExpression(switchStatement.expression()),
                Cases = switchStatement.caseStatement().Select(GenerateCaseStatement).ToList()
            };

        private static CaseStatement GenerateCaseStatement(CixParser.CaseStatementContext caseStatement)
        {
            if (caseStatement.literalCaseStatement() != null)
            {
                var literalCase = caseStatement.literalCaseStatement();

                var literal = (literalCase.Integer() != null)
                    ? GenerateIntegerLiteral(literalCase.Integer())
                    : (Literal)new StringLiteral { Value = literalCase.StringLiteral().GetText() };

                return new CaseStatement
                {
                    CaseLiteral = literal,
                    Statement = GenerateStatement(literalCase.statement())
                };
            }
            else
            {
                return new CaseStatement
                {
                    CaseLiteral = null,
                    Statement = GenerateStatement(caseStatement.defaultCaseStatement().statement())
                };
            }
        }

        private static WhileStatement GenerateWhileStatement(CixParser.WhileStatementContext whileStatement) =>
            new WhileStatement()
            {
                Condition = GenerateExpression(whileStatement.expression()),
                LoopStatement = GenerateStatement(whileStatement.statement())
            };

        private static VariableDeclaration GenerateVariableDeclarationStatement(CixParser.VariableDeclarationStatementContext variableDeclarationStatement) =>
            new VariableDeclaration
            {
                Type = GenerateTypeName(variableDeclarationStatement.typeName()),
                Name = variableDeclarationStatement.Identifier().GetText()
            };

        private static VariableDeclarationWithInitialization GenerateVariableDeclarationWithInitializationStatement(CixParser.VariableDeclarationWithInitializationStatementContext variableDeclarationWithInitializationStatementContext) =>
            new VariableDeclarationWithInitialization
            {
                Type = GenerateTypeName(variableDeclarationWithInitializationStatementContext.typeName()),
                Name = variableDeclarationWithInitializationStatementContext.Identifier().GetText(),
                Initializer = GenerateExpression(variableDeclarationWithInitializationStatementContext.expression())
            };

        private static Expression GenerateExpression(CixParser.ExpressionContext expression) => GenerateAssignmentExpression(expression.assignmentExpression());

        private static Expression GenerateAssignmentExpression(CixParser.AssignmentExpressionContext assignmentExpression)
        {
            if (assignmentExpression.conditionalExpression() != null)
            {
                return GenerateConditionalExpression(assignmentExpression.conditionalExpression());
            }

            return new BinaryExpression
            {
                Left = GenerateUnaryExpression(assignmentExpression.unaryExpression()),
                Operator = assignmentExpression.assignmentOperator().GetText(),
                Right = GenerateAssignmentExpression(assignmentExpression.assignmentExpression())
            };
        }

        private static Expression GenerateConditionalExpression(CixParser.ConditionalExpressionContext conditionalExpression) =>
            conditionalExpression.expression() == null
                ? GenerateLogicalOrExpression(conditionalExpression.logicalOrExpression())
                : new TernaryExpression
                {
                    Operand1 = GenerateLogicalOrExpression(conditionalExpression.logicalOrExpression()),
                    Operator1 = "?",
                    Operand2 = GenerateExpression(conditionalExpression.expression()),
                    Operator2 = ":",
                    Operand3 = GenerateConditionalExpression(conditionalExpression.conditionalExpression())
                };

        private static Expression GenerateLogicalOrExpression(CixParser.LogicalOrExpressionContext logicalOrExpression) =>
            logicalOrExpression.logicalOrExpression() == null
                ? GenerateLogicalAndExpression(logicalOrExpression.logicalAndExpression())
                : new BinaryExpression
                {
                    Left = GenerateLogicalOrExpression(logicalOrExpression.logicalOrExpression()),
                    Operator = "||",
                    Right = GenerateLogicalAndExpression(logicalOrExpression.logicalAndExpression())
                };

        private static Expression GenerateLogicalAndExpression(CixParser.LogicalAndExpressionContext logicalAndExpression) =>
            logicalAndExpression.logicalAndExpression() == null
                ? GenerateInclusiveOrExpression(logicalAndExpression.inclusiveOrExpression())
                : new BinaryExpression
                {
                    Left = GenerateLogicalAndExpression(logicalAndExpression.logicalAndExpression()),
                    Operator = "&&",
                    Right = GenerateInclusiveOrExpression(logicalAndExpression.inclusiveOrExpression())
                };

        private static Expression GenerateInclusiveOrExpression(CixParser.InclusiveOrExpressionContext inclusiveOrExpression) =>
            inclusiveOrExpression.inclusiveOrExpression() == null
                ? GenerateExclusiveOrExpression(inclusiveOrExpression.exclusiveOrExpression())
                : new BinaryExpression
                {
                    Left = GenerateInclusiveOrExpression(inclusiveOrExpression.inclusiveOrExpression()),
                    Operator = "|",
                    Right = GenerateExclusiveOrExpression(inclusiveOrExpression.exclusiveOrExpression())
                };

        private static Expression GenerateExclusiveOrExpression(CixParser.ExclusiveOrExpressionContext exclusiveOrExpression) =>
            exclusiveOrExpression.exclusiveOrExpression() == null
                ? GenerateAndExpression(exclusiveOrExpression.andExpression())
                : new BinaryExpression()
                {
                    Left = GenerateExclusiveOrExpression(exclusiveOrExpression.exclusiveOrExpression()),
                    Operator = "^",
                    Right = GenerateAndExpression(exclusiveOrExpression.andExpression())
                };

        private static Expression GenerateAndExpression(CixParser.AndExpressionContext andExpression) =>
            (andExpression.andExpression() == null)
                ? GenerateEqualityExpression(andExpression.equalityExpression())
                : new BinaryExpression
                {
                    Left = GenerateAndExpression(andExpression.andExpression()),
                    Operator = "&",
                    Right = GenerateEqualityExpression(andExpression.equalityExpression())
                };

        private static Expression GenerateEqualityExpression(CixParser.EqualityExpressionContext equalityExpressionContext)
        {
            if (equalityExpressionContext.equalityExpression() == null)
            {
                return GenerateRelationalExpression(equalityExpressionContext.relationalExpression());
            }

            return new BinaryExpression
            {
                Left = GenerateEqualityExpression(equalityExpressionContext.equalityExpression()),
                Operator = (equalityExpressionContext.Equals() != null) ? "==" : "!=",
                Right = GenerateRelationalExpression(equalityExpressionContext.relationalExpression())
            };
        }

        private static Expression GenerateRelationalExpression(CixParser.RelationalExpressionContext relationalExpression)
        {
            if (relationalExpression.relationalExpression() == null)
            {
                return GenerateShiftExpression(relationalExpression.shiftExpression());
            }

            string operatorSymbol;

            if (relationalExpression.LessThan() != null) { operatorSymbol = "<"; }
            else if (relationalExpression.LessThanOrEqualTo() != null) { operatorSymbol = "<="; }
            else if (relationalExpression.GreaterThan() != null) { operatorSymbol = ">"; }
            else if (relationalExpression.GreaterThanOrEqualTo() != null) { operatorSymbol = ">="; }
            else { throw new ArgumentOutOfRangeException(); }

            return new BinaryExpression
            {
                Left = GenerateRelationalExpression(relationalExpression.relationalExpression()),
                Operator = operatorSymbol,
                Right = GenerateShiftExpression(relationalExpression.shiftExpression())
            };
        }

        private static Expression GenerateShiftExpression(CixParser.ShiftExpressionContext shiftExpression)
        {
            if (shiftExpression.shiftExpression() == null)
            {
                return GenerateAdditiveExpression(shiftExpression.additiveExpression());
            }

            return new BinaryExpression
            {
                Left = GenerateShiftExpression(shiftExpression.shiftExpression()),
                Operator = (shiftExpression.ShiftLeft() != null) ? "<<" : ">>",
                Right = GenerateAdditiveExpression(shiftExpression.additiveExpression())
            };
        }

        private static Expression GenerateAdditiveExpression(CixParser.AdditiveExpressionContext additiveExpression)
        {
            if (additiveExpression.additiveExpression() == null)
            {
                return GenerateMultiplicativeExpression(additiveExpression.multiplicativeExpression());
            }

            return new BinaryExpression
            {
                Left = GenerateAdditiveExpression(additiveExpression.additiveExpression()),
                Operator = (additiveExpression.Plus() != null) ? "+" : "-",
                Right = GenerateMultiplicativeExpression(additiveExpression.multiplicativeExpression())
            };
        }

        private static Expression GenerateMultiplicativeExpression(CixParser.MultiplicativeExpressionContext multiplicativeExpression)
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

            return new BinaryExpression
            {
                Left = GenerateMultiplicativeExpression(multiplicativeExpression.multiplicativeExpression()),
                Operator = operatorSymbol,
                Right = GenerateCastExpression(multiplicativeExpression.castExpression())
            };
        }

        private static Expression GenerateCastExpression(CixParser.CastExpressionContext castExpression)
        {
            if (castExpression.unaryExpression() != null)
            {
                return GenerateUnaryExpression(castExpression.unaryExpression());
            }

            return new CastExpression
            {
                ToType = GenerateTypeName(castExpression.typeName()),
                Operand = GenerateCastExpression(castExpression.castExpression())
            };
        }

        private static Expression GenerateUnaryExpression(CixParser.UnaryExpressionContext unaryExpression)
        {
            if (unaryExpression.postfixExpression() != null)
            {
                return GeneratePostfixExpression(unaryExpression.postfixExpression());
            }
            else if (unaryExpression.Increment() != null || unaryExpression.Decrement() != null)
            {
                return new UnaryExpression
                {
                    Operator = (unaryExpression.Increment() != null) ? "++" : "--",
                    Operand = GenerateUnaryExpression(unaryExpression.unaryExpression()),
                    IsPostfix = false
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

                var operatorTerminals = new[]
                {
                    operatorContext.Plus(),
                    operatorContext.Minus(),
                    operatorContext.BitwiseNot(),
                    operatorContext.LogicalNot(),
                    operatorContext.Ampersand(),
                    operatorContext.Asterisk()
                };
                var operatorTerminal = operatorTerminals.First(t => t != null);

                return new UnaryExpression
                {
                    Operator = operatorMappings[operatorTerminal.Symbol.Type],
                    Operand = GenerateCastExpression(unaryExpression.castExpression()),
                    IsPostfix = false
                };
            }
            else if (unaryExpression.unaryExpression() != null)
            {
                return new UnaryExpression
                {
                    Operator = "sizeof", Operand = GenerateUnaryExpression(unaryExpression.unaryExpression())
                };
            }
            
            return new SizeOfExpression { Type = GenerateTypeName(unaryExpression.typeName()) };
        }

        private static Expression GeneratePostfixExpression(CixParser.PostfixExpressionContext postfixExpression)
        {
            if (postfixExpression.primaryExpression() != null)
            {
                return GeneratePrimaryExpression(postfixExpression.primaryExpression());
            }
            else if (postfixExpression.expression() != null)
            {
                return new ArrayAccess
                {
                    Operand = GeneratePostfixExpression(postfixExpression.postfixExpression()),
                    Index = GenerateExpression(postfixExpression.expression())
                };
            }
            else if (postfixExpression.Increment() != null || postfixExpression.Decrement() != null)
            {
                return new UnaryExpression
                {
                    Operator = (postfixExpression.Increment() != null) ? "++" : "--",
                    Operand = GeneratePostfixExpression(postfixExpression.postfixExpression()),
                    IsPostfix = true
                };
            }
            else if (postfixExpression.DirectMemberAccess() != null || postfixExpression.PointerMemberAccess() != null)
            {
                return new BinaryExpression
                {
                    Left = GeneratePostfixExpression(postfixExpression.postfixExpression()),
                    Operator = (postfixExpression.DirectMemberAccess() != null) ? "." : "->",
                    Right = new Identifier { IdentifierText = postfixExpression.Identifier().GetText() } 
                };
            }
            else
            {
                var arguments = (postfixExpression.argumentExpressionList() != null)
                    ? GenerateArgumentExpressionList(postfixExpression.argumentExpressionList())
                    : new List<Expression>();

                return new FunctionInvocation
                {
                    Operand = GeneratePostfixExpression(postfixExpression.postfixExpression()),
                    Arguments = arguments
                };
            }
        }

        private static List<Expression> GenerateArgumentExpressionList(CixParser.ArgumentExpressionListContext argumentExpressionList) =>
            GenerateArgumentExpressionListCdr(argumentExpressionList).Reverse().ToList();

        private static CarCdr<Expression> GenerateArgumentExpressionListCdr(CixParser.ArgumentExpressionListContext argumentExpressionList)
        {
            if (argumentExpressionList == null) { return null; }
            
            var car = GenerateAssignmentExpression(argumentExpressionList.assignmentExpression());
            var cdr = GenerateArgumentExpressionListCdr(argumentExpressionList.argumentExpressionList());

            return new CarCdr<Expression> { Car = car, Cdr = cdr };
        }

        private static Expression GeneratePrimaryExpression(CixParser.PrimaryExpressionContext primaryExpression) =>
            primaryExpression.Identifier() != null
                ? new Identifier { IdentifierText = primaryExpression.Identifier().GetText() }
                : primaryExpression.StringLiteral() != null
                    ? new StringLiteral { Value = primaryExpression.StringLiteral().GetText() }
                    : primaryExpression.number() != null
                        ? GenerateNumber(primaryExpression.number())
                        : GenerateExpression(primaryExpression.expression());

        private static Literal GenerateNumber(CixParser.NumberContext number) =>
            number.Integer() != null
                ? (Literal)GenerateIntegerLiteral(number.Integer())
                : GenerateFloatingPointLiteral(number.FloatingPoint());

        private static FloatingPointLiteral GenerateFloatingPointLiteral(ITerminalNode floatingPoint)
        {
            var literalType = NumericLiteralType.Single;
            var literalText = floatingPoint.GetText();

            if (literalText.EndsWith("d", StringComparison.InvariantCultureIgnoreCase))
            {
                literalType = NumericLiteralType.Double;
            }

            return new FloatingPointLiteral
            {
                ValueBits = (literalType == NumericLiteralType.Double
                    || literalText.EndsWith("f", StringComparison.InvariantCultureIgnoreCase))
                    ? double.Parse(literalText[0..^1], NumberStyles.Float)
                    : double.Parse(literalText, NumberStyles.Float),
                NumericLiteralType = literalType
            };
        }
    }
}
