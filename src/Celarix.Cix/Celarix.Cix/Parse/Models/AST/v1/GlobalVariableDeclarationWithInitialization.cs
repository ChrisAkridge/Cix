using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public sealed class GlobalVariableDeclarationWithInitialization : GlobalVariableDeclaration
    {
        public Expression Initializer { get; set; }

        public override string PrettyPrint(int indentLevel) => $"{new string(' ', indentLevel * 4)}global {Type.PrettyPrint(0)} {Name} = {Initializer.PrettyPrint()};";
    }
}