using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public sealed class FunctionInvocation : Expression
    {
        public Expression Operand { get; set; }
        public List<Expression> Arguments { get; set; }
        public override string PrettyPrint() => $"{Operand.PrettyPrint()}({string.Join(", ", Arguments.Select(a => a.PrettyPrint()))})";
    }
}