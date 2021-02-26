using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public sealed class SourceFileRoot : ASTNode
    {
        public int ASTVersion { get; set; }
        public List<Struct> Structs { get; set; }
        public List<GlobalVariableDeclaration> GlobalVariableDeclarations { get; set; }
        public List<Function> Functions { get; set; }

        public override string PrettyPrint(int indentLevel)
        {
            var fileBuilder = new StringBuilder();

            foreach (var @struct in Structs) { fileBuilder.AppendLine(@struct.PrettyPrint(0)); }
            fileBuilder.AppendLine();
            
            foreach (var globalVariableDeclaration in GlobalVariableDeclarations) { fileBuilder.AppendLine(globalVariableDeclaration.PrettyPrint(0)); }
            fileBuilder.AppendLine();

            foreach (var function in Functions) { fileBuilder.AppendLine(function.PrettyPrint(0)); }

            return fileBuilder.ToString();
        }
    }
}
