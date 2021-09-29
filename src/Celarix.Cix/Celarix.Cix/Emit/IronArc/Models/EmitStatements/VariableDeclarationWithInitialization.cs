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

            var codeComment = new CommentPrinterVertex(OriginalCode);
            var initializationFlow = Initializer.Generate(context, null);

            // The initializer pushes a virtual stack element representing their
            // result, so pop it off and replace it with our named variable
            context.CurrentStack.Pop();
            context.CurrentStack.Push(new VirtualStackEntry(Name, Type));

            codeComment.ConnectTo(initializationFlow, FlowEdgeType.DirectFlow);

            return new GeneratedFlow
            {
                ControlFlow = new StartEndVertices(codeComment, initializationFlow.End),
                UnconnectedJumps = new List<UnconnectedJump>()
            };
        }
    }
}