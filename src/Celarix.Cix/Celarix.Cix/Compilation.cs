using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Celarix.Cix.Compiler.Emit;
using Celarix.Cix.Compiler.Emit.IronArc;
using Celarix.Cix.Compiler.IO.Models;
using Celarix.Cix.Compiler.Lowering;
using Celarix.Cix.Compiler.Lowering.Models;
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
            logger.Info("Starting preparse phase...");
            
            var lines = IO.IO.SplitFileIntoLines(CompilationOptions.InputFilePath);
            preprocessedFile = new Preprocessor(lines, CompilationOptions.DeclaredSymbols).Preprocess().ToList();
            Preprocessor.SetOverallLineAndCharacterIndices(preprocessedFile);
            
            StringLiteralMarker.MarkStringLiterals(preprocessedFile);

            CommentRemover.RemoveComments(preprocessedFile);

            preparseFile = new SourceFile(preprocessedFile);
            
            logger.Info("End preparse phase...");
        }

        public void Parse()
        {
            logger.Info("Starting parse phase...");
            
            var sourceFileContext = ParserInvoker.Invoke(preparseFile);
            AbstractSyntaxTreeRoot = ASTGenerator.GenerateSourceFile(sourceFileContext);

            if (CompilationOptions.SaveTemps)
            {
                var astJson = JsonConvert.SerializeObject(AbstractSyntaxTreeRoot, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });
                
                File.WriteAllText(IO.IO.GetSaveTempsPath(CompilationOptions.OutputFilePath, SaveTempsFile.AbstractSyntaxTree), astJson);
            }
            
            logger.Info("Ending parse phase...");
        }

        public void Lower()
        {
            logger.Info("Start lowering phase...");

            var hardwareDefinitionJson = File.ReadAllText(CompilationOptions.HardwareDefinitionPath);
            var hardwareDefinition = JsonConvert.DeserializeObject<HardwareDefinition>(hardwareDefinitionJson);
            var hardwareCallFunctions = HardwareCallWriter.WriteHardwareCallFunctions(hardwareDefinition);
            AbstractSyntaxTreeRoot.Functions.AddRange(hardwareCallFunctions);
            
            Lowerings.PerformLowerings(AbstractSyntaxTreeRoot);

            if (CompilationOptions.SaveTemps)
            {
                var tempFileText = AbstractSyntaxTreeRoot.PrettyPrint(0);
                File.WriteAllText(IO.IO.GetSaveTempsPath(CompilationOptions.OutputFilePath, SaveTempsFile.Preprocessed), tempFileText);
            }

            logger.Info("End lowering phase...");
        }

        public void Emit()
        {
            logger.Info("Start emit phase...");

            var codeGenerator = new CodeGenerator(AbstractSyntaxTreeRoot);
            codeGenerator.GenerateCode();
            IronArcAssemblyFile = codeGenerator.IronArcAssembly;
            
            logger.Info("End emit phase...");
        }

        public void SaveAssemblyFile()
        {
            logger.Info("Saving assembly file...");
            
            File.WriteAllText(CompilationOptions.OutputFilePath, IronArcAssemblyFile);
            
            logger.Info("Saved assembly file");
        }
    }
}
