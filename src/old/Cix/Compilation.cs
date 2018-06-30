using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cix.Errors;
using Cix.Parser;
using Cix.Text;

namespace Cix
{
	public sealed class Compilation : IErrorListProvider
	{
		private readonly List<Error> errors = new List<Error>();

		private readonly string initialFilePath;
		private List<Line> initialFile = new List<Line>();
		private List<LexedWord> lexedFile;

		public IReadOnlyList<Line> InitialFile => initialFile.AsReadOnly();
		public IReadOnlyList<LexedWord> LexedFile => lexedFile.AsReadOnly() ?? new List<LexedWord>().AsReadOnly();

		public Compilation(string filePath) => initialFilePath = filePath;

		public void LoadInputFile()
		{
			var io = new IO(this);

			string fileText = io.LoadInputFile(initialFilePath);

			initialFile = io.SplitFileByLine(initialFilePath, fileText).ToList();
			ThrowIfErrors();
		}

		public void RemoveComments()
		{
			var commentRemover = new CommentRemover(this);
			initialFile = commentRemover.RemoveComments(initialFile).ToList();
			ThrowIfErrors();
		}

		public void Preprocess()
		{
			var preprocessor = new Preprocessor(this, initialFile, initialFilePath);
			initialFile = preprocessor.Preprocess().ToList();
			ThrowIfErrors();
		}

		public void Lex()
		{
			var lexer = new Lexer(this);
			lexedFile = lexer.EnumerateWords(initialFile).ToList();
			ThrowIfErrors();
		}

		public void AddError(ErrorSource source, int errorNumber, string message, Line line)
		{
			if (line == null) { line = new Line("{none}", 0, "{none}"); }

			errors.Add(new Error(source, errorNumber, message, line));
		}

		private void ThrowIfErrors()
		{
			if (errors.Any()) { throw new ErrorsEncounteredException(errors); }
		}
	}
}
