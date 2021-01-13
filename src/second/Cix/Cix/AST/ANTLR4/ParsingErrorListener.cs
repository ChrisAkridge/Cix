using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Dfa;
using Antlr4.Runtime.Sharpen;
using Cix.Errors;
using Cix.Extensions;

namespace Cix.AST.ANTLR4
{
    internal sealed class ParsingErrorListener : BaseErrorListener
    {
        // https://stackoverflow.com/a/26573239/2709212
        private readonly IList<Error> parsingErrors = new List<Error>();

        public IReadOnlyList<Error> ParsingErrors => parsingErrors.AsReadOnly();

        public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine,
            string msg, RecognitionException e)
        {
            parsingErrors.Add(new LineError(ErrorSource.ANTLR4Parser, 1, msg, "Preprocessed File", line));
            
            base.SyntaxError(output, recognizer, offendingSymbol, line, charPositionInLine, msg, e);
        }
    }
}
