using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celarix.Cix.Compiler.IO.Models;
using Celarix.Cix.Compiler.Parse.ANTLR;
using Celarix.Cix.Compiler.Parse.Models.AST;
using Celarix.Cix.Compiler.Parse.Models.AST.v1;
using Celarix.Cix.Compiler.Preparse;
using Celarix.Cix.Compiler.Preparse.Models;
using Newtonsoft.Json;
using NLog;

namespace Celarix.Cix.Compiler
{
    public sealed class Compilation
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        
        private List<Line> preprocessedFile = new List<Line>();

        private SourceFile preparseFile;
        
        public CompilationOptions CompilationOptions { get; init; }

        internal IReadOnlyList<Line> PreprocessedFile => preprocessedFile.AsReadOnly();

        internal SourceFile PreparseFile => preparseFile;
        
        public SourceFileRoot AbstractSyntaxTreeRoot { get; private set; }
        public string IronArcAssemblyFile { get; private set; }

        public void Preparse()
        {
            logger.Trace("Starting preparse phase...");
            
            var lines = IO.IO.SplitFileIntoLines(CompilationOptions.InputFilePath);
            preprocessedFile = new Preprocessor(lines, CompilationOptions.DeclaredSymbols).Preprocess().ToList();
            Preprocessor.SetOverallLineAndCharacterIndices(preprocessedFile);
            
            StringLiteralMarker.MarkStringLiterals(preprocessedFile);

            CommentRemover.RemoveComments(preprocessedFile);

            preparseFile = new SourceFile(preprocessedFile);
            
            logger.Trace("End preparse phase...");
        }

        public void Parse()
        {
            logger.Trace("Starting parse phase...");
            
            var sourceFileContext = ParserInvoker.Invoke(preparseFile);
            AbstractSyntaxTreeRoot = ASTGenerator.GenerateSourceFile(sourceFileContext);

            var tempFileText = JsonConvert.SerializeObject(AbstractSyntaxTreeRoot, Formatting.Indented,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });
            File.WriteAllText(CompilationOptions.OutputFilePath, tempFileText);
            
            logger.Trace("Ending parse phase...");
        }
    }
}
