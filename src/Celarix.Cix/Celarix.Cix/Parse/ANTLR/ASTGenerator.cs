using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Tree;
using Celarix.Cix.Compiler.Common.Models;
using Celarix.Cix.Compiler.Parse.AST;
using Celarix.Cix.Compiler.Parse.Models.AST;
using Celarix.Cix.Compiler.Parse.Models.AST.v1;

namespace Celarix.Cix.Compiler.Parse.ANTLR
{
    public sealed class ASTGenerator
    {
        // TODO: Make everything here static
        // TODO: Add line number to AST nodes (https://stackoverflow.com/a/19799925)

        private const int CurrentASTVersion = 1;
        
        public SourceFileRoot GenerateSourceFile(CixParser.SourceFileContext sourceFile)
        {
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

        public Struct GenerateStruct(CixParser.StructContext @struct)
        {
            var name = @struct.Identifier().GetText();
            var members = @struct.structMember().Select(GenerateStructMember).ToList();

            return new Struct
            {
                Name = name,
                Members = members
            };
        }

        public StructMember GenerateStructMember(CixParser.StructMemberContext structMember)
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

        public DataType GenerateTypeName(CixParser.TypeNameContext typeName)
        {
            var pointerLevel = 0;
            var pointerAsteriskList = typeName.pointerAsteriskList();
            if (pointerAsteriskList != null)
            {
                pointerLevel = pointerAsteriskList.Asterisk().Length;
            }
            
            if (typeName.funcptrTypeName() != null)
            {
                return new FuncptrDataType { Types = GenerateTypeNameList(typeName.funcptrTypeName().typeNameList()), PointerLevel = pointerLevel };
            }
            else
            {
                var typeIdentifierName = (typeName.primitiveType() != null)
                    ? typeName.primitiveType().GetText()
                    : typeName.Identifier().GetText();

                return new NamedDataType() { Name = typeIdentifierName, PointerLevel = pointerLevel };
            }
        }

        public List<DataType> GenerateTypeNameList(CixParser.TypeNameListContext typeNameList)
        {
            var carCdr = GenerateTypeNameListCdr(typeNameList);

            return carCdr.ToList();
        }

        public CarCdr<DataType> GenerateTypeNameListCdr(CixParser.TypeNameListContext typeNameList)
        {
            var typeListCAR = GenerateTypeName(typeNameList.typeName());
            CarCdr<DataType> typeListCDR = null;

            if (typeNameList.typeNameList() != null)
            {
                typeListCDR = GenerateTypeNameListCdr(typeNameList.typeNameList());
            }

            return new CarCdr<DataType>() { Car = typeListCAR, Cdr = typeListCDR };
        }

        public int GenerateStructArraySize(CixParser.StructArraySizeContext structArraySize)
        {
            return (int)ParseInteger(structArraySize.Integer().GetText());
        }

        public ulong ParseInteger(string integerText)
        {
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

            if (integerPart.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase))
            {
                return ulong.Parse(integerPart.Substring(2), NumberStyles.AllowHexSpecifier);
            }

            return ulong.Parse(integerPart);
        }

        public GlobalVariableDeclaration GenerateGlobalVariableDeclaration(CixParser.GlobalVariableDeclarationContext globalVariableDeclaration)
        {
            if (globalVariableDeclaration.variableDeclarationStatement() != null)
            {
                var declaration =
                    GenerateVariableDeclarationStatement(globalVariableDeclaration.variableDeclarationStatement());

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
                return new GlobalVariableDeclarationWithInitialization
                {
                    Type = declaration.Type,
                    Name = declaration.Name,
                    Initializer = declaration.Initializer
                };
            }
        }

        public Function GenerateFunction(CixParser.FunctionContext function)
        {
            var returnType = GenerateTypeName(function.typeName());
            var name = function.Identifier().GetText();

            var parameters = (function.functionParameterList() != null)
                ? GenerateFunctionParameterList(function.functionParameterList())
                : new List<FunctionParameter>();

            var statements = function?.statement().Select(GenerateStatement).ToList() ?? new List<Statement>();

            return new Function
            {
                ReturnType = returnType, 
                Name = name,
                Parameters = parameters,
                Statements = statements
            };
        }

        public List<FunctionParameter> GenerateFunctionParameterList(CixParser.FunctionParameterListContext functionParameterList)
        {
            return GenerateFunctionParameterListCDR(functionParameterList).ToList();
        }

        private CarCdr<FunctionParameter> GenerateFunctionParameterListCDR(CixParser.FunctionParameterListContext functionParameterList)
        {
            if (functionParameterList == null) { return null; }
            var car = GenerateFunctionParameter(functionParameterList.functionParameter());
            var cdr = GenerateFunctionParameterListCDR(functionParameterList.functionParameterList());

            return new CarCdr<FunctionParameter> { Car = car, Cdr = cdr };
        }

        public FunctionParameter GenerateFunctionParameter(CixParser.FunctionParameterContext functionParameter)
        {
            return new FunctionParameter
            {
                Type = GenerateTypeName(functionParameter.typeName()),
                Name = functionParameter.Identifier().GetText()
            };
        }

        public Statement GenerateStatement(CixParser.StatementContext statement)
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

        public Block GenerateBlock(CixParser.BlockContext block)
        {
            return new Block { Statements = block.statement().Select(GenerateStatement).ToList() };
        }

        public ConditionalStatement GenerateConditionalStatement(CixParser.ConditionalStatementContext conditionalStatement)
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

        public DoWhileStatement GenerateDoWhileStatement(CixParser.DoWhileStatementContext doWhileStatement)
        {
            var statement = GenerateStatement(doWhileStatement.statement());
            var whileExpression = GenerateExpression(doWhileStatement.expression());

            return new DoWhileStatement { LoopStatement = statement, Condition = whileExpression };
        }

        public ExpressionStatement GenerateExpressionStatement(CixParser.ExpressionStatementContext expressionStatement) =>
            new ExpressionStatement { Expression = GenerateExpression(expressionStatement.expression()) };

        public ForStatement GenerateForStatement(CixParser.ForStatementContext forStatement)
        {
            return new ForStatement
            {
                Initializer = GenerateExpression(forStatement.expression(0)),
                Condition = GenerateExpression(forStatement.expression(1)),
                Iterator = GenerateExpression(forStatement.expression(2)),
                LoopStatement = GenerateStatement(forStatement.statement())
            };
        }

        public ReturnStatement GenerateReturnStatement(CixParser.ReturnStatementContext returnStatement)
        {
            return new ReturnStatement
            {
                ReturnValue = (returnStatement.expression() != null)
                    ? GenerateExpression(returnStatement.expression())
                    : null
            };
        }

        public SwitchStatement GenerateSwitchStatement(CixParser.SwitchStatementContext switchStatement)
        {
            return new SwitchStatement
            {
                Expression = GenerateExpression(switchStatement.expression()),
                Cases = switchStatement.caseStatement().Select(GenerateCaseStatement).ToList()
            };
        }

        public CaseStatement GenerateCaseStatement(CixParser.CaseStatementContext caseStatement)
        {
            if (caseStatement.literalCaseStatement() != null)
            {
                var literalCase = caseStatement.literalCaseStatement();

                Literal literal = (literalCase.Integer() != null)
                    ? (Literal)(new IntegerLiteral { ValueBits = ParseInteger(literalCase.Integer().GetText()) })
                    : (Literal)(new StringLiteral { Value = literalCase.StringLiteral().GetText() });

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
                    Statement = GenerateStatement(caseStatement.literalCaseStatement().statement())
                };
            }
        }

        public WhileStatement GenerateWhileStatement(CixParser.WhileStatementContext whileStatement)
        {
            return new WhileStatement()
            {
                Condition = GenerateExpression(whileStatement.expression()),
                LoopStatement = GenerateStatement(whileStatement.statement())
            };
        }

        public VariableDeclaration GenerateVariableDeclarationStatement(CixParser.VariableDeclarationStatementContext variableDeclarationStatement)
        {
            return new VariableDeclaration
            {
                Type = GenerateTypeName(variableDeclarationStatement.typeName()),
                Name = variableDeclarationStatement.Identifier().GetText()
            };
        }

        public VariableDeclarationWithInitialization GenerateVariableDeclarationWithInitializationStatement(CixParser.VariableDeclarationWithInitializationStatementContext variableDeclarationWithInitializationStatementContext)
        {
            return new VariableDeclarationWithInitialization
            {
                Type = GenerateTypeName(variableDeclarationWithInitializationStatementContext.typeName()),
                Name = variableDeclarationWithInitializationStatementContext.Identifier().GetText(),
                Initializer = GenerateExpression(variableDeclarationWithInitializationStatementContext.expression())
            };
        }

        public Expression GenerateExpression(CixParser.ExpressionContext expression)
        {
            return GenerateAssignmentExpression(expression.assignmentExpression());
        }

        public Expression GenerateAssignmentExpression(CixParser.AssignmentExpressionContext assignmentExpression)
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

        public Expression GenerateConditionalExpression(CixParser.ConditionalExpressionContext conditionalExpression)
        {
            return conditionalExpression.expression() == null
                ? GenerateLogicalOrExpression(conditionalExpression.logicalOrExpression())
                : new TernaryExpression
                {
                    Operand1 = GenerateLogicalOrExpression(conditionalExpression.logicalOrExpression()),
                    Operator1 = "?",
                    Operand2 = GenerateExpression(conditionalExpression.expression()),
                    Operator2 = ":",
                    Operand3 = GenerateConditionalExpression(conditionalExpression.conditionalExpression())
                };
        }

        public Expression GenerateLogicalOrExpression(CixParser.LogicalOrExpressionContext logicalOrExpression)
        {
            return logicalOrExpression.logicalOrExpression() == null
                ? GenerateLogicalAndExpression(logicalOrExpression.logicalAndExpression())
                : new BinaryExpression
                {
                    Left = GenerateLogicalOrExpression(logicalOrExpression.logicalOrExpression()),
                    Operator = "||",
                    Right = GenerateLogicalAndExpression(logicalOrExpression.logicalAndExpression())
                };
        }

        public Expression GenerateLogicalAndExpression(CixParser.LogicalAndExpressionContext logicalAndExpression)
        {
            return logicalAndExpression.logicalAndExpression() == null
                ? GenerateInclusiveOrExpression(logicalAndExpression.inclusiveOrExpression())
                : new BinaryExpression
                {
                    Left = GenerateLogicalAndExpression(logicalAndExpression.logicalAndExpression()),
                    Operator = "&&",
                    Right = GenerateInclusiveOrExpression(logicalAndExpression.inclusiveOrExpression())
                };
        }

        public Expression GenerateInclusiveOrExpression(CixParser.InclusiveOrExpressionContext inclusiveOrExpression)
        {
            return inclusiveOrExpression.inclusiveOrExpression() == null
                ? GenerateExclusiveOrExpression(inclusiveOrExpression.exclusiveOrExpression())
                : new BinaryExpression
                {
                    Left = GenerateInclusiveOrExpression(inclusiveOrExpression.inclusiveOrExpression()),
                    Operator = "|",
                    Right = GenerateExclusiveOrExpression(inclusiveOrExpression.exclusiveOrExpression())
                };
        }

        public Expression GenerateExclusiveOrExpression(CixParser.ExclusiveOrExpressionContext exclusiveOrExpression)
        {
            return exclusiveOrExpression.exclusiveOrExpression() == null
                ? GenerateAndExpression(exclusiveOrExpression.andExpression())
                : new BinaryExpression()
                {
                    Left = GenerateExclusiveOrExpression(exclusiveOrExpression.exclusiveOrExpression()),
                    Operator = "^",
                    Right = GenerateAndExpression(exclusiveOrExpression.andExpression())
                };
        }

        public Expression GenerateAndExpression(CixParser.AndExpressionContext andExpression)
        {
            return (andExpression.andExpression() == null)
                ? GenerateEqualityExpression(andExpression.equalityExpression())
                : new BinaryExpression
                {
                    Left = GenerateAndExpression(andExpression.andExpression()),
                    Operator = "&",
                    Right = GenerateEqualityExpression(andExpression.equalityExpression())
                };
        }

        public Expression GenerateEqualityExpression(CixParser.EqualityExpressionContext equalityExpressionContext)
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

        public Expression GenerateRelationalExpression(CixParser.RelationalExpressionContext relationalExpression)
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

            return new BinaryExpression
            {
                Left = GenerateRelationalExpression(relationalExpression.relationalExpression()),
                Operator = operatorSymbol,
                Right = GenerateShiftExpression(relationalExpression.shiftExpression())
            };
        }

        public Expression GenerateShiftExpression(CixParser.ShiftExpressionContext shiftExpression)
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

        public Expression GenerateAdditiveExpression(CixParser.AdditiveExpressionContext additiveExpression)
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

        public Expression GenerateMultiplicativeExpression(CixParser.MultiplicativeExpressionContext multiplicativeExpression)
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

        public Expression GenerateCastExpression(CixParser.CastExpressionContext castExpression)
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

        public Expression GenerateUnaryExpression(CixParser.UnaryExpressionContext unaryExpression)
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

                var operatorTerminals = new ITerminalNode[]
                {
                    operatorContext.Plus(), operatorContext.Minus(), operatorContext.BitwiseNot(), operatorContext.LogicalNot(), operatorContext.Ampersand(), operatorContext.Asterisk()
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

        public Expression GeneratePostfixExpression(CixParser.PostfixExpressionContext postfixExpression)
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

        public List<Expression> GenerateArgumentExpressionList(CixParser.ArgumentExpressionListContext argumentExpressionList)
        {
            return GenerateArgumentExpressionListCdr(argumentExpressionList).ToList();
        }

        public CarCdr<Expression> GenerateArgumentExpressionListCdr(CixParser.ArgumentExpressionListContext argumentExpressionList)
        {
            if (argumentExpressionList == null) { return null; }
            
            var car = GenerateAssignmentExpression(argumentExpressionList.assignmentExpression());
            var cdr = GenerateArgumentExpressionListCdr(argumentExpressionList.argumentExpressionList());

            return new CarCdr<Expression> { Car = car, Cdr = cdr };
        }

        public Expression GeneratePrimaryExpression(CixParser.PrimaryExpressionContext primaryExpression)
        {
            if (primaryExpression.Identifier() != null)
            {
                return new Identifier { IdentifierText = primaryExpression.Identifier().GetText() };
            }
            else if (primaryExpression.StringLiteral() != null)
            {
                return new StringLiteral
                {
                    Value =
                        primaryExpression.StringLiteral().GetText()
                };
            }
            else
            {
                return primaryExpression.number() != null
                    ? (Expression)GenerateNumber(primaryExpression.number())
                    : GenerateExpression(primaryExpression.expression());
            }
        }

        public Literal GenerateNumber(CixParser.NumberContext number) =>
            number.Integer() != null
                ? new IntegerLiteral
                {
                    ValueBits = ParseInteger(number.Integer().GetText())
                }
                : (Literal)new FloatingPointLiteral
                {
                    ValueBits = double.Parse(GetFloatingPointNumberText(number.FloatingPoint().GetText()), NumberStyles.Float)
                };

        public string GetFloatingPointNumberText(string nodeText)
        {
            if (nodeText.EndsWith("f", StringComparison.InvariantCultureIgnoreCase)
                || nodeText.EndsWith("d", StringComparison.InvariantCultureIgnoreCase))
            {
                return nodeText[0..^1];
            }

            return nodeText;
        }
    }
}
