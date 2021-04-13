using System;
using System.Collections.Generic;
using System.Linq;
using Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.EmitStatements
{
    internal sealed class WhileStatement : EmitStatement
    {
        public TypedExpression Condition { get; set; }
        public EmitStatement LoopStatement { get; set; }
    }
}