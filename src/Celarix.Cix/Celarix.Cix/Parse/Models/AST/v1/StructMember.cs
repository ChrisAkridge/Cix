using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public sealed class StructMember : ASTNode
    {
        public DataType Type { get; set; }
        public string Name { get; set; }
        public int StructArraySize { get; set; }

        public override string PrettyPrint(int indentLevel)
        {
            var indent = new string(' ', indentLevel * 4);
            var arraySize = (StructArraySize > 1) ? $"[{StructArraySize}]" : "";

            return $"{indent}{Type.PrettyPrint(0)} {Name}{StructArraySize};";
        }
    }
}
