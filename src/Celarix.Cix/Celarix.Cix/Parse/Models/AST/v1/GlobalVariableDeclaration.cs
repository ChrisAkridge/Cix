using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public class GlobalVariableDeclaration : ASTNode
    {
        public DataType Type { get; set; }
        public string Name { get; set; }
    }
}