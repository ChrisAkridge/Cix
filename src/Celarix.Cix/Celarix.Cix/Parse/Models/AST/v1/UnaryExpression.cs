using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public sealed class UnaryExpression : Expression
    {
        public Expression Operand { get; set; }
        public string Operator { get; set; }
        public bool IsPostfix { get; set; }
        public override string PrettyPrint() => (IsPostfix)
            ? $"({Operand.PrettyPrint()}){Operator}"
            : $"{Operator}({Operand.PrettyPrint()})";
    }
}