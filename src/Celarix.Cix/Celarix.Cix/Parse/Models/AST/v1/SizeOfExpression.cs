using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public sealed class SizeOfExpression : Expression
    {
        public DataType Type { get; set; }
        public override string PrettyPrint() => $"sizeof({Type.PrettyPrint(0)})";
    }
}