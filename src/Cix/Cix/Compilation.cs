using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cix.AST;
using Cix.AST.Generator;
using Cix.Errors;
using Cix.Models;
using Cix.Parser;
using Cix.Text;
using Newtonsoft.Json;

namespace Cix
{
	public sealed class Compilation : IErrorListProvider
	{
		private readonly List<Error> errors = new List<Error>();
		private readonly string initialFilePath;
		//private List<Line> initialFile = new List<Line>();
		//private List<LexedWord> lexedFile;
		//private List<Token> tokenizedFile;
		//private List<Element> abstractSyntaxTree;
		private readonly HardwareDefinition hardwareDefinition;

		//public IReadOnlyList<Line> InitialFile => initialFile.AsReadOnly();
		//public IReadOnlyList<LexedWord> LexedFile => lexedFile.AsReadOnly() ?? new List<LexedWord>().AsReadOnly();

		//public IReadOnlyList<Token> TokenizedFile =>
		//          tokenizedFile.AsReadOnly() ?? new List<Token>().AsReadOnly();

		//public IReadOnlyList<Element> AbstractSyntaxTree => abstractSyntaxTree.AsReadOnly();
        
        // WYLO: We may need to rethink how we're handling errors.
        // Perhaps a string iterator that can also report current file, line number, and char on each character.

		public Compilation(string filePath, string hardwareDefinitionPath)
		{
			initialFilePath = filePath;

			string hardwareDefinitionFile = System.IO.File.ReadAllText(hardwareDefinitionPath);
			hardwareDefinition = JsonConvert.DeserializeObject<HardwareDefinition>(hardwareDefinitionFile);
		}

		//public void LoadInputFile()
		//{
		//	var io = new IO(this);

		//	string fileText = io.LoadInputFile(initialFilePath);

		//	initialFile = io.SplitFileByLine(initialFilePath, fileText).ToList();
		//	ThrowIfErrors();
		//}

  //      public void Preprocess()
  //      {
  //          var preprocessor = new Preprocessor(this, initialFile, initialFilePath);
  //          initialFile = preprocessor.Preprocess().ToList();
  //          ThrowIfErrors();
  //      }

		//public void RemoveComments()
		//{
		//	var commentRemover = new CommentRemover(this);
		//	initialFile = commentRemover.RemoveComments(initialFile).ToList();
		//	ThrowIfErrors();
		//}
        
  //      [Obsolete]
		//public void Lex()
		//{
		//	var lexer = new Lexer(this);
		//	lexedFile = lexer.EnumerateWords(initialFile).ToList();
		//	ThrowIfErrors();
		//}

  //      [Obsolete]
		//public void Tokenize()
		//{
		//	var tokenizer = new Tokenizer(this);
		//	tokenizedFile = tokenizer.Tokenize(lexedFile).ToList();
		//	ThrowIfErrors();
		//}

  //      [Obsolete]
		//public void GenerateAST()
		//{
		//	var tokenEnumerator = new TokenEnumerator(tokenizedFile, this);
		//	var firstPassGenerator = new FirstPassGenerator(tokenEnumerator, this);

		//	firstPassGenerator.GenerateFirstPassAST();
		//	abstractSyntaxTree = firstPassGenerator.Tree.ToList();
		//	ThrowIfErrors();
		//}

		public void AddLineError(ErrorSource source, int errorNumber, string message, Line line)
        {
            line ??= new Line("{none}", 0, "{none}");

            errors.Add(new LineError(source, errorNumber, message, line));
        }

		public void AddLineError(ErrorSource source, int errorNumber, string message, string filePath,
			int lineNumber)
		{
			errors.Add(new LineError(source, errorNumber, message, filePath, lineNumber));
		}

        public void AddCharError(ErrorSource source, int errorNumber, string message, string filePath, int startIndex,
            int endIndex)
        {
            errors.Add(new CharError(source, errorNumber, message, filePath, startIndex, endIndex));
        }

        private void ThrowIfErrors()
		{
			if (errors.Any()) { throw new ErrorsEncounteredException(errors); }
		}
	}
}
