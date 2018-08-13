using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cix;
using Cix.AST;
using Cix.AST.Generator;
using Cix.AST.Generator.IntermediateForms;
using Cix.Errors;
using Cix.Exceptions;
using Cix.Parser;
using Cix.Text;

namespace CixFrontend
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			Console.WriteLine("Cix Platform");
			Console.WriteLine("Copyright © 2014-2018 Chris Akridge.");
			Console.WriteLine("Licensed under the MIT Public License.");

			if (args.Length < 1)
			{
				Console.WriteLine("Usage: cixfrontend <file-path> /flag1 /flag2 ...");
				Console.ReadKey();
				return;
			}

			string filePath = args[0];
			var compilation = new Compilation(filePath);

			try
			{
				compilation.LoadInputFile();
				compilation.RemoveComments();
				compilation.Preprocess();
				compilation.Lex();
				compilation.Tokenize();
				compilation.GenerateAST();
			}
			catch (ErrorsEncounteredException ex)
			{
				foreach (Error error in ex.Errors) { Console.WriteLine(error); }
				Console.ReadKey();
				return;
			}

			StringBuilder builder = new StringBuilder();
			foreach (var element in compilation.AbstractSyntaxTree)
			{
				element.Print(builder, 0);
			}

			Console.Write(builder.ToString());

			#if DEBUG
			Console.ReadKey();
			#endif
		}
	}
}
