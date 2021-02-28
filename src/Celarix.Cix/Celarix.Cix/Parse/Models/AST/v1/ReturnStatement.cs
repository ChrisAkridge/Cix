using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public sealed class ReturnStatement : Statement
    {
        public Expression ReturnValue { get; set; }

        public override string PrettyPrint(int indentLevel)
        {
            var indent = new string(' ', indentLevel * 4);

            return ReturnValue != null
                ? $"{indent}return {ReturnValue.PrettyPrint()};"
                : $"{indent}return;";
        }
    }
}