using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions
{
    internal sealed class BinaryExpression : TypedExpression
    {
        public TypedExpression Left { get; set; }
        public TypedExpression Right { get; set; }
        public string Operator { get; set; }
    }
}