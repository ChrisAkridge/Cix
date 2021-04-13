using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.EmitStatements
{
    internal class VariableDeclaration : EmitStatement
    {
        public TypeInfo Type { get; set; }
        public string Name { get; set; }
    }
}