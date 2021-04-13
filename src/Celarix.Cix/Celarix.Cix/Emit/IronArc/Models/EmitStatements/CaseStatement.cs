using System;
using System.Collections.Generic;
using System.Linq;
using Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.EmitStatements
{
    internal sealed class CaseStatement : EmitStatement, ISecondPassConnect
    {
        public Literal CaseLiteral { get; set; }
        public EmitStatement Statement { get; set; }

        public override StartEndVertices Generate(EmitContext context, EmitStatement parent) => EmitHelpers.GetUngeneratedVertex(this);

        public void ConnectToGeneratedTree(ControlFlowVertex after, EmitContext context = null)
        {
            context.BreakTargets.Push(after);
            Statement.Generate(context, this).ConnectTo(after, FlowEdgeType.UnconditionalJump);
        }
    }
}