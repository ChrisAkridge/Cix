using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public class CaseStatement : Statement
    {
        public Literal CaseLiteral { get; set; }
        public Statement Statement { get; set; }

        public override string PrettyPrint(int indentLevel)
        {
            var builder = new StringBuilder();
            var indent = new string(' ', indentLevel * 4);

            builder.AppendLine($"{indent}case {CaseLiteral?.PrettyPrint() ?? "default"}:");
            builder.AppendLine(Statement.PrettyPrint(indentLevel + 1));

            return builder.ToString();
        }
    }
}