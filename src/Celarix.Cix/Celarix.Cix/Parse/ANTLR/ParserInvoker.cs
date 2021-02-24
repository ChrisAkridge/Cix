using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
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
            var sourceFileText = file.JoinLines();
            var charStream = CharStreams.fromString(sourceFileText);

            var lexerOutWriter = new StringWriter();
            var lexerErrorWriter = new StringWriter();
            var lexer = new CixLexer(charStream, lexerOutWriter, lexerErrorWriter);

            var tokenStream = new CommonTokenStream(lexer);
            var parserOutWriter = new StringWriter();
            var parserErrorWriter = new StringWriter();
            var parser = new CixParser(tokenStream, parserOutWriter, parserErrorWriter);

            return parser.sourceFile();
        }
    }
}
