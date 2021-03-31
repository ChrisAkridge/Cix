using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions
{
    internal sealed class HardwareCallVoidInternal : TypedExpression
    {
        public Celarix.Cix.Compiler.Parse.Models.AST.v1.HardwareCallVoidInternal ASTNode { get; set; }
        public string CallName { get; set; }
        public List<UsageTypeInfo> ParameterTypes { get; set; }

        public override UsageTypeInfo ComputeType(ExpressionEmitContext context, TypedExpression parent)
        {
            ComputedType = new UsageTypeInfo
            {
                DeclaredType = new NamedTypeInfo
                {
                    Name = "void", Size = 1
                }
            };

            return ComputedType;
        }
    }
}