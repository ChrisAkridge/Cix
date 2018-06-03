using System;
using System.IO;
using CommandLine;

namespace CixCLI
{
	internal class Program
    {
	    private static void Main(string[] args)
	    {
			ParserResult<Options> result = Parser.Default.ParseArguments<Options>(args);

		    if (result is Parsed<Options> options)
		    {
			    if (options.Value.MoreThanOneCompileOptionSet)
			    {
					Console.WriteLine("You can only compile up to a single given stage. Please check your arguments.");
				    return;
			    }

			    CompileUpTo compileUpTo = options.Value.GetCompileUpTo();
			    string inputFilePath = options.Value.InputFilePath;
			    if (!TryReadInputFile(inputFilePath, out string inputFileText))
			    {
				    return;
			    }

			    string outputFilePath = options.Value.OutputFilePath;
			    if (!TryCreateOutputFile(outputFilePath))
			    {
				    return;
			    }

				RunCompiler(inputFilePath, inputFileText, outputFilePath, compileUpTo);
		    }
	    }

	    private static bool TryReadInputFile(string filePath, out string fileText)
	    {
		    fileText = null;

		    if (!File.Exists(filePath))
		    {
				Console.WriteLine($"The file at {0} does not exist.");
			    return false;
		    }

		    try
		    {
			    fileText = File.ReadAllText(filePath);
		    }
		    catch (Exception e)
		    {
			    Console.WriteLine("An exception was encountered when trying to read the input file:");
				Console.WriteLine($"\t{e.GetType().Name}");
				Console.WriteLine($"\t{e.Message}");
			    return false;
		    }

		    return true;
	    }

	    private static bool TryCreateOutputFile(string filePath)
	    {
		    try
		    {
			    File.Create(filePath);
		    }
		    catch (Exception e)
		    {
				Console.WriteLine("An exception was encountered when trying to create the output file:");
			    Console.WriteLine($"\t{e.GetType().Name}");
			    Console.WriteLine($"\t{e.Message}");
			    return false;
		    }

		    return true;
	    }

	    private static void RunCompiler(string inputFilePath, string inputText, string outputFilePath,
		    CompileUpTo compileUpTo)
	    {
		    return;
	    }
    }
}