using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public sealed class IntegerLiteral : Literal
    {
        public ulong ValueBits { get; set; }
        public NumericLiteralType LiteralType { get; set; }
        public override string PrettyPrint() => ValueBits.ToString();
    }
}