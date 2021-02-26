using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public sealed class Function : ASTNode
    {
        public DataType ReturnType { get; set; }
        public string Name { get; set; }
        public List<FunctionParameter> Parameters { get; set; }
        public List<Statement> Statements { get; set; }
    }
}