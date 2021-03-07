using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celarix.Cix.Compiler.Parse.Models.AST.v1;
using Celarix.Cix.Compiler.Parse.Visitor;

namespace Celarix.Cix.Compiler.Emit.Models
{
    internal sealed class StringLiteralFinder : ASTVisitor
    {
        public List<string> FoundLiterals { get; set; }
        
        public override void VisitStringLiteral(StringLiteral stringLiteral)
        {
            FoundLiterals.Add(stringLiteral.Value);
        }
    }
}
