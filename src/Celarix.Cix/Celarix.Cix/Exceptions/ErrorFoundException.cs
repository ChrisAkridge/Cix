using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celarix.Cix.Compiler.IO.Models;

namespace Celarix.Cix.Compiler.Exceptions
{
    public sealed class ErrorFoundException : Exception
    {
        public ErrorSource ErrorSource { get; }
        public int ErrorNumber { get; }
        public string ErrorMessage { get; }
        public Line ErrorLine { get; }

        public ErrorFoundException(ErrorSource errorSource, int errorNumber, string errorMessage, Line errorLine)
            : base($"{GetErrorSourceAbbreviation(errorSource)}{errorNumber:D3}: {errorMessage}")
        {
            ErrorSource = errorSource;
            ErrorNumber = errorNumber;
            ErrorMessage = errorMessage;
            ErrorLine = errorLine;
        }

        public static string GetErrorSourceAbbreviation(ErrorSource source) =>
            source switch
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

        public override string ToString() => $"{GetErrorSourceAbbreviation(ErrorSource)}{ErrorNumber:D3}: {ErrorMessage}";
    }
}
