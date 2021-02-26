using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;

namespace Celarix.Cix.Compiler.Parse.ANTLR
{
    internal sealed class ParsingErrorListener : BaseErrorListener
    {
        // https://stackoverflow.com/a/26573239/2709212
        //private readonly IList<Error> parsingErrors = new List<Error>();

        //public IReadOnlyList<Error> ParsingErrors => parsingErrors.AsReadOnly();

        //public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine,
        //    string msg, RecognitionException e)
        //{
        //    parsingErrors.Add(new LineError(ErrorSource.ANTLR4Parser, 1, msg, "Preprocessed File", line));
            
        //    base.SyntaxError(output, recognizer, offendingSymbol, line, charPositionInLine, msg, e);
        //}
    }
}
