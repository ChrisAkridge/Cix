using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public sealed class DoWhileStatement : Statement
    {
        public Statement LoopStatement { get; set; }
        public Expression Condition { get; set; }

        public override string PrettyPrint(int indentLevel)
        {
            var builder = new StringBuilder();
            var indent = new string(' ', indentLevel * 4);

            builder.AppendLine($"{indent}do");
            builder.AppendLine(LoopStatement.PrettyPrint(indentLevel + 1));
            builder.AppendLine($"{indent}while ({Condition.PrettyPrint()});");

            return builder.ToString();
        }
    }
}