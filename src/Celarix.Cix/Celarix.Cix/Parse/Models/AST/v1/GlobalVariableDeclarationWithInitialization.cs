using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public sealed class GlobalVariableDeclarationWithInitialization : GlobalVariableDeclaration
    {
        public Expression Initializer { get; set; }
    }
}