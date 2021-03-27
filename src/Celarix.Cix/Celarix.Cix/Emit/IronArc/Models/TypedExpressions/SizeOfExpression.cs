using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions
{
    internal sealed class SizeOfExpression : TypedExpression
    {
        public UsageTypeInfo Type { get; set; }

        public override UsageTypeInfo ComputeType(TypeComputationContext context, TypedExpression parent)
        {
            ComputedType = new UsageTypeInfo
            {
                DeclaredType = new NamedTypeInfo
                {
                    Name = "int", Size = 4
                }
            };

            return ComputedType;
        }
    }
}