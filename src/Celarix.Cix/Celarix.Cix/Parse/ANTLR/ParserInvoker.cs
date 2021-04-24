using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Celarix.Cix.Compiler.Exceptions;
using Celarix.Cix.Compiler.Parse.AST;
using Celarix.Cix.Compiler.Preparse.Models;
using NLog;

namespace Celarix.Cix.Compiler.Parse.ANTLR
{
    internal static class ParserInvoker
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static CixParser.SourceFileContext Invoke(SourceFile file)
        {
            logger.Debug($"Invoking ANTLR4 parser...");
            
            var sourceFileText = file.JoinLines();
            var charStream = CharStreams.fromString(sourceFileText);
            
            logger.Trace($"Parse file has {sourceFileText.Length} characters");

            var lexerOutWriter = new StringWriter();
            var lexerErrorWriter = new StringWriter();
            var lexer = new CixLexer(charStream, lexerOutWriter, lexerErrorWriter);

            var tokenStream = new CommonTokenStream(lexer);
            var parserOutWriter = new StringWriter();
            var parserErrorWriter = new StringWriter();
            var parser = new CixParser(tokenStream, parserOutWriter, parserErrorWriter);

            var errorBuilder = new StringBuilder();

            if (lexerErrorWriter.GetStringBuilder()?.Length > 0)
            {
                errorBuilder.Append(lexerErrorWriter.GetStringBuilder());
            }

            if (parserErrorWriter.GetStringBuilder()?.Length > 0)
            {
                errorBuilder.Append(parserErrorWriter.GetStringBuilder());
            }

            if (errorBuilder.Length > 0)
            {
                throw new ErrorFoundException(ErrorSource.ANTLR4Parser, 0, errorBuilder.ToString(), null, 0);
            }

            logger.Debug("ANTLR4 parsing complete");
            return parser.sourceFile();
        }
    }
}
