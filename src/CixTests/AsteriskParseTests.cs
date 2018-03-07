using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cix;
using Cix.AST;
using Cix.AST.Generator;
using Cix.AST.Generator.IntermediateForms;

namespace CixTests
{
	[TestClass()]
	public class AsteriskParseTests
	{
		private const string TestCaseName = "AsteriskTokenParse";
		private string testCasePath;
		private string testCaseFile;

		private bool cachesCreated = false;

		private List<string> lexedWordCache;
		private List<Token> tokenCache;
		private List<Element> tree;
		private List<IntermediateFunction> functions;

		private void CreateCaches()
		{
			if (cachesCreated) { return; }

			testCasePath = Utilities.GetTestCasePath(TestCaseName);
			testCaseFile = System.IO.File.ReadAllText(testCasePath);

			testCaseFile = testCaseFile.RemoveComments();

			Preprocessor preprocessor = new Preprocessor(testCaseFile, testCasePath);
			testCaseFile = preprocessor.Preprocess();

			Lexer lexer = new Lexer(testCaseFile);
			lexedWordCache = lexer.EnumerateWords();

			Tokenizer tokenizer = new Tokenizer();
			tokenCache = tokenizer.Tokenize(lexedWordCache);

			FirstPassGenerator generator = new FirstPassGenerator(new TokenEnumerator(tokenCache));
			var structs = generator.GenerateIntermediateStructs();
			tree = generator.GenerateStructTree(structs);
			tree = generator.AddGlobalsToTree(tree);
			functions = generator.GenerateIntermediateFunctions();

			cachesCreated = true;
		}

		[TestMethod()]
		public void AsterisksInWordsOfLengthOne()
		{
			CreateCaches();

			var wordsWithAsterisks = lexedWordCache.Where(w => w.Contains('*'));

			// Validate that all words with asterisks are of length 1 and have only asterisks
			foreach (string word in wordsWithAsterisks)
			{
				// A word with length 1 that has asterisks will only contain an asterisk
				if (word.Length > 1) { Assert.Fail($"Word {word} is too long (length must be 1)"); }
			}
		}

		[TestMethod()]
		public void AsterisksLexedIntoSeparateWords()
		{
			CreateCaches();

			// In the test case, globals _3 and _4, s._7, s._8, and f3(int**) and f4(int**) all have
			// multiple asterisks in sequence. The lexer doesn't care about structure like this, so
			// we just need to check _3.
			int indexOf_3 = lexedWordCache.IndexOf("_3");
			// Go back three tokens to "int"
			int indexOfInt = indexOf_3 - 3;

			if (lexedWordCache[indexOfInt] != "int") { Assert.Fail($"Word 1:{lexedWordCache[indexOfInt]}"); }
			if (lexedWordCache[indexOfInt + 1] != "*") { Assert.Fail($"Word 2: {lexedWordCache[indexOfInt+1]}"); }
			if (lexedWordCache[indexOfInt + 2] != "*") { Assert.Fail($"Word 3: {lexedWordCache[indexOfInt+2]}"); }
		}

		[TestMethod()]
		public void SingleAsteriskTokenizedSeparately()
		{
			CreateCaches();

			// Find _1.
			int indexOf_1 = tokenCache.IndexOf(tokenCache.First(t => t.Word == "_1"));

			// Go back two tokens.
			int indexOfInt = indexOf_1 - 2;

			// This word should be int.
			Token t1 = tokenCache[indexOfInt];
			if (t1.Word != "int") { Assert.Fail($"Token 1: {t1.ToString()}"); }

			// The next word should be *.
			Token t2 = tokenCache[indexOfInt + 1];
			if (t2.Word != "*") { Assert.Fail($"Token 2: {t2.ToString()}"); }

			// The next word should be _1. We already found it per the IndexOf call above, so we
			// don't need to test this.
		}

		[TestMethod()]
		public void DoubleAsteriskTokenizedSeparately()
		{
			CreateCaches();

			// Find _3.
			int indexOf_3 = tokenCache.IndexOf(tokenCache.Find(t => t.Word == "_3"));

			// Go back three tokens. The tokens should be { int, *, *, _3 }.
			int indexOfInt = indexOf_3 - 3;

			Token t1 = tokenCache[indexOfInt];
			if (t1.Word != "int") { Assert.Fail($"Token 1: {t1.ToString()}"); }

			Token t2 = tokenCache[indexOfInt + 1];
			if (t2.Word != "*") { Assert.Fail($"Token 2: {t2.ToString()}"); }

			Token t3 = tokenCache[indexOfInt + 2];
			if (t3.Word != "*") { Assert.Fail($"Token 3: {t3.ToString()}"); }

			Token t4 = tokenCache[indexOfInt + 3];
			if (t4.Word != "_3") { Assert.Fail($"Token 4: {t4.ToString()}"); }
		}

		[TestMethod()]
		public void TestCaseStructValid()
		{
			CreateCaches();
			// The struct definition in the test case is duplicated below:
			// struct s
			// {
			//	int* _5;
			//	int* _6;
			//	int** _7;
			//	int** _8;
			// }

			Func<Element, bool> predicate = e => e is StructDeclaration && ((StructDeclaration)e).Name == "s";
			if (!tree.Any(predicate))
			{
				Assert.Fail($"No struct named \"s\" exists.");
			}

			var s = (StructDeclaration)tree.First(t => ((StructDeclaration)t).Name == "s");

			if (s.Members.Count != 4) { Assert.Fail($"Struct has {s.Members.Count} member(s), 4 expected"); }

			var _5 = s.Members[0];
			var _6 = s.Members[1];
			var _7 = s.Members[2];
			var _8 = s.Members[3];

			if (_5.Type.PointerLevel != 1) { Assert.Fail($"s._5 is an int*, but has pointer level {_5.Type.PointerLevel}."); }
			if (_6.Type.PointerLevel != 1) { Assert.Fail($"s._6 is an int*, but has pointer level {_6.Type.PointerLevel}."); }
			if (_7.Type.PointerLevel != 2) { Assert.Fail($"s._7 is an int**, but has pointer level {_7.Type.PointerLevel}."); }
			if (_8.Type.PointerLevel != 2) { Assert.Fail($"s._8 is an int**, but has pointer level {_8.Type.PointerLevel}."); }
		}

		[TestMethod()]
		public void TestCaseGlobalsValid()
		{
			CreateCaches();

			int globalCount = tree.Count(e => e is GlobalVariableDeclaration);
			if (globalCount != 4) { Assert.Fail($"Expected 4 globals, got {globalCount}"); }

			var globals = tree.Where(e => e is GlobalVariableDeclaration).Cast<GlobalVariableDeclaration>();

			var _1 = globals.First(g => g.Name == "_1");
			var _2 = globals.First(g => g.Name == "_2");
			var _3 = globals.First(g => g.Name == "_3");
			var _4 = globals.First(g => g.Name == "_4");

			if (_1.Type.PointerLevel != 1) { Assert.Fail($"_1 is an int* but has pointer level {_1.Type.PointerLevel}"); }
			if (_2.Type.PointerLevel != 1) { Assert.Fail($"_2 is an int* but has pointer level {_2.Type.PointerLevel}"); }
			if (_3.Type.PointerLevel != 2) { Assert.Fail($"_3 is an int** but has pointer level {_3.Type.PointerLevel}"); }
			if (_4.Type.PointerLevel != 2) { Assert.Fail($"_4 is an int** but has pointer level {_4.Type.PointerLevel}"); }
		}

		[TestMethod()]
		public void TestCaseFunctionsValid()
		{
			CreateCaches();

			if (functions.Count != 4) { Assert.Fail($"Expected 4 functions, got {functions.Count}"); }

			var f1 = functions[0];
			var f2 = functions[1];
			var f3 = functions[2];
			var f4 = functions[3];

			// f1 takes int* and returns int*
			if (f1.ReturnType.PointerLevel != 1) { Assert.Fail($"f1 returns an int*, but the pointer level is {f1.ReturnType.PointerLevel}"); }
			if (f1.Arguments[0].Type.PointerLevel != 1)
			{
				Assert.Fail($"f1's a1 argument is int*, but has a pointer level of {f1.Arguments[0].Type.PointerLevel}");
			}

			// f2 takes int* and returns int*
			if (f2.ReturnType.PointerLevel != 1) { Assert.Fail($"f2 returns an int*, but the pointer level is {f2.ReturnType.PointerLevel}"); }
			if (f2.Arguments[0].Type.PointerLevel != 1)
			{
				Assert.Fail($"f2's a2 argument is int*, but has a pointer level of {f2.Arguments[0].Type.PointerLevel}");
			}

			// f3 takes int** and returns int**
			if (f3.ReturnType.PointerLevel != 2) { Assert.Fail($"f3 returns an int**, but the pointer level is {f3.ReturnType.PointerLevel}"); }
			if (f3.Arguments[0].Type.PointerLevel != 2)
			{
				Assert.Fail($"f3's a3 argument is int**, but has a pointer level of {f3.Arguments[0].Type.PointerLevel}");
			}

			// f4 takes int** and returns int**
			if (f4.ReturnType.PointerLevel != 2) { Assert.Fail($"f4 returns an int**, but the pointer level is {f4.ReturnType.PointerLevel}"); }
			if (f4.Arguments[0].Type.PointerLevel != 2)
			{
				Assert.Fail($"f4's a4 argument is int**, but has a pointer level of {f4.Arguments[0].Type.PointerLevel}");
			}
		}
	}
}
