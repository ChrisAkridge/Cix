using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public sealed class CastExpression : Expression
    {
        public DataType ToType { get; set; }
        public Expression Operand { get; set; }
        public override string PrettyPrint() => $"({ToType.PrettyPrint(0)})({Operand})";
    }
}