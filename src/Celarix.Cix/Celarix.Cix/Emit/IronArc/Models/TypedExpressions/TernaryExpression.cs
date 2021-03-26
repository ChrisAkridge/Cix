using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions
{
    internal sealed class TernaryExpression : TypedExpression
    {
        public TypedExpression Operand1 { get; set; }
        public string Operator1 { get; set; }
        public TypedExpression Operand2 { get; set; }
        public string Operator2 { get; set; }
        public TypedExpression Operand3 { get; set; }
    }
}