using System;
using System.Collections.Generic;
using System.Linq;
using Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions;
using NLog;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.EmitStatements
{
    internal sealed class CaseStatement : EmitStatement
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public Literal CaseLiteral { get; set; }
        public EmitStatement Statement { get; set; }

        public override GeneratedFlow Generate(EmitContext context, EmitStatement parent)
        {
            logger.Trace("Generating code for case statement...");
            
            return Statement.Generate(context, this);
        }
    }
}