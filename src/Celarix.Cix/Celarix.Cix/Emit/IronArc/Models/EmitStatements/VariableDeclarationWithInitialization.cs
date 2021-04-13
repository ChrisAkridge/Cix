using System;
using System.Collections.Generic;
using System.Linq;
using Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.EmitStatements
{
    internal sealed class VariableDeclarationWithInitialization : VariableDeclaration
    {
        public TypedExpression Initializer { get; set; }
    }
}