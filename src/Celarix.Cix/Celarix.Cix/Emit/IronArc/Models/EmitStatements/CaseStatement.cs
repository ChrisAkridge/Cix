using System;
using System.Collections.Generic;
using System.Linq;
using Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.EmitStatements
{
    internal sealed class CaseStatement : EmitStatement
    {
        public Literal CaseLiteral { get; set; }
        public EmitStatement Statement { get; set; }

        public override StartEndVertices Generate(EmitContext context, EmitStatement parent) =>
            Statement.Generate(context, this);

        public override void ConnectVerticesToTargets(ControlFlowVertex selfStartVertex, EmitContext context,
            ControlFlowVertex breakAfterTarget, ControlFlowVertex continueTarget)
        {
            Statement.ConnectVerticesToTargets(selfStartVertex, context, breakAfterTarget, continueTarget);
        }
    }
}