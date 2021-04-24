using System;
using System.Collections.Generic;
using System.Linq;
using Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.EmitStatements
{
    internal sealed class VariableDeclarationWithInitialization : VariableDeclaration
    {
        public TypedExpression Initializer { get; set; }

        public override GeneratedFlow Generate(EmitContext context, EmitStatement parent)
        {
            Initializer.ComputeType(context, null);
            context.CurrentStack.Push(new VirtualStackEntry(Name, Type));
            
            return new GeneratedFlow
            {
                ControlFlow = Initializer.Generate(context, null), UnconnectedJumps = new List<UnconnectedJump>()
            };
        }
    }
}