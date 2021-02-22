using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace Celarix.Cix.Console
{
	internal sealed class CompilerOptions
	{
		[Option('i', "input", Required = true, HelpText = "The path to the Cix file you wish to compile.")]
		public string InputFilePath { get; set; }

		[Option('o', "output", Required = true, HelpText = "The path to the IronArc assembly file you wish to compile to.")]
		public string OutputFilePath { get; set; }

		[Option('t', "save-temps", Required = false, HelpText = "Outputs the preprocessed Cix file and its AST as JSON to the output folder.")]
		public bool SaveTemps { get; set; }

		[Option('s', "symbols", Required = false, HelpText = "A comma-separated list of symbols that will be treated as declared during preprocessing.")]
		public string SymbolsText { get; set; }

		[Option('l', "log-level", Required = false, Default = "info", HelpText = "The minimum logging level (trace, debug, log, warn, error, fatal) that will be displayed.")]
		public string LogLevel { get; set; }

        public IEnumerable<string> Symbols => SymbolsText?.Split(',') ?? Array.Empty<string>();
    }
}
