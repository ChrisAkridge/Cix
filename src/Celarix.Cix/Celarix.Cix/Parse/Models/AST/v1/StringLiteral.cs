using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public sealed class StringLiteral : Literal
    {
        public string Value { get; set; }
        public override string PrettyPrint() => $"\"{Value}\"";
    }
}