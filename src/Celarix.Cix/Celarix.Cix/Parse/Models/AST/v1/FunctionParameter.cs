using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public sealed class FunctionParameter : ASTNode
    {
        public DataType Type { get; set; }
        public string Name { get; set; }
        public override string PrettyPrint(int indentLevel) => $"{Type.PrettyPrint(0)} {Name}";
    }
}