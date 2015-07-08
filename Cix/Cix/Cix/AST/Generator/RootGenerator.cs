using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST.Generator
{
	/// <summary>
	/// Generates AST from a token list at the root level of the program, above all structs and functions.
	/// </summary>
	public sealed class RootGenerator
	{
		// Note: this variable is passed to sub-generators that rely on the enumerator not being reset to the beginning
		private TokenEnumerator tokens;
		private Dictionary<string, Element> nameTable;

		public RootGenerator(TokenEnumerator tokens)
		{
			if (tokens == null)
			{
				throw new ArgumentNullException("Token list must not be null");
			}

			this.tokens = tokens;

			// Add base types to the name table.
			this.nameTable = new Dictionary<string,Element>()
			{
				{"char", new DataType("char", 0, 1)},
				{"schar", new DataType("schar", 0, 1)},
				{"short", new DataType("short", 0, 2)},
				{"ushort", new DataType("ushort", 0, 2)},
				{"int", new DataType("int", 0, 4)},
				{"uint", new DataType("uint", 0, 4)},
				{"long", new DataType("long", 0, 8)},
				{"ulong", new DataType("ulong", 0, 8)},
				{"float", new DataType("float", 0, 4)},
				{"double", new DataType("double", 0, 8)},
				{"lpstring", new DataType("lpstring", 0, -1)},
				{"void", new DataType("void", 0, 0)}
			};
		}

		public List<Element> GenerateAST()
		{
			// At Root context, we expect either a structure header (i.e. struct MyStruct),
			// or a function header (i.e. void MyFunc). Structs always start with KeyStruct,
			// and functions always start with a type name or void (which we'll call a type
			// name for simplicity here). Additionally, ambiguous asterisks in this scope
			// are always pointer type indicators (i.e. int*).

		    List<Element> result = new List<Element>();

			do
			{
				Token current = this.tokens.Current;

				if (current.Type == TokenType.KeyStruct)
				{
					// Structure header found; generate the structure
				}
				else if (this.IsTypeName(current.Word))
				{
					// Function header found; generate a function
				}
				else
				{
					// PEBKAC found; berate the user
					throw new ArgumentException(string.Format("Invalid token {0} (word: \"{1}\") in root context", current.Type, current.Word));
				}
			} while (this.tokens.MoveNext());

			return result;
		}

		private bool IsTypeName(string word)
		{
			var matches = this.nameTable.Keys.Where(k => word.ToLowerInvariant() == k);
			return matches.Any(m => this.nameTable[m] is DataType);
		}
	}
}
