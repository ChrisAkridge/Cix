using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;
using CommandLine.Text;

namespace CixCLI
{
    internal class Options
    {
		[Option('i', "input", HelpText = "\"path\\to\\input.cix\": The path to an input file containing Cix code.", Required = true)]
	    public string InputFilePath { get; set; }

	    [Option('o', "output", HelpText = "\"path\\to\\output.iasm\": The path to the output file to write the compiled IronArc assembly to.", Required = true)]
		public string OutputFilePath { get; set; }

	    [Option('r', "removeComments", HelpText = "Remove comments only.", Required = false)]
		public bool RunUpToRemoveComments { get; set; }

	    [Option('p', "preprocess", HelpText = "Remove comments and run the preprocessor only.", Required = false)]
		public bool RunUpToPreprocessing { get; set; }

	    [Option('x', "lex", HelpText = "Remove comments, run the preprocessor, and perform lexing only.", Required = false)]
		public bool RunUpToLexing { get; set; }

	    [Option('t', "tokenize", HelpText = "Remove comments, run the preprocessor, perform lexing, and tokenize only.", Required = false)]
		public bool RunUpToTokenization { get; set; }

	    [Option('a', "generateAST", HelpText = "Remove comments, run the preprocessor, perform lexing, tokenize, and generate the AST only.", Required = false)]
		public bool RunUpToASTGeneration { get; set; }

	    [Option('l', "lower", HelpText = "Remove comments, run the preprocessor, perform lexing, tokenize, generate the AST, and perform lowering only.", Required = false)]
		public bool RunUpToLowering { get; set; }

	    public bool MoreThanOneCompileOptionSet =>
		    (new List<bool>()
		    {
			    RunUpToRemoveComments,
			    RunUpToPreprocessing,
			    RunUpToLexing,
			    RunUpToTokenization,
			    RunUpToASTGeneration,
			    RunUpToLowering
		    }).Count(b => b) > 1;

	    public CompileUpTo GetCompileUpTo()
	    {
		    if (RunUpToRemoveComments)
		    {
			    return CompileUpTo.RemovingComments;
		    }
			else if (RunUpToPreprocessing)
		    {
			    return CompileUpTo.Preprocessing;
		    }
			else if (RunUpToLexing)
		    {
			    return CompileUpTo.Lexing;
		    }
			else if (RunUpToTokenization)
		    {
			    return CompileUpTo.Tokenization;
		    }
			else if (RunUpToASTGeneration)
		    {
			    return CompileUpTo.ASTGeneration;
		    }
			else if (RunUpToLowering)
		    {
			    return CompileUpTo.Lowering;
		    }
		    else
		    {
			    return CompileUpTo.FullCompile;
		    }
	    }
    }

	internal enum CompileUpTo
	{
		RemovingComments,
		Preprocessing,
		Lexing,
		Tokenization,
		ASTGeneration,
		Lowering,
		FullCompile
	}
}
