using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public sealed class ForStatement : Statement
    {
        public Expression Initializer { get; set; }
        public Expression Condition { get; set; }
        public Expression Iterator { get; set; }
        public Statement LoopStatement { get; set; }

        public override string PrettyPrint(int indentLevel)
        {
            var builder = new StringBuilder();
            var indent = new string(' ', indentLevel * 4);

            builder.AppendLine(
                $"{indent}for ({Initializer.PrettyPrint()}; {Condition.PrettyPrint()}; {Iterator.PrettyPrint()})");
            builder.AppendLine(LoopStatement.PrettyPrint(indentLevel + 1));

            return builder.ToString();
        }
    }
}