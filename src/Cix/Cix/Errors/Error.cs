using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Cix.Text;

namespace Cix.Errors
{
	public abstract class Error
	{
		public ErrorSource Source { get; }
		public int ErrorNumber { get; }
		public string Message { get; }
		protected Error(ErrorSource source, int errorNumber, string message)
		{
			Source = source;
			ErrorNumber = errorNumber;
			Message = message;
		}

		protected static string GetErrorSourceAbbreviation(ErrorSource source)
        {
            return source switch
            {
                ErrorSource.IO => "IO",
                ErrorSource.StringLiteralFinder => "SL",
				ErrorSource.Preprocessor => "PR",
				ErrorSource.CommentRemover => "CR",
                ErrorSource.TypeRewriter => "TR",
                ErrorSource.Lexer => "LX",
                ErrorSource.Tokenizer => "TK",
                ErrorSource.ANTLR4Parser => "PA",
                ErrorSource.ASTGenerator => "AG",
                ErrorSource.Lowering => "LW",
                ErrorSource.CodeGeneration => "CG",
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null)
            };
        }

		public override string ToString() => $"{GetErrorSourceAbbreviation(Source)}{ErrorNumber:D3}: {Message}";
	}

	public enum ErrorSource
	{
		IO,
        StringLiteralFinder,
        Preprocessor,
        CommentRemover,
        TypeRewriter,
        Lexer,
		Tokenizer,
        ANTLR4Parser,
		ASTGenerator,
		Lowering,
		CodeGeneration
	}
}
