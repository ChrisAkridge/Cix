using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public sealed class Block : Statement
    {
        public List<Statement> Statements { get; set; }

        public override string PrettyPrint(int indentLevel)
        {
            // All the statements that are made of statements add 1 to their own
            // indent level to print the child statement. That looks like this:
            //
            // if (condition)
            //  return 0;
            //
            // But for a block, that would look like this:
            // if (condition)
            //  {
            //      return 0;
            //  }
            // 
            // Ew. So we'll just decrement the indent level here to make the brackets
            // align with the above statement.
            
            var builder = new StringBuilder();
            var indent = new string(' ', indentLevel * 4);

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