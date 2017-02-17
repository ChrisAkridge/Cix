using System;
using System.Collections.Generic;
using System.Linq;
using Cix.Exceptions;
using Cix.AST.Generator.IntermediateForms;

namespace Cix.AST.Generator
{
	public sealed class FirstPassGenerator
	{
		private TokenEnumerator tokens;

		public FirstPassGenerator(TokenEnumerator cTokens)
		{
			tokens = cTokens;
		}

		public List<IntermediateStruct> StageAGenerator()
		{
			// Take every struct header into an intermediate definition of
			// { Name, FirstDefinitionTokenIndex, LastTokenIndex }, the
			// last of which is the index of the final semicolon, NOT the
			// closescope.

			List<IntermediateStruct> result = new List<IntermediateStruct>();

			// ensure we're at the beginning of all the code
			tokens.Reset();

			while (!tokens.AtEnd)
			{
				if (tokens.Current.Type != TokenType.KeyStruct)
				{
					// Skip over everything that isn't a structure header.
					tokens.MoveNext();
					continue;
				}

				tokens.MoveNextValidate(TokenType.Identifier);
				IntermediateStruct newStruct = new IntermediateStruct(tokens.Current.Word);
				tokens.MoveNextValidate(TokenType.OpenScope);
				newStruct.FirstDefinitionTokenIndex = tokens.CurrentIndex + 1;
				tokens.SkipBlock();
				newStruct.LastTokenIndex = tokens.CurrentIndex - 2;
				result.Add(newStruct);
			}

			return result;
		}

		public List<Element> StageBGenerator(List<IntermediateStruct> intermediateStructs)
		{
			// Part 1: Get intermediate forms for all the struct members where types are just named and pointer levels are considered.
			// Here, we'll ensure that void- and lpstring-typed members may only appear as pointer members.

			foreach (var intermediateStruct in intermediateStructs)
			{
				ParseIntermediateStructMembers(intermediateStruct);
				NameTable.Instance[intermediateStruct.Name] = intermediateStruct;
			}

			// Ensure that all structs have unique names.
			if (intermediateStructs.Select(s => s.Name).Count() != intermediateStructs.Select(s => s.Name).Distinct().Count())
			{
				throw new ASTException("Some structs have the same name.");
			}

			// Part 2: Create the fully-formed definitions for all structs.
			// Loop through every intermediate struct and define all its member. If a member is a struct that isn't defined yet, define it.
			List<Element> tree = new List<Element>();

			foreach (var intermediateStruct in intermediateStructs)
			{
				if (NameTable.Instance[intermediateStruct.Name] is IntermediateStruct)
				{
					int depthLevel = 0;
					CreateStructDeclaration(intermediateStruct, ref depthLevel);
				}
			}

			tree.AddRange(NameTable.Instance.Names.Where(n => n.Value is StructDeclaration).Select(kvp => kvp.Value));
			return tree;
		}

		/// <summary>
		/// Parses the token stream to find global variables and add them to the tree.
		/// </summary>
		/// <param name="tree">The abstract syntax tree immediately after stage B generation.</param>
		/// <returns>The abstract syntax tree containing global variable declarations plus stage B generation.</returns>
		public List<Element> StageCGenerator(List<Element> tree)
		{
			tokens.Reset();
			var globalVariables = new List<GlobalVariableDeclaration>();

			// Scan through all tokens at the root level, looking for the global keyword.
			int nestingDepth = 0;
			do
			{
				if (tokens.Current.Type == TokenType.OpenScope)
				{
					nestingDepth++;
				}
				else if (tokens.Current.Type == TokenType.CloseScope)
				{
					nestingDepth--;
				}
				else if (tokens.Current.Word == "global" && nestingDepth == 0)
				{
					var globalStatement = tokens.MoveStatement();
					globalVariables.Add(GetGlobalVariableDeclaration(globalStatement));
				}
				tokens.MoveNext();
			} while (!tokens.AtEnd);

			tree.AddRange(globalVariables);
			return tree;
		}

		/// <summary>
		/// Parses the token stream to find function headers and their start/end token indices.
		/// </summary>
		/// <param name="tree">A tree containing everything up to the stage C generation.</param>
		/// <returns>A tree with the function information.</returns>
		public List<IntermediateFunction> StageDGenerator()
		{
			tokens.Reset();
			var functions = new List<IntermediateFunction>();

			// Scan through all the tokens at the root level.
			// Functions are the only root element that doesn't start with "struct" or "global".
			int nestingDepth = 0;
			do
			{
				if (tokens.Current.Type == TokenType.OpenScope)
				{
					nestingDepth++;
				}
				else if (tokens.Current.Type == TokenType.CloseScope)
				{
					nestingDepth--;
				}
				else if (tokens.Current.Type == TokenType.KeyStruct)
				{
					// Skip this entire struct definition.
					tokens.SkipBlock();
					continue;
				}
				else if (tokens.Current.Word == "global")
				{
					tokens.MoveStatement();
				}
				else if (nestingDepth == 0 && tokens.Current.Type != TokenType.KeyStruct &&
					tokens.Current.Word != "global")
				{
					// We've found a type name! Probably! Let's find it in the nametable before we
					// get too excited.
					string possibleTypeName = tokens.Current.Word;
					if (!NameTable.Instance.Names.ContainsKey(possibleTypeName.TrimAsterisks()))
					{
						throw new ASTException($"A function was declared with return type {possibleTypeName}. No type by that name exists.");
					}

					// This is a proper function. Let's get started!
					// Remember that the return type can appear as both "type* name" and "type * name",
					// so we'll have to check for that.
					DataType returnType = GetDataTypeFromNameTable(possibleTypeName);
					tokens.MoveNext();

					if (tokens.Current.Type == TokenType.Indeterminate)
					{
						returnType = returnType.WithPointerLevel(tokens.Current.Word.Length);
						tokens.MoveNext();
					}

					string name = tokens.Current.Word;
					if (!name.IsIdentifier())
					{
						throw new ASTException($"The function name \"{name}\" is not valid.");
					}

					// Up next should be the leftparen for the function argument list.
					tokens.MoveNext();
					if (tokens.Current.Type != TokenType.OpenParen)
					{
						throw new ASTException($"Expected a left paren, found {tokens.Current.Word}");
					}
					int argsStartIndex = tokens.CurrentIndex;
					int argsEndIndex = argsStartIndex;

					// Find the close parentheses. No function argument can have parentheses in it,
					// so we don't have to worry about nesting.
					while (tokens.Current.Type != TokenType.CloseParen)
					{
						tokens.MoveNext();
						argsEndIndex++;
					}

					// The above loop stops just before the closeparen. Add 1 to move it to the
					// closeparen.
					argsEndIndex += 1;
					//tokens.MoveNext();

					// Parse the function arguments.
					var functionArguments = ParseFunctionArguments(argsStartIndex, argsEndIndex);

					// The openscope should be the next token.
					if (!tokens.MoveNextValidate(TokenType.OpenScope))
					{
						throw new ASTException($"Unexpected token \"{tokens.Current.Word}\" between function arguments and function statements.");
					}

					int openScopeIndex = tokens.CurrentIndex;
					int closeScopeIndex = -1;
					int functionNestingDepth = 1;

					while (true)
					{
						if (!tokens.MoveNext()) { throw new ASTException($"The function {name} is missing a }}."); }
						if (tokens.Current.Type == TokenType.OpenScope) { functionNestingDepth++; }
						else if (tokens.Current.Type == TokenType.CloseScope)
						{
							if (functionNestingDepth > 1) { functionNestingDepth--; }
							else
							{
								closeScopeIndex = tokens.CurrentIndex;
								break;
							}
						}
					}

					functions.Add(new IntermediateFunction(returnType, name, functionArguments, openScopeIndex, closeScopeIndex));
				}

				tokens.MoveNext();
			} while (!tokens.AtEnd);

			return functions;
		}

		private void ParseIntermediateStructMembers(IntermediateStruct intermediateStruct)
		{
			var statements = tokens.Subset(intermediateStruct.FirstDefinitionTokenIndex, intermediateStruct.LastTokenIndex).SplitOnSemicolon();

			foreach (var statement in statements)
			{
				// Valid member definitions:
				// int i;
				// void* pv;
				// byte** ppb;
				// int array[50];
				// short* p_array[25];

				string memberTypeName = "";
				int memberPointerLevel = 0;
				string memberName = "";
				int memberArraySize = 0;

				var enumerator = statement.GetEnumerator();
				enumerator.MoveNext();	// start with the first item

				if (enumerator.Current.Word.Contains("*"))
				{
					memberTypeName = enumerator.Current.Word.Remove(enumerator.Current.Word.IndexOf('*'));
					memberPointerLevel = enumerator.Current.Word.Count(c => c == '*');
				}
				else
				{
					if (enumerator.Current.Word == "void" || enumerator.Current.Word == "lpstring")
					{
						throw new ASTException("Members of type void or lpstring may not appear in structs. Use a pointer to that type instead.");
					}
					memberTypeName = enumerator.Current.Word;
				}

				enumerator.MoveNext(); // next up is the member name...

				if (enumerator.Current.Type == TokenType.Indeterminate)
				{
					memberPointerLevel = enumerator.Current.Word.Length; // ...usually
					enumerator.MoveNext();
				}

				if (enumerator.Current.Type != TokenType.Identifier) { throw new ASTException($"Expected member name, not a {enumerator.Current.Type}."); }
				memberName = enumerator.Current.Word;

				if (enumerator.MoveNext() && enumerator.Current.Type == TokenType.OpenBracket)
				{
					// This is an array member.
					if (!enumerator.MoveNext() || enumerator.Current.Type != TokenType.Identifier) { throw new ASTException("Expected number of items."); }
					NumericLiteral arraySizeLiteral = NumericLiteral.Parse(enumerator.Current.Word);
					if (arraySizeLiteral.UnderlyingType != typeof(int)) { throw new ASTException($"Invalid array size {enumerator.Current.Word}."); }
					memberArraySize = (int)arraySizeLiteral.SignedIntegralValue;

					if (!enumerator.MoveNext() || enumerator.Current.Type != TokenType.CloseBracket) { throw new ASTException("Expected close bracket."); }
				}
				else
				{
					// This is not an array member, but...
					memberArraySize = 1;
					// ...it still acts like a one-member array.
				}

				intermediateStruct.Members.Add(new IntermediateStructMember(memberTypeName, memberName, memberPointerLevel, memberArraySize));
			}
		}

		private StructDeclaration CreateStructDeclaration(IntermediateStruct intermediateStruct, ref int depthLevel)
		{
			string structName = intermediateStruct.Name;
			List<StructMemberDeclaration> members = new List<StructMemberDeclaration>();
			int offsetCounter = 0;
			
			foreach (var intermediateMember in intermediateStruct.Members)
			{
				// Resolve the type.
				var typeEntry = NameTable.Instance[intermediateMember.TypeName]; // TODO: if name not found, throw
				if (typeEntry is DataType)
				{
					// Scenario 1: Member has a primitive type or is a pointer to a primitive type
					DataType baseType = (DataType)typeEntry;
					DataType fullType = new DataType(baseType.TypeName, intermediateMember.PointerLevel, baseType.TypeSize);
					StructMemberDeclaration member = new StructMemberDeclaration(fullType, intermediateMember.Name, intermediateMember.ArraySize, offsetCounter);
					members.Add(member);
					offsetCounter += member.MemberType.TypeSize * member.ArraySize;
				}
				else if (typeEntry is StructDeclaration)
				{
					// Scenario 2: Member has a struct type or is a pointer to a struct type
					StructDeclaration baseType = (StructDeclaration)typeEntry;
					members.Add(GetDeclarationOfStructMember(baseType, intermediateMember, ref offsetCounter));
				}
				else if (typeEntry is IntermediateStruct)
				{
					// Scenario 3: Member has a struct type which is not fully defined
					depthLevel++;

					if (depthLevel > 100)
					{
						// why is the user nesting 100 structs
						throw new ASTException("Too many nested struct members. Consider refactoring or look for circular struct members.");
					}

					StructDeclaration newlyDefinedStruct = CreateStructDeclaration((IntermediateStruct)typeEntry, ref depthLevel);
					members.Add(GetDeclarationOfStructMember(newlyDefinedStruct, intermediateMember, ref offsetCounter));
				}
				else if (typeEntry == null && intermediateMember.Name == "void" && intermediateMember.PointerLevel > 0)
				{
					// Scenario 4: Pointer to void
					StructMemberDeclaration member = new StructMemberDeclaration(new DataType("void", intermediateMember.PointerLevel, 8), intermediateMember.Name, intermediateMember.ArraySize, offsetCounter);
					members.Add(member);
				}
			}

			StructDeclaration result = new StructDeclaration(intermediateStruct.Name, members);
			NameTable.Instance[intermediateStruct.Name] = result;
			return result;
		}

		private StructMemberDeclaration GetDeclarationOfStructMember(StructDeclaration memberType, IntermediateStructMember intermediateMember, ref int offsetCounter)
		{
			DataType fullType = new DataType(memberType.Name, intermediateMember.PointerLevel, memberType.Size);
			StructMemberDeclaration result = new StructMemberDeclaration(fullType, intermediateMember.Name, intermediateMember.ArraySize, offsetCounter);
			offsetCounter += result.MemberType.TypeSize * result.ArraySize;
			return result;
		}

		private GlobalVariableDeclaration GetGlobalVariableDeclaration(List<Token> globalStatement)
		{
			// Valid global variable definitions
			//  global T name;
			//  global U primitive = 137;
			//  global T* ptr;

			int pointerLevel = 0;

			if (globalStatement.Any(t => t.Type == TokenType.Indeterminate))
			{
				Token indeterminate = globalStatement.First(t => t.Type == TokenType.Indeterminate);
				pointerLevel = indeterminate.Word.Length;
				globalStatement.RemoveAt(globalStatement.IndexOf(indeterminate));
			}

			var typeData = globalStatement[1].Word.SeparateTypeNameAndPointerLevel();
			string typeName = typeData.Item1;
			pointerLevel = (pointerLevel == 0) ? typeData.Item2 : pointerLevel;

			string variableName = globalStatement[2].Word;
			NumericLiteral numericLiteral = null;

			if (globalStatement.Any(t => t.Word == "=") && NameTable.IsPrimitiveType(typeName))
			{
				string numericLiteralToken = globalStatement[4].Word;
				numericLiteral = NumericLiteral.Parse(numericLiteralToken);
			}

			if (typeName == "void" && pointerLevel == 0) { throw new ASTException("No global variable may have the type of void."); }
			else if (!(NameTable.Instance[typeName] is DataType) && !(NameTable.Instance[typeName] is StructMemberDeclaration) && typeName != "void")
				{ throw new ASTException($"{typeName} is not a type; it's a {NameTable.Instance[typeName].GetType().Name}."); }
			else if (!NameTable.Instance.Names.ContainsKey(typeName) && typeName != "void") { throw new ASTException($"Type {typeName} for global variable doesn't exist."); }
			else if (typeName == "lpstring") { throw new ASTException("No global variable may directly have the type of lpstring. Consider using byte* instead."); }

			int typeSize = (pointerLevel == 0) ? ((DataType)NameTable.Instance[typeName]).TypeSize : 8;
			return new GlobalVariableDeclaration(new DataType(typeName, pointerLevel, typeSize), variableName, numericLiteral);
		}

		private DataType GetDataTypeFromNameTable(string typeName)
		{
			var typeNameWithPointerLevel = typeName.SeparateTypeNameAndPointerLevel();
			var typeInNameTable = NameTable.Instance[typeNameWithPointerLevel.Item1];

			if (typeInNameTable == null)
			{
				throw new ASTException($"Tried to find a type named \"{typeName}\", but that name isn't defined.");
			}

			if (typeInNameTable is DataType)
			{
				return ((DataType)typeInNameTable).WithPointerLevel(typeNameWithPointerLevel.Item2);
			}
			else if (typeInNameTable is StructDeclaration)
			{
				return ((StructDeclaration)typeInNameTable).ToDataType()
					.WithPointerLevel(typeNameWithPointerLevel.Item2);
			}
			else
			{
				throw new ASTException($"Tried to find a type named \"{typeName}\", but that name defines a {typeInNameTable.GetType().Name}");
			}
		}

		private List<FunctionArgument> ParseFunctionArguments(int startIndex, int endIndex)
		{
			// Start index and end index should point to the openparen and closeparen, respectively
			// Add 1 to startIndex and subtract 1 from endIndex to point them to the first and last
			// actual tokens in the function arguments.
			var argTokens = tokens.Subset(startIndex + 1, endIndex - 1);
			var result = new List<FunctionArgument>();

			// Easy case: if the closeparen is right after the openparen, there are no arguments.
			if (startIndex + 1 == endIndex) { return result; }

			var args = argTokens.SplitOnComma();

			// A function argument is either "type name" or "type* name" or "type * name"
			// (with however many asterisks, of course)

			foreach (var arg in args)
			{
				if (arg.Count < 2 || arg.Count > 3)
				{
					throw new ASTException($"There are {arg.Count} token(s) in a function argument. There should be 2 or 3.");
				}

				Token typeNameToken = arg[0];
				Token argNameToken = arg.Last();
				Token separatePointerToken = (arg.Count == 3) ? arg[1] : null;

				// Look up the type name in the name table to check if it's real.
				string typeName = typeNameToken.Word.TrimAsterisks();
				if (!NameTable.Instance.Names.ContainsKey(typeName))
				{
					throw new ASTException($"A function argument was declared with return type {typeName}. No type by that name exists.");
				}

				string argName = argNameToken.Word;
				if (!argName.IsIdentifier())
				{
					throw new ASTException($"A function argument had the invalid name \"{argName}\".");
				}

				if (separatePointerToken != null && separatePointerToken.Type != TokenType.Indeterminate)
				{
					string argDeclaration = $"{typeName} {separatePointerToken.Word} {argName}";
					throw new ASTException($"The function argument \"${argDeclaration}\" is not valid.");
				}

				int pointerLevel = 0;
				if (separatePointerToken != null) { pointerLevel = separatePointerToken.Word.Length; }
				else { pointerLevel = typeNameToken.Word.SeparateTypeNameAndPointerLevel().Item2; }

				// Look up the data type in the name table.
				DataType argType = GetDataTypeFromNameTable(typeName).WithPointerLevel(pointerLevel);

				result.Add(new FunctionArgument(argType, argName));
			}

			return result;
		}
	}
}
