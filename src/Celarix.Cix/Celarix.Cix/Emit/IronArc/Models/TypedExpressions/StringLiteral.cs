using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions
{
    internal sealed class StringLiteral : TypedExpression
    {
        public string LiteralValue { get; set; }

        public override UsageTypeInfo ComputeType(ExpressionEmitContext context, TypedExpression parent)
        {
            ComputedType = new UsageTypeInfo
            {
                DeclaredType = new NamedTypeInfo
                {
                    Name = "byte", Size = 1
                },
                PointerLevel = 1
            };

            return ComputedType;
        }
    }
}