using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cix;
using Cix.Exceptions;

namespace CixFrontend
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Cix Platform");
			Console.WriteLine("Copyright © 2014 Chris Akridge.");
			Console.WriteLine("Licensed under the MIT Public License.");

			if (args.Length < 1)
			{
				Console.WriteLine("Usage: cixfrontend <file-path> /flag1 /flag2 ...");
				Console.ReadKey();
				return;
			}

			string filePath = args[0];

			if (!File.Exists(filePath))
			{
				Console.WriteLine("File Not Found: The file at {0} does not exist.", filePath);
				Console.ReadKey();
				return;
			}

			string file = File.ReadAllText(filePath);

			Console.Write("Remove comments (C)/Preprocessed (P)/By character (B)/Tokenized (T) ");
			char option = char.ToLower((char)Console.Read());
			Console.WriteLine();

			if (option == 'c')
			{
				Console.WriteLine(file.RemoveComments());
			}
			else if (option == 'p')
			{
				Preprocessor preprocessor = new Preprocessor(file, filePath);
				//try
				//{
					foreach (string line in preprocessor.Preprocess().Split(new string[] { "\r", "\r\n"}, StringSplitOptions.RemoveEmptyEntries))
					{
						Console.WriteLine(line);
					}
				//}
				//catch (Exception ex)
				//{
				//	Console.WriteLine("{0}: {1}", ex.GetType().Name, ex.Message);
				//}
			}
			else if (option == 'b')
			{
				Lexer iterator = new Lexer(file.RemoveComments());
				try
				{
					foreach (string word in iterator.EnumerateWords())
					{
						Console.WriteLine(word);
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("{0}: {1} ({2})", ex.GetType().Name, ex.Message, ((ParseException)ex).ErrorLocation);
				}
			}
			else if (option == 't')
			{
				try
				{
					Tokenizer tokenizer = new Tokenizer();
					var tokenList = tokenizer.Tokenize(new Lexer(filePath).EnumerateWords());

					foreach (var token in tokenList)
					{
						Console.WriteLine("{0}: {1}", token.Type, token.Word);
					}
				}
				catch (Exception ex)
				{
					if (ex is ParseException)
					{
						Console.WriteLine("Parse exception: {0} ({1})", ex.Message, ((ParseException)ex).ErrorLocation);
					}
					else if (ex is TokenException)
					{
						Console.WriteLine("Token exception: {0}", ex.Message);
					}
					else
					{
						Console.Write("{0}: {1}", ex.GetType().Name, ex.Message);
					}
				}
			}
			Console.ReadKey();
		}
	}
}
