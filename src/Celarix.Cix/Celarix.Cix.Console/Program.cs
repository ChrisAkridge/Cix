using System;
using System.IO.Enumeration;
using System.Linq;
using Celarix.Cix.Compiler;
using CommandLine;
using NLog;

namespace Celarix.Cix.Console
{
	public static class Program
	{
		private static void Main(string[] args)
        {
            CompilationOptions cixCompilationOptions = null;
            
            Parser.Default.ParseArguments<CompilerOptions>(args)
                .WithParsed(o =>
                {
                    cixCompilationOptions = new CompilationOptions
                    {
                        InputFilePath = o.InputFilePath,
                        OutputFilePath = o.OutputFilePath,
                        SaveTemps = o.SaveTemps,
                        DeclaredSymbols = o.Symbols.ToList()
                    };
                    
                    LoggingConfigurer.ConfigureLogging(o.LogLevel);
                });

            var compilation = new Compilation { CompilationOptions = cixCompilationOptions };
            compilation.Preparse();
            compilation.Parse();
        }
	}
}
