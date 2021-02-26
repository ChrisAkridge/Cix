using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public sealed class ForStatement : Statement
    {
        public Expression Initializer { get; set; }
        public Expression Condition { get; set; }
        public Expression Iterator { get; set; }
        public Statement LoopStatement { get; set; }
    }
}