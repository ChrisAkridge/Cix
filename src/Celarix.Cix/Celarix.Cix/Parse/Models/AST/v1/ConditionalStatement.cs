using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public sealed class ConditionalStatement : Statement
    {
        public Expression Condition { get; set; }
        public Statement IfTrue { get; set; }
        public Statement IfFalse { get; set; }

        public override string PrettyPrint(int indentLevel)
        {
            var builder = new StringBuilder();
            var indent = new string(' ', indentLevel * 4);

            builder.AppendLine($"{indent}if ({Condition.PrettyPrint()})");
            builder.AppendLine(IfTrue.PrettyPrint(indentLevel + 1));

            if (IfFalse != null)
            {
                builder.AppendLine($"{indent}else");
                builder.AppendLine(IfFalse.PrettyPrint(indentLevel + 1));
            }

            return builder.ToString();
        }
    }
}