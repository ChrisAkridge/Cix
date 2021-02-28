using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public sealed class Function : ASTNode
    {
        public DataType ReturnType { get; set; }
        public string Name { get; set; }
        public List<FunctionParameter> Parameters { get; set; }
        public List<Statement> Statements { get; set; }

        public override string PrettyPrint(int indentLevel)
        {
            var builder = new StringBuilder();
            var indent = new string(' ', indentLevel * 4);
            var parameters = string.Join(", ", Parameters.Select(p => p.PrettyPrint(0)));

            builder.AppendLine($"{indent}{ReturnType.PrettyPrint(0)} {Name}({Parameters})");
            builder.AppendLine($"{indent}{{");

            foreach (var statement in Statements)
            {
                builder.AppendLine(statement.PrettyPrint(indentLevel + 1));
            }

            builder.AppendLine($"{indent}}}");

            return builder.ToString();
        }
    }
}