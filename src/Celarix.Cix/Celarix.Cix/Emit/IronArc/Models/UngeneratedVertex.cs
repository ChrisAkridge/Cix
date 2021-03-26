using System;
using System.Collections.Generic;
using System.Linq;
using Celarix.Cix.Compiler.Parse.Models.AST.v1;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    internal sealed class UngeneratedVertex : ControlFlowVertex
    {
        public ASTNode NodeToGenerateFor { get; set; }
    }
}