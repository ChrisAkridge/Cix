using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public sealed class SwitchStatement : Statement
    {
        public Expression Expression { get; set; }
        public List<CaseStatement> Cases { get; set; }

        public override string PrettyPrint(int indentLevel)
        {
            var builder = new StringBuilder();
            var indent = new string(' ', indentLevel * 4);

            builder.AppendLine($"{indent}switch ({Expression.PrettyPrint()})");
            builder.AppendLine($"{indent}{{");

            foreach (var @case in Cases)
            {
                builder.AppendLine(@case.PrettyPrint(indentLevel + 1));
            }

            builder.AppendLine($"{indent}}}");

            return builder.ToString();
        }
    }
}