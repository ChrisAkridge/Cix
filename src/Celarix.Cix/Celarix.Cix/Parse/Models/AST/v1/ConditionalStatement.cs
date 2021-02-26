using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public sealed class ConditionalStatement : Statement
    {
        public Expression Condition { get; set; }
        public Statement IfTrue { get; set; }
        public Statement IfFalse { get; set; }
    }
}