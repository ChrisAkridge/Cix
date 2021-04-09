using System;
using System.Collections.Generic;
using System.Linq;
using Celarix.Cix.Compiler.Parse.Models.AST.v1;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    internal sealed class UngeneratedVertex : ControlFlowVertex
    {
        // TODO: this shouldn't be an AST node
        // we probably need models and converters for AST statements to codegen statements
        // and have them all implement Generate and ConnectToGeneratedTree
        public ASTNode NodeToGenerateFor { get; set; }
    }
}