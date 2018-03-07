using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cix;
using Cix.AST;
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

			Console.Write("Remove comments (C)/Preprocessed (P)/By character (L)/Tokenized (T)/First stage (F)/AST First Pass Stage A (A)/Stage B (B)/Stage C (D)/Stage D (E)/Full First Stage AST Generation (1)");
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
			else if (option == 'l')
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
			else if (option == 'f')
			{
				//try
				//{
					file = file.RemoveComments();

					Preprocessor preprocessor = new Preprocessor(file, filePath);
					file = preprocessor.Preprocess();

					var words = new Lexer(file).EnumerateWords();
					var tokenList = new Tokenizer().Tokenize(words);

					foreach (var token in tokenList)
					{
						Console.WriteLine("{0}: {1}", token.Type, token.Word);
					}
				//}
				//catch (Exception ex)
				//{
				//	Console.WriteLine("{0}: {1}", ex.GetType().Name, ex.Message);
				//}
			}
			else if (option == 'a')
			{
				file = file.RemoveComments();

				Preprocessor preprocessor = new Preprocessor(file, filePath);
				file = preprocessor.Preprocess();

				var words = new Lexer(file).EnumerateWords();
				var tokenList = new Tokenizer().Tokenize(words);
				var intermediateStructs = new Cix.AST.Generator.FirstPassGenerator(new TokenEnumerator(tokenList)).GenerateIntermediateStructs();
				
				foreach (var intermediateStruct in intermediateStructs)
				{
					Console.WriteLine($"Struct named {intermediateStruct.Name}, starting at index {intermediateStruct.FirstDefinitionTokenIndex}, ending at index {intermediateStruct.LastTokenIndex}.");
				}
			}
			else if (option == 'b')
			{
				file = file.RemoveComments();

				Preprocessor preprocessor = new Preprocessor(file, filePath);
				file = preprocessor.Preprocess();

				var words = new Lexer(file).EnumerateWords();
				var tokenList = new Tokenizer().Tokenize(words);
				var intermediateStructs = new Cix.AST.Generator.FirstPassGenerator(new TokenEnumerator(tokenList)).GenerateIntermediateStructs();
				var structs = new Cix.AST.Generator.FirstPassGenerator(new TokenEnumerator(tokenList)).GenerateStructTree(intermediateStructs);

				foreach (StructDeclaration structDeclaration in structs)
				{
					Console.WriteLine($"Struct named {structDeclaration.Name} with size {structDeclaration.Size} ({structDeclaration.Members.Count} members):");
					foreach (var member in structDeclaration.Members)
					{
						Console.WriteLine($"\tMember of type {member.Type.TypeName}{new string('*', member.Type.PointerLevel)}");
						Console.WriteLine($"\tNamed {member.Name} with array size {member.ArraySize}.");
						Console.WriteLine($"\tOverall size {member.Size} bytes, offset +{member.Offset}.");
						Console.WriteLine();
					}
				}
			}
			else if (option == 'd')
			{
				file = file.RemoveComments();
				Preprocessor preprocesor = new Preprocessor(file, filePath);
				file = preprocesor.Preprocess();

				var words = new Lexer(file).EnumerateWords();
				var tokenList = new Tokenizer().Tokenize(words);
				var intermediateStructs = new Cix.AST.Generator.FirstPassGenerator(new TokenEnumerator(tokenList)).GenerateIntermediateStructs();
				var structs = new Cix.AST.Generator.FirstPassGenerator(new TokenEnumerator(tokenList)).GenerateStructTree(intermediateStructs);
				var treeWithGlobals = new Cix.AST.Generator.FirstPassGenerator(new TokenEnumerator(tokenList)).AddGlobalsToTree(structs);

				foreach (GlobalVariableDeclaration global in treeWithGlobals.Where(e => e is GlobalVariableDeclaration))
				{
					Console.WriteLine($"Global variable named {global.Name} with type {global.Type.TypeName} {((global.InitialValue != null) ? "with" : "without")} init");
				}
			}
			else if (option == 'e')
			{
				file = file.RemoveComments();
				Preprocessor preprocessor = new Preprocessor(file, filePath);
				file = preprocessor.Preprocess();

				var words = new Lexer(file).EnumerateWords();
				var tokenList = new Tokenizer().Tokenize(words);
				var generator = new Cix.AST.Generator.FirstPassGenerator(new TokenEnumerator(tokenList));
				var intermediateStructs = generator.GenerateIntermediateStructs();
				var structs = generator.GenerateStructTree(intermediateStructs);
				var treeWithGlobals = generator.AddGlobalsToTree(structs);

				try
				{
					var intermediateFunctions = generator.GenerateIntermediateFunctions();
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			}
			else if (option == '1')
			{
				file = file.RemoveComments();
				file = new Preprocessor(file, filePath).Preprocess();

				var words = new Lexer(file).EnumerateWords();
				var tokenList = new Tokenizer().Tokenize(words);
				var generator = new Cix.AST.Generator.FirstPassGenerator(new TokenEnumerator(tokenList));

				try
				{
					generator.GenerateFirstPassAST();
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			}
			Console.ReadKey();
		}
	}
}
