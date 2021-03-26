using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions
{
    internal sealed class CastExpression : TypedExpression
    {
        public UsageTypeInfo ToType { get; set; }
        public TypedExpression Expression { get; set; }
    }
}