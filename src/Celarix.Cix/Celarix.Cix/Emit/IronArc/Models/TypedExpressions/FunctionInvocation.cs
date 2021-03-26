using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions
{
    internal sealed class FunctionInvocation : TypedExpression
    {
        public TypedExpression Operand { get; set; }
        public List<TypedExpression> Arguments { get; set; }
    }
}