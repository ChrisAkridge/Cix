using System;
using Cix.Errors;

namespace Cix
{
	internal static class EnumExtensions
	{
		public static string ToErrorSourceAbbreviation(this ErrorSource source)
		{
			switch (source)
			{
				case ErrorSource.Invalid:
					throw new ArgumentException("An error was created with no specified source.", nameof(source));
				case ErrorSource.IO: return "IO";
				case ErrorSource.CommentRemover: return "CR";
				case ErrorSource.Preprocesor: return "PR";
				case ErrorSource.Lexer: return "LX";
				case ErrorSource.Tokenizer: return "TK";
				case ErrorSource.FirstPassASTGenerator: return "AA";
				case ErrorSource.SecondPassASTGenerator: return "AB";
				case ErrorSource.CodeGenerator: return "CG";
				default:
					throw new ArgumentOutOfRangeException(nameof(source), source, "An error was created with no valid source.");
			}
		}
	}
}

namespace Cix.Errors
{
	public enum ErrorSource
	{
		Invalid,
		IO,
		CommentRemover,
		Preprocesor,
		Lexer,
		Tokenizer,
		FirstPassASTGenerator,
		SecondPassASTGenerator,
		CodeGenerator
	}
}