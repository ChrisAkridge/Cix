using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public class VariableDeclaration : Statement
    {
        public DataType Type { get; set; }
        public string Name { get; set; }
        public override string PrettyPrint(int indentLevel) => $"{new string(' ', indentLevel * 4)}{Type.PrettyPrint(0)} {Name};";
    }
}