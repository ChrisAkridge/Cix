using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public sealed class ReturnStatement : Statement
    {
        public Expression ReturnValue { get; set; }
    }
}