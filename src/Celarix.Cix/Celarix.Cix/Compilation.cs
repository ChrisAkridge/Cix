using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celarix.Cix.Compiler.IO.Models;
using Celarix.Cix.Compiler.Preparse;
using NLog;

namespace Celarix.Cix.Compiler
{
    public sealed class Compilation
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        
        private List<Line> preprocessedFile = new List<Line>();
        
        public CompilationOptions CompilationOptions { get; init; }

        internal IReadOnlyList<Line> PreprocessedFile => preprocessedFile.AsReadOnly();
        // public Program AbstractSyntaxTreeRoot { get; private set; }
        public string IronArcAssemblyFile { get; private set; }

        public void Preparse()
        {
            logger.Trace("Starting preparse phase...");
            
            var lines = IO.IO.SplitFileIntoLines(CompilationOptions.InputFilePath);
            preprocessedFile = new Preprocessor(lines, CompilationOptions.DeclaredSymbols).Preprocess().ToList();
            Preprocessor.SetOverallLineAndCharacterIndices(preprocessedFile);
            
            StringLiteralMarker.MarkStringLiterals(preprocessedFile);

            CommentRemover.RemoveComments(preprocessedFile);

            var tempFileText = IO.IO.JoinLinesIntoString(preprocessedFile);
            File.WriteAllText(CompilationOptions.OutputFilePath, tempFileText);
        }
    }
}
