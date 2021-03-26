using System;
using System.Collections.Generic;
using System.Linq;
using Celarix.Cix.Compiler.Parse.Models.AST.v1;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions
{
    internal sealed class Identifier : TypedExpression
    {
        private Function referentFunction;
        private VirtualStackEntry referentVariable;
        private GlobalVariableInfo referentGlobal;
        private StructMemberInfo referentStructMember;
        
        public IdentifierReferentKind ReferentKind { get; set; }
    }
}