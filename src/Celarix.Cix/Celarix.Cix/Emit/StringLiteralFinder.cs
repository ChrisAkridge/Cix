using System;
using System.Collections.Generic;
using System.Linq;
using Celarix.Cix.Compiler.Parse.Models.AST.v1;
using Celarix.Cix.Compiler.Parse.Visitor;

namespace Celarix.Cix.Compiler.Emit
{
    internal sealed class StringLiteralFinder : ASTVisitor
    {
        public List<string> FoundLiterals { get; set; } = new List<string>();
        
        public override void VisitStringLiteral(StringLiteral stringLiteral)
        {
            FoundLiterals.Add(stringLiteral.Value);
        }
    }
}
