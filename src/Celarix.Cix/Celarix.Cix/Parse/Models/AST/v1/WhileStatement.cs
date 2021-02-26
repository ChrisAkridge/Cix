using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public sealed class WhileStatement : Statement
    {
        public Expression Condition { get; set; }
        public Statement LoopStatement { get; set; }

        public override string PrettyPrint(int indentLevel)
        {
            var statementBuilder = new StringBuilder();
            var indent = new string(' ', indentLevel * 4);

            statementBuilder.AppendLine($"{indent}while ({Condition.PrettyPrint()})");
            statementBuilder.Append(LoopStatement.PrettyPrint(indentLevel + 1));

            return statementBuilder.ToString();
        }
    }
}