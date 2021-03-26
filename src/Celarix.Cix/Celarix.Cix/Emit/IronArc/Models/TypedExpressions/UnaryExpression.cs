using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions
{
    internal sealed class UnaryExpression : TypedExpression
    {
        public TypedExpression Operand { get; set; }
        public string Operator { get; set; }
        public bool IsPostfix { get; set; }
    }
}