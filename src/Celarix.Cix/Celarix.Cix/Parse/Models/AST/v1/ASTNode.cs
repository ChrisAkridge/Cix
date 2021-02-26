using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public abstract class ASTNode
    {
        public abstract string PrettyPrint(int indentLevel);
    }
}
