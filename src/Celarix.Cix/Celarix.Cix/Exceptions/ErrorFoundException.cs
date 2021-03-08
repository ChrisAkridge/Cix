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
        public string SourceFilePath { get; }
        public int LineNumber { get; }
        public int LineCharacterIndex { get; }

        internal ErrorFoundException(ErrorSource errorSource, int errorNumber, string errorMessage, Line errorLine, int lineCharacterIndex)
            : base($"{GetErrorSourceAbbreviation(errorSource)}{errorNumber:D3}: {errorMessage}")
        {
            ErrorSource = errorSource;
            ErrorNumber = errorNumber;
            ErrorMessage = errorMessage;
            SourceFilePath = errorLine?.SourceFilePath;
            LineNumber = errorLine?.FileLineNumber ?? -1;
            LineCharacterIndex = lineCharacterIndex;
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
                ErrorSource.InternalCompilerError => "IC",
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null)
            };

        public override string ToString() => $"{GetErrorSourceAbbreviation(ErrorSource)}{ErrorNumber:D3}: {ErrorMessage}";
    }
}
