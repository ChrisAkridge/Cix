using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public sealed class FloatingPointLiteral : Literal
    {
        public double ValueBits { get; set; }
        public NumericLiteralType NumericLiteralType { get; set; }

        public override string PrettyPrint() => ValueBits.ToString(CultureInfo.InvariantCulture);
    }
}