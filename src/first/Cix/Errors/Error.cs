using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Cix.Text;

namespace Cix.Errors
{
	public sealed class Error
	{
		public ErrorSource Source { get; }
		public int ErrorNumber { get; }
		public string Message { get; }
		public string FilePath { get; }
		public int LineNumber { get; }

		public Error(ErrorSource source, int errorNumber, string message, Line line)
		{
			Source = source;
			ErrorNumber = errorNumber;
			Message = message;
			FilePath = line.FilePath;
			LineNumber = line.LineNumber;
		}

		public Error(ErrorSource source, int errorNumber, string message, string filePath, int lineNumber)
		{
			Source = source;
			ErrorNumber = errorNumber;
			Message = message;
			FilePath = filePath;
			LineNumber = lineNumber;
		}

		private static string GetErrorSourceAbbreviation(ErrorSource source)
		{
			switch (source)
			{
				case ErrorSource.IO: return "IO";
				case ErrorSource.CommentRemover: return "CR";
				case ErrorSource.Preprocessor: return "PR";
				case ErrorSource.Lexer: return "LX";
				case ErrorSource.Tokenizer: return "TK";
				case ErrorSource.ASTGenerator: return "AG";
				case ErrorSource.Lowering: return "LW";
				case ErrorSource.CodeGeneration: return "CG";
				default: throw new ArgumentOutOfRangeException(nameof(source), source, null);
			}
		}

		public override string ToString()
			=> $"{GetErrorSourceAbbreviation(Source)}{ErrorNumber:D3}: {Message} ({Path.GetFileName(FilePath)}:{LineNumber + 1})";
	}

	public enum ErrorSource
	{
		IO,
		CommentRemover,
		Preprocessor,
		Lexer,
		Tokenizer,
		ASTGenerator,
		Lowering,
		CodeGeneration
	}
}
