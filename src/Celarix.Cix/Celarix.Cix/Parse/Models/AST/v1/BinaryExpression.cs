using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public sealed class BinaryExpression : Expression
    {
        public Expression Left { get; set; }
        public string Operator { get; set; }
        public Expression Right { get; set; }

        public override string PrettyPrint() =>
            Operator != "." && Operator != "->"
                ? $"{Left.PrettyPrint()} {Operator} {Right.PrettyPrint()}"
                : $"{Left.PrettyPrint()}{Operator}{Right.PrettyPrint()}";
    }
}