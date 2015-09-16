using System;
using System.Collections.Generic;
using System.Linq;
using Cix.Exceptions;

namespace Cix.AST.Generator
{
	public sealed class FirstPassGenerator
	{
		private Dictionary<string, Element> nameTable;
		private TokenEnumerator tokens;
		private List<string> unresolvedTypeNames = new List<string>();

		public FirstPassGenerator(List<Token> cTokens, Dictionary<string, Element> cNameTable)
		{
			if (cTokens == null) throw new ArgumentNullException(nameof(cTokens), "The provided token list was null.");
			if (cTokens.Count == 0) throw new ArgumentException(nameof(cTokens), "The provided token list was empty.");
			if (cNameTable == null) throw new ArgumentNullException(nameof(cNameTable), "The provided name table was null.");

			tokens = new TokenEnumerator(cTokens);
			nameTable = cNameTable;

			nameTable.Add("byte", new DataType("byte", 0, 1));
			nameTable.Add("sbyte", new DataType("sbyte", 0, 1));
			nameTable.Add("short", new DataType("short", 0, 2));
			nameTable.Add("ushort", new DataType("ushort", 0, 2));
			nameTable.Add("char", new DataType("char", 0, 2));
			nameTable.Add("int", new DataType("int", 0, 4));
			nameTable.Add("uint", new DataType("uint", 0, 4));
			nameTable.Add("float", new DataType("float", 0, 4));
			nameTable.Add("long", new DataType("long", 0, 8));
			nameTable.Add("ulong", new DataType("ulong", 0, 8));
			nameTable.Add("double", new DataType("double", 0, 8));
			nameTable.Add("void", new DataType("void", 0, -1));
		}

		public IEnumerable<Element> GenerateAST()
		{
			List<Element> result = new List<Element>();

			while (!tokens.AtEnd)
			{
				Token token = tokens.Current;

				if (token.Type == TokenType.KeyStruct)
				{
					// Structure found, enter the struct parser
				}
				else if (token.Type == TokenType.Identifier)
				{
					// Function found
				}
			}

			return result;
		}

		private void GenerateStructureAST(List<Element> tree)
		{
			// Move to the next token to read the name.
			tokens.MoveNextValidate(TokenType.Identifier);
			string structName = tokens.Current.Word;
			string memberName = null;
			uint memberArraySize = 0;
			int currentOffset = 0;

			// Create the structure instance.
			StructDeclaration structure = new StructDeclaration(structName, new List<StructMemberDeclaration>());

			// Expect an openscope.
			tokens.MoveNextValidate(TokenType.OpenScope);

			while (tokens.Current.Type != TokenType.CloseScope)
			{
				// Expect an identifier for the struct member type name.
				tokens.MoveNextValidate(TokenType.Identifier);
				string typeName = tokens.Current.Word;
				int typePointerLevel = typeName.Count(c => c == '*');
				typeName = typeName.Substring(0, typeName.Length - typePointerLevel - 1);   // Remove the asterisks from the type name.

				// Expect either an indentifier (member name) or indeterminate asterisk.
				tokens.MoveNext();
				if (tokens.Current.Type == TokenType.Indeterminate)
				{
					if (typePointerLevel > 0)
					{
						// Too many pointer declarations (int*** ** i;) is technically legal in ISO C but it's not legal here because it's bad style
						throw new ASTException($"Within struct {structName} member #{structure.Members.Count}: Too many separate pointer definitions (some asterisks separated by whitespace). Remove whitespace or asterisks.");
					}

					typePointerLevel = tokens.Current.Word.Length;

					// Expect an identifier (member name).
					tokens.MoveNextValidate(TokenType.Identifier);
					memberName = tokens.Current.Word;
				}
				else if (tokens.Current.Type == TokenType.Identifier)
				{
					memberName = tokens.Current.Word;
				}
				else
				{
					throw new ASTException($"Within struct {structName} member #{structure.Members.Count}: Invalid token, expected identifier or asterisks, got {tokens.Current.Type}.");
				}

				// Expect either a semicolon (end member declaration) or an OpenBracket (define this member as an array).
				tokens.MoveNext();
				if (tokens.Current.Type == TokenType.Semicolon)
				{
					goto createMemberDefintion;
				}
				else if (tokens.Current.Type == TokenType.OpenBracket)
				{
					// The array's size is defined by a 32-bit unsigned integer.
					// Read and expect the next token to be a literal.
					tokens.MoveNextValidate(TokenType.Identifier);
					if (tokens.Current.Word.IsNumericLiteral())
					{
						NumericLiteral literal = NumericLiteral.Parse(tokens.Current.Word);

						if (literal.UnsignedIntegralValue > Int32.MaxValue)
						{
							throw new ASTException($"StructArrayMemberSizeOutOfRange: The {memberName} member of the {structName} structure has a size of {literal.UnsignedIntegralValue}, which is far too large. Maximum value is {int.MaxValue}.");
						}

						memberArraySize = (uint)literal.UnsignedIntegralValue;

						// Now we're expecting a close bracket and semicolon.
						tokens.MoveNextValidate(TokenType.CloseBracket);
						tokens.MoveNextValidate(TokenType.Semicolon);
					}
					else
					{
						throw new ASTException($"StructArrayMemberInvalidSize: The {memberName} member of the {structName} structure had a size that wasn't a number, but \"{tokens.Current.Word}\".");
                    }
				}

				createMemberDefintion:
				structure.Members.Add(new StructMemberDeclaration(GetTypeByName(typeName), memberName, (int)memberArraySize, currentOffset));
				currentOffset += GetTypeByName(typeName).TypeSize;
			}

			tree.Add(structure);
		}

		private DataType GetTypeByName(string name)
		{
			if (!nameTable.ContainsKey(name) || !(nameTable[name] is DataType))
			{
				throw new ASTException();
			}

			return (DataType)nameTable[name];
		}
	}
}
