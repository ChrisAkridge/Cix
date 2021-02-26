using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public sealed class ArrayAccess : Expression
    {
        public Expression Operand { get; set; }
        public Expression Index { get; set; }
        public override string PrettyPrint() => $"{Operand.PrettyPrint()}[{Index.PrettyPrint()}]";
    }
}