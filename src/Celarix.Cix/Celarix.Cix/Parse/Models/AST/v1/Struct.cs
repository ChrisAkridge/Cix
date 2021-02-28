using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public sealed class Struct : ASTNode
    {
        public string Name { get; set; }
        public List<StructMember> Members { get; set; }

        public override string PrettyPrint(int indentLevel)
        {
            var builder = new StringBuilder();
            var indent = new string(' ', indentLevel * 4);

            builder.AppendLine($"{indent}struct {Name}");
            builder.AppendLine($"{indent}{{");

            foreach (var member in Members)
            {
                builder.AppendLine(member.PrettyPrint(indentLevel + 1));
            }

            builder.AppendLine($"{indent}}}");

            return builder.ToString();
        }
    }
}
