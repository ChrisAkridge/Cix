using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions
{
    internal abstract class TypedExpression
    {
        public UsageTypeInfo ComputedType { get; set; }
        public bool IsAssignable { get; set; }

        public abstract UsageTypeInfo ComputeType(TypeComputationContext context, TypedExpression parent);
    }
}
