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

            var codeComment = new CommentPrinterVertex(OriginalCode);
            var initializationFlow = Initializer.Generate(context, null);
            
            codeComment.ConnectTo(initializationFlow, FlowEdgeType.DirectFlow);

            return new GeneratedFlow
            {
                ControlFlow = new StartEndVertices(codeComment, initializationFlow.End),
                UnconnectedJumps = new List<UnconnectedJump>()
            };
        }
    }
}