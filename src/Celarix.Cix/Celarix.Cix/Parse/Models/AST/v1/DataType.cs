using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public abstract class DataType : ASTNode
    {
        public int PointerLevel { get; set; }
    }
}
