using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celarix.Cix.Compiler.Parse.Models.AST.v1;
using NLog;

namespace Celarix.Cix.Compiler.Lowering
{
    internal static class AsteriskDisambiguator
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        
        public static void Disambiguate(SourceFileRoot sourceFile)
        {
            logger.Trace("Disambiguating statements of the form \"x * y\"...");
            
            var definedTypes = new List<string>
            {
                "byte",
                "sbyte",
                "short",
                "ushort",
                "int",
                "uint",
                "long",
                "ulong",
                "float",
                "double",
                "void"
            }
            .Concat(sourceFile.Structs.Select(s => s.Name))
            .ToList();

            foreach (var function in sourceFile.Functions)
            {
                for (int i = 0; i < function.Statements.Count; i++)
                {
                    function.Statements[i] = DisambiguateStatement(function.Statements[i], definedTypes);
                }
            }
            
            logger.Trace("Statement disambiguation complete.");
        }

        private static Statement DisambiguateStatement(Statement statement, IList<string> definedTypes)
        {
            switch (statement)
            {
                case Block block:
                {
                    for (int i = 0; i < block.Statements.Count; i++)
                    {
                        block.Statements[i] = DisambiguateStatement(block.Statements[i], definedTypes);
                    }

                    break;
                }
                case CaseStatement caseStatement:
                    caseStatement.Statement = DisambiguateStatement(caseStatement.Statement, definedTypes);

                    break;
                case ConditionalStatement conditionalStatement:
                {
                    conditionalStatement.IfTrue = DisambiguateStatement(conditionalStatement.IfTrue, definedTypes);

                    if (conditionalStatement.IfFalse != null)
                    {
                        conditionalStatement.IfFalse = DisambiguateStatement(conditionalStatement.IfFalse, definedTypes);
                    }

                    break;
                }
                case DoWhileStatement doWhileStatement:
                    doWhileStatement.LoopStatement = DisambiguateStatement(doWhileStatement.LoopStatement, definedTypes);

                    break;
                case ExpressionStatement expressionStatement:
                {
                    if (expressionStatement.Expression is BinaryExpression binaryExpression
                        && binaryExpression.Operator == "*"
                        && binaryExpression.Left is Identifier left
                        && binaryExpression.Right is Identifier right
                        && definedTypes.Any(t => left.IdentifierText == t))
                    {
                        logger.Trace($"Found expression that was actually a variable declaration: {expressionStatement.PrettyPrint(0)}");
                        return new VariableDeclaration
                        {
                            Type = new NamedDataType { Name = left.IdentifierText, PointerLevel = 1 },
                            Name = right.IdentifierText
                        };
                    }

                    break;
                }
                case ForStatement forStatement:
                    forStatement.LoopStatement = DisambiguateStatement(forStatement.LoopStatement, definedTypes);

                    break;
                case VariableDeclaration variableDeclaration:
                {
                    if (variableDeclaration.Type is NamedDataType namedDataType
                        && variableDeclaration.Type.PointerLevel == 1
                        && definedTypes.All(t => namedDataType.Name != t))
                    {
                        logger.Trace($"Found variable declaration that was actually an expression: {variableDeclaration.PrettyPrint(0)}");
                        return new ExpressionStatement
                        {
                            Expression = new BinaryExpression
                            {
                                Left = new Identifier { IdentifierText = namedDataType.Name },
                                Operator = "*",
                                Right = new Identifier { IdentifierText = variableDeclaration.Name }
                            }
                        };
                    }

                    break;
                }
                case WhileStatement whileStatement:
                    whileStatement.LoopStatement = DisambiguateStatement(whileStatement.LoopStatement, definedTypes);

                    break;
            }

            return statement;
        }
    }
}
