using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions
{
    internal sealed class HardwareCallReturnsInternal : TypedExpression
    {
        public string CallName { get; set; }
        public List<UsageTypeInfo> ParameterTypes { get; set; }
    }
}