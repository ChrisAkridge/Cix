using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public sealed class Block : Statement
    {
        public List<Statement> Statements { get; set; }
    }
}