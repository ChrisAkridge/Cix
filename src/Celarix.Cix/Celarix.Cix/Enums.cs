using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celarix.Cix.Compiler
{
    internal enum SaveTempsFile
    {
        Preprocessed,
        AbstractSyntaxTree
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
        CodeGeneration,
        InternalCompilerError
    }
}
