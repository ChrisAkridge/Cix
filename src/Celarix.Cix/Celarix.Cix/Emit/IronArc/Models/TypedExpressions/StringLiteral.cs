using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions
{
    internal sealed class StringLiteral : TypedExpression
    {
        public string LiteralValue { get; set; }
    }
}