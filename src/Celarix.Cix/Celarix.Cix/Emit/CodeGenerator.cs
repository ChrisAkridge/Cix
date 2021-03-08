using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celarix.Cix.Compiler.Emit.Models;
using Celarix.Cix.Compiler.Parse.Models.AST.v1;
using Celarix.Cix.Compiler.Parse.Visitor;

namespace Celarix.Cix.Compiler.Emit
{
    public static class CodeGenerator
    {
        public static object GenerateCode(SourceFileRoot sourceFile)
        {
            var stringLiterals = GetAllStringLiterals(sourceFile);
            
            
        }

        private static List<string> GetAllStringLiterals(SourceFileRoot sourceFile)
        {
            var stringLiteralFinder = new StringLiteralFinder();
            ASTVisitor.VisitSourceFile(stringLiteralFinder, sourceFile);

            return stringLiteralFinder.FoundLiterals;
        }

        private static StructInfo GenerateStructInfo(StructInfo structInfo)
        {
            
        }
    }
}
