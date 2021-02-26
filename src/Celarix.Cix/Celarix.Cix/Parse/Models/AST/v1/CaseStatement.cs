using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public class CaseStatement : Statement
    {
        public Literal CaseLiteral { get; set; }
        public Statement Statement { get; set; }
    }
}