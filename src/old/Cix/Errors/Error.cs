using System;
using System.Collections.Generic;
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

		private static string GetErrorSourceAbbreviation(ErrorSource source)
		{
			switch (source)
			{
				case ErrorSource.IO: return "IO";
				case ErrorSource.CommentRemover: return "CR";
				case ErrorSource.Preprocessor: return "PR";
				case ErrorSource.Lexer: return "LX";
				case ErrorSource.Tokenizer: return "TK";
				case ErrorSource.ASTFirstPass: return "AA";
				case ErrorSource.ASTSecondPass: return "AB";
				case ErrorSource.Lowering: return "LW";
				case ErrorSource.CodeGeneration: return "CG";
				default: throw new ArgumentOutOfRangeException(nameof(source), source, null);
			}
		}
	}

	public enum ErrorSource
	{
		IO,
		CommentRemover,
		Preprocessor,
		Lexer,
		Tokenizer,
		ASTFirstPass,
		ASTSecondPass,
		Lowering,
		CodeGeneration
	}
}
