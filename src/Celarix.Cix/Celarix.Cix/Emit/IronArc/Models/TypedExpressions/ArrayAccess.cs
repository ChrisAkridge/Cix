using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions
{
    internal sealed class ArrayAccess : TypedExpression
    {
        public TypedExpression Operand { get; set; }
        public TypedExpression Index { get; set; }

        public override void ComputeType(TypeComputationContext context, TypedExpression parent)
        {
            // TArray* x;
            // int y;
        }
    }
}