using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public sealed class SwitchStatement : Statement
    {
        public Expression Expression { get; set; }
        public List<CaseStatement> Cases { get; set; }
    }
}