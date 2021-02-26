using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public sealed class Identifier : Literal
    {
        public string IdentifierText { get; set; }
        public override string PrettyPrint() => IdentifierText;
    }
}