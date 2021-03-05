using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celarix.Cix.Compiler.Parse.Models.AST.v1;
using NLog;

namespace Celarix.Cix.Compiler.Lowering
{
    internal static class Lowerings
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static void PerformLowerings(SourceFileRoot sourceFile)
        {
            logger.Trace("Performing lowerings...");

            RewriteForLoops(sourceFile);
        }

        private static void RewriteForLoops(SourceFileRoot sourceFile)
        {
            logger.Trace("Rewriting for loops as while loops...");

            foreach (var function in sourceFile.Functions)
            {
                for (int i = 0; i < function.Statements.Count; i++)
                {
                    function.Statements[i] = RewriteForLoopsInStatement(function.Statements[i]);
                }
            }
        }

        private static Statement RewriteForLoopsInStatement(Statement statement)
        {
            switch (statement)
            {
                case Block block:
                {
                    for (int i = 0; i < block.Statements.Count; i++)
                    {
                        block.Statements[i] = RewriteForLoopsInStatement(block.Statements[i]);
                    }

                    break;
                }
                case CaseStatement caseStatement:
                    caseStatement.Statement = RewriteForLoopsInStatement(caseStatement.Statement);

                    break;
                case ConditionalStatement conditionalStatement:
                {
                    conditionalStatement.IfTrue = RewriteForLoopsInStatement(conditionalStatement.IfTrue);

                    if (conditionalStatement.IfFalse != null)
                    {
                        conditionalStatement.IfFalse = RewriteForLoopsInStatement(conditionalStatement.IfFalse);
                    }

                    break;
                }
                case DoWhileStatement doWhileStatement:
                    doWhileStatement.LoopStatement = RewriteForLoopsInStatement(doWhileStatement.LoopStatement);

                    break;
                case ForStatement forStatement:
                    // for (i = 0; i < 10; i++) statement();
                    // becomes
                    // { i = 0; while (i < 10) { statement(); i++; } }
                    return new Block
                    {
                        Statements = new List<Statement>
                        {
                            new ExpressionStatement
                            {
                                Expression = forStatement.Initializer
                            },
                            new WhileStatement
                            {
                                Condition = forStatement.Condition,
                                LoopStatement = new Block
                                {
                                    Statements = new List<Statement>
                                    {
                                        forStatement.LoopStatement,
                                        new ExpressionStatement
                                        {
                                            Expression = forStatement.Iterator
                                        }
                                    }
                                }
                            }
                        } 
                    };
                case WhileStatement whileStatement:
                    whileStatement.LoopStatement = RewriteForLoopsInStatement(whileStatement.LoopStatement);

                    break;
            }

            return statement;
        }
    }
}
