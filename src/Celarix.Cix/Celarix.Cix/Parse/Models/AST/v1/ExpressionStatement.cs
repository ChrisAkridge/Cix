using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public sealed class ExpressionStatement : Statement
    {
        public Expression Expression { get; set; }
        public override string PrettyPrint(int indentLevel) => $"{new string(' ', indentLevel * 4)}{Expression.PrettyPrint()};";
    }
}