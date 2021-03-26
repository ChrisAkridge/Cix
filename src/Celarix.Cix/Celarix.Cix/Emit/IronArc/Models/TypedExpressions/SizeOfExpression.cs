using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions
{
    internal sealed class SizeOfExpression : TypedExpression
    {
        public UsageTypeInfo Type { get; set; }
    }
}