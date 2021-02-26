using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public sealed class TernaryExpression : Expression
    {
        public Expression Operand1 { get; set; }
        public string Operator1 { get; set; }
        public Expression Operand2 { get; set; }
        public string Operator2 { get; set; }
        public Expression Operand3 { get; set; }
        public override string PrettyPrint() => $"({Operand1.PrettyPrint()}) {Operator1} ({Operand2.PrettyPrint()}) {Operator2} ({Operand3.PrettyPrint()})";
    }
}