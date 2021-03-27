using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions
{
    internal sealed class FloatingPointLiteral : TypedExpression
    {
        public ulong ValueBits { get; set; }
        public NumericLiteralType NumericLiteralType { get; set; }

        public override UsageTypeInfo ComputeType(TypeComputationContext context, TypedExpression parent)
        {
            ComputedType = (NumericLiteralType == NumericLiteralType.Single)
                ? new UsageTypeInfo
                {
                    DeclaredType = new NamedTypeInfo
                    {
                        Name = "float", Size = 4
                    }
                }
                : new UsageTypeInfo
                {
                    DeclaredType = new NamedTypeInfo
                    {
                        Name = "double", Size = 4
                    }
                };
            return ComputedType;
        }
    }
}