using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public sealed class Struct : ASTNode
    {
        public string Name { get; set; }
        public List<StructMember> Members { get; set; }
    }
}
