using System;
using System.Collections.Generic;
using System.Linq;
using Celarix.Cix.Compiler.Parse.Models.AST.v1;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions
{
    internal sealed class HardwareCallReturnsInternal : TypedExpression
    {
        public Celarix.Cix.Compiler.Parse.Models.AST.v1.HardwareCallReturnsInternal ASTNode { get; set; }
        public string CallName { get; set; }
        public UsageTypeInfo ReturnType { get; set; }
        public List<UsageTypeInfo> ParameterTypes { get; set; }

        public override UsageTypeInfo ComputeType(ExpressionEmitContext context, TypedExpression parent)
        {
            ComputedType = ReturnType;

            return ComputedType;
        }
    }
}