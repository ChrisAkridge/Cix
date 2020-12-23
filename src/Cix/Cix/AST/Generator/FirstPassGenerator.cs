using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Cix.AST.Generator.IntermediateForms;
using Cix.Errors;
using Cix.Extensions;
using Cix.Parser;

namespace Cix.AST.Generator
{
	/// <summary>
	///     Performs the first pass of generation of the abstract syntax tree, given a tokenized Cix
	///     file.
	/// </summary>
	/// <remarks>
	///     This stage creates a tree containing the complete declaration of all structures
	///     and global variables, and the headers and locations of all functions.
	/// </remarks>
	[Obsolete]
    public sealed class FirstPassGenerator
	{
		/// <summary>
		/// The maximum depth that structs can be members of other structs.
		/// </summary>
		private const int MaxStructNestingDepth = 100;

		private readonly TokenEnumerator tokens;
		private bool astGenerated;

		private readonly IErrorListProvider errorList;
		private List<IntermediateFunction> intermediateFunctions = new List<IntermediateFunction>();
		private List<Element> tree = new List<Element>();

		/// <summary>
		///     Gets a read-only list of elements representing the AST.
		/// </summary>
		public IReadOnlyList<Element> Tree => tree.AsReadOnly();

		/// <summary>
		///     Gets a read-only list of the intermediate functions in the tree.
		/// </summary>
		public IReadOnlyList<IntermediateFunction> IntermediateFunctions =>
			intermediateFunctions.AsReadOnly();

		/// <summary>
		///     Initializes a new instance of the <see cref="FirstPassGenerator" /> class.
		/// </summary>
		/// <param name="tokens">The tokens of the Cix file to generate a tree for.</param>
		/// <param name="errorList">A provider for an error list to add errors to.</param>
		public FirstPassGenerator(TokenEnumerator tokens, IErrorListProvider errorList)
		{
			this.tokens = tokens;
			this.errorList = errorList;
		}

		/// <summary>
		///     Generates the first pass of the abstract syntax tree.
		/// </summary>
		public void GenerateFirstPassAST()
		{
			if (astGenerated)
			{
				errorList.AddError(ErrorSource.ASTGenerator, 1,
					"First-pass AST already generated; did you accidentally call GenerateFirstPassAST again?", "",
					0);
				return;
			}

			List<IntermediateStruct> intermediateStructs = GenerateIntermediateStructs();
			tree = GenerateStructTree(intermediateStructs);
			tree = AddGlobalsToTree(tree);
			intermediateFunctions = GenerateIntermediateFunctions();
			tree.AddRange(intermediateFunctions);

			astGenerated = true;
		}

		/// <summary>
		///     Parses the token stream to find a list of intermediate structs, containing each
		///     struct's name, start token index and end token index.
		/// </summary>
		/// <returns>A list of intermediate structs.</returns>
		public List<IntermediateStruct> GenerateIntermediateStructs()
		{
			// Take every struct header into an intermediate definition of
			// { Name, FirstDefinitionTokenIndex, LastTokenIndex }, the
			// last of which is the index of the final semicolon, NOT the
			// closescope.

			var result = new List<IntermediateStruct>();

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

				if (!tokens.MoveNextValidate(TokenType.Identifier)) { continue; }
				string structName = tokens.Current.Text;
				int nameTokenIndex = tokens.CurrentIndex;

				if (!tokens.MoveNextValidate(TokenType.OpenScope)) { continue; }
				int firstTokenIndex = tokens.CurrentIndex + 1;

				tokens.SkipBlock();
				int lastTokenIndex = tokens.CurrentIndex - 2;

				var newStruct =
					new IntermediateStruct(structName, nameTokenIndex, firstTokenIndex, lastTokenIndex);
				result.Add(newStruct);
			}

			return result;
		}

		/// <summary>
		///     Converts a list of intermediate structs into a fully generated tree of the structs and
		///     all their members.
		/// </summary>
		/// <param name="intermediateStructs">
		///     The intermediate structures for which to generate a tree.
		/// </param>
		/// <returns>A tree containing the complete struct definitions.</returns>
		public List<Element> GenerateStructTree(List<IntermediateStruct> intermediateStructs)
		{
			if (!AllStructNamesUnique(intermediateStructs)) { return null; }

			// Part 1: Get intermediate forms for all the struct members where types are just names
			// and pointer levels are considered. Here, we'll ensure that void- and
			// lpstring-typed members may only appear as pointer members.

			foreach (IntermediateStruct intermediateStruct in intermediateStructs)
			{
				ParseIntermediateStructMembers(intermediateStruct);
				NameTable.Instance[intermediateStruct.Name] = intermediateStruct;
			}

			// Part 2: Create the fully-formed definitions for all structs.
			// Loop through every intermediate struct and define all its members.
			// If a member is a struct that isn't defined yet, define it.
			var structsTree = new List<Element>();

			foreach (IntermediateStruct intermediateStruct in intermediateStructs)
			{
				if (!(NameTable.Instance[intermediateStruct.Name] is IntermediateStruct)) { continue; }
				int depthLevel = 0;
				CreateStructDeclaration(intermediateStruct, ref depthLevel);
			}

			structsTree.AddRange(NameTable.Instance.Names.Where(n => n.Value is StructDataType)
				.Select(kvp => kvp.Value));
			return structsTree;
		}

		private bool AllStructNamesUnique(IEnumerable<IntermediateStruct> structs)
		{
			IEnumerable<IGrouping<string, IntermediateStruct>>
				groupingsByName = structs.GroupBy(s => s.Name);
			IEnumerable<IGrouping<string, IntermediateStruct>> groupsWithDuplicates =
				groupingsByName.Where(g => g.Count() > 1);

			if (groupsWithDuplicates.Any())
			{
				foreach (IGrouping<string, IntermediateStruct> duplicateStructs in groupsWithDuplicates)
				{
					foreach (IntermediateStruct duplicateStruct in duplicateStructs)
					{
						Token nameToken = tokens[duplicateStruct.NameTokenIndex];
						errorList.AddError(ErrorSource.ASTGenerator, 5,
							$"Multiple structs named {duplicateStruct.Name}",
							nameToken.FilePath, nameToken.LineNumber);
					}
				}
				return false;
			}

			return true;
		}

		/// <summary>
		///     Parses the token stream to find global variables and add them to the tree.
		/// </summary>
		/// <param name="tree">The abstract syntax tree immediately after stage B generation.</param>
		/// <returns>The abstract syntax tree containing global variable declarations plus stage B generation.</returns>
		// ReSharper disable once ParameterHidesMember
		public List<Element> AddGlobalsToTree(List<Element> tree)
		{
			tokens.Reset();
			var globalVariables = new List<GlobalVariableDeclaration>();

			// Scan through all tokens at the root level, looking for the global keyword.
			int nestingDepth = 0;
			do
			{
				// ReSharper disable once ConvertIfStatementToSwitchStatement
				if (tokens.Current.Type == TokenType.OpenScope) { nestingDepth++; }
				else if (tokens.Current.Type == TokenType.CloseScope) { nestingDepth--; }
				else if (tokens.Current.Text == "global" && nestingDepth == 0)
				{
					List<Token> globalStatement = tokens.MoveNextStatement();
					globalVariables.Add(GetGlobalVariableDeclaration(globalStatement));
				}
				tokens.MoveNext();
			} while (!tokens.AtEnd);

			tree.AddRange(globalVariables);
			return tree;
		}

		/// <summary>
		///     Parses the token stream to find function headers and their start/end token indices.
		/// </summary>
		/// <returns>A tree with the function information.</returns>
		public List<IntermediateFunction> GenerateIntermediateFunctions()
		{
			tokens.Reset();
			var functions = new List<IntermediateFunction>();

			// Scan through all the tokens at the root level.
			// Functions are the only root element that doesn't start with "struct" or "global".
			int nestingDepth = 0;
			do
			{
				if (tokens.Current.Type == TokenType.OpenScope) { nestingDepth++; }
				else if (tokens.Current.Type == TokenType.CloseScope) { nestingDepth--; }
				else if (tokens.Current.Type == TokenType.KeyStruct)
				{
					// Skip this entire struct definition.
					tokens.SkipBlock();
					continue;
				}
				else if (tokens.Current.Text == "global") { tokens.MoveNextStatement(); }
				else if (nestingDepth == 0 && tokens.Current.Type != TokenType.KeyStruct && tokens.Current.Text != "global")
				{
					// We've found a type name! Probably! Let's find it in the nametable before we
					// get too excited.
					// TODO: functions can return @funcptr, too
					string possibleTypeName = tokens.Current.Text;
					if (!NameTable.Instance.Names.ContainsKey(possibleTypeName.TrimAsterisks()))
					{
						errorList.AddError(ErrorSource.ASTGenerator, 6,
							$"Return type {possibleTypeName} is not defined.",
							tokens.Current.FilePath, tokens.Current.LineNumber);
						continue;
					}

					// This is a proper function. Let's get started!
					// Remember that the return type can appear as both "type* name" and "type * name",
					// so we'll have to check for that.
					DataType returnType = GetDataTypeFromNameTable(possibleTypeName);
					if (!tokens.MoveNext()) { break; }

					int pointerLevel = 0;
					while (tokens.Current.Text == "*")
					{
						pointerLevel++;
						if (!tokens.MoveNext()) { break; }
					}

					if (returnType != null) { returnType = returnType.WithPointerLevel(pointerLevel); }
					else { continue; }

					string name = tokens.Current.Text;
					if (!name.IsIdentifier())
					{
						errorList.AddError(ErrorSource.ASTGenerator, 7, $"Function name {name} is not valid.",
							tokens.Current.FilePath, tokens.Current.LineNumber);
						continue;
					}

					// Up next should be the leftparen for the function argument list.
					if (!tokens.MoveNextValidate(TokenType.OpenParen)) { continue; }
					int argsStartIndex = tokens.CurrentIndex;
					int argsEndIndex = argsStartIndex;

					// Find the close parentheses. No function argument can have parentheses in it,
					// so we don't have to worry about nesting.
					while (tokens.Current.Type != TokenType.CloseParen)
					{
						if (!tokens.MoveNext()) { break; }
						argsEndIndex++;
					}

					// The above loop stops just before the closeparen. Add 1 to move it to the
					// closeparen.
					// TODO: is that right?
					argsEndIndex += 1;

					// Parse the function arguments.
					IEnumerable<FunctionParameter> functionArguments =
						ParseFunctionArguments(argsStartIndex, argsEndIndex);

					// The openscope should be the next token.
					if (!tokens.MoveNextValidate(TokenType.OpenScope)) { continue; }

					int openScopeIndex = tokens.CurrentIndex;
					int closeScopeIndex = -1;
					int functionNestingDepth = 1;

					while (true)
					{
						if (!tokens.MoveNext()) { break; }
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

					var function = new IntermediateFunction(returnType, name, functionArguments, openScopeIndex,
						closeScopeIndex);
					functions.Add(function);
					NameTable.Instance.Names.Add(function.Name, function);
				}

				tokens.MoveNext();
			} while (!tokens.AtEnd);

			return functions;
		}

		[SuppressMessage("ReSharper", "PossibleNullReferenceException")] // no part of the token list should ever be null
		private void ParseIntermediateStructMembers(IntermediateStruct intermediateStruct)
		{
			IEnumerable<TokenEnumerator> statements = tokens
				.Subset(intermediateStruct.FirstDefinitionTokenIndex, intermediateStruct.LastTokenIndex)
				.SplitOnSemicolon();

			foreach (TokenEnumerator statementTokens in statements)
			{
				// Valid member definitions:
				// int i;
				// void* pv;
				// byte** ppb;
				// int[50] array;
				// short*[25] p_array;
				// @funcptr<void, int, int***> funcptr;
				// @funcptr<void, int, int***>[] funcptr_array;

				int memberPointerLevel = 0;
				int memberArraySize = 0;

				Token firstTypeToken = statementTokens.Current; // used for error tracking

				if (!TypeParser.TryParseType(statementTokens, errorList, out DataType memberType))
				{
					continue;
				}

				string memberName = null;
				if (statementTokens.Current.Type == TokenType.Identifier)
				{
					memberName = statementTokens.Current.Text;
				}
				else if (statementTokens.Current.Type == TokenType.OpenBracket)
				{
					// This is an array member.
					if (!statementTokens.MoveNext() || !statementTokens.Current.Type.IsNumericLiteralToken())
					{
						errorList.AddError(ErrorSource.ASTGenerator, 13,
							"The size of the struct array element was not declared.",
							firstTypeToken.FilePath, firstTypeToken.LineNumber);
						continue;
					}

					ExpressionConstant arraySizeLiteral = LiteralParser.Parse(statementTokens.Current, errorList);
					if (arraySizeLiteral.Type.Name != "int")
					{
						errorList.AddError(ErrorSource.ASTGenerator, 14,
							"The type of the size of the struct array element is not valid; must be int.",
							firstTypeToken.FilePath, firstTypeToken.LineNumber);
						continue;
					}
					else if (arraySizeLiteral.ToInt() <= 0)
					{
						errorList.AddError(ErrorSource.ASTGenerator, 15,
							"The size of the struct array element is not valid; must be positive.",
							firstTypeToken.FilePath, firstTypeToken.LineNumber);
						continue;
					}
					memberArraySize = arraySizeLiteral.ToInt();

					if (!statementTokens.MoveNext() || statementTokens.Current.Type != TokenType.CloseBracket)
					{
						errorList.AddError(ErrorSource.ASTGenerator, 16,
							"Expected closing bracket.",
							firstTypeToken.FilePath, firstTypeToken.LineNumber);
						continue;
					}

					if (!statementTokens.MoveNext() || statementTokens.Current.Type != TokenType.Identifier)
					{
						errorList.AddError(ErrorSource.ASTGenerator, 12,
							$"Invalid token {statementTokens.Current.Text} of type {statementTokens.Current.Type} after type.",
							firstTypeToken.FilePath, firstTypeToken.LineNumber);
						continue;
					}

					memberName = statementTokens.Current.Text;
				}

				if (memberArraySize == 0) { memberArraySize = 1;}

				intermediateStruct.Members.Add(new IntermediateStructMember(memberType, memberName,
					memberPointerLevel, memberArraySize, firstTypeToken));
			}
		}

		private StructDataType CreateStructDeclaration(IntermediateStruct intermediateStruct,
			ref int depthLevel)
		{
			string structName = intermediateStruct.Name;
			var members = new List<StructMemberDeclaration>();
			var offsetCounter = 0;

			foreach (IntermediateStructMember member in intermediateStruct.Members)
			{
				// Resolve the type.
				if (!NameTable.Contains(member.Type))
				{
					errorList.AddError(ErrorSource.ASTGenerator, 16,
						$"Invalid type {member.Type.Name} for {structName}.{member.Name}.",
						member.SourceFilePath, member.SourceLineNumber);
					continue;
				}

				if (member.Type is FunctionPointerType)
				{
					// Scenario 0: Member is a function pointer type
					var memberDeclaration = new StructMemberDeclaration(member.Type,
						member.Name, member.ArraySize, offsetCounter);
					members.Add(memberDeclaration);
					offsetCounter += 8 * member.ArraySize;
					continue;
				}

				Element typeEntry = NameTable.Instance[member.Type.Name];

				if (typeEntry is DataType primitiveType)
				{
					// Scenario 1: Member has a primitive type or is a pointer to a primitive type
					DataType fullType = primitiveType.WithPointerLevel(member.Type.PointerLevel);
					var memberDeclaration =
						new StructMemberDeclaration(fullType, member.Name, member.ArraySize, offsetCounter);
					members.Add(memberDeclaration);
					offsetCounter += memberDeclaration.Type.Size * member.ArraySize;
				}
				else if (typeEntry is StructDataType structDataType)
				{
					// Scenario 2: Member has a struct type or is a pointer to a struct type
					members.Add(GetDeclarationOfStructMember(structDataType, member, ref offsetCounter));
				}
				else if (typeEntry is IntermediateStruct intermediateStructType)
				{
					// Scenario 3: Member has a struct type which is not fully defined
					depthLevel++;

					if (depthLevel > MaxStructNestingDepth)
					{
						// why is the user nesting 100 structs
						errorList.AddError(ErrorSource.ASTGenerator, 17,
							$"Maximum struct nesting depth of {MaxStructNestingDepth} achieved. Look for circular struct members.",
							member.SourceFilePath, member.SourceLineNumber);
						continue;
					}

					StructDataType newlyDefinedStruct = CreateStructDeclaration(intermediateStructType, ref depthLevel);
					members.Add(GetDeclarationOfStructMember(newlyDefinedStruct, member, ref offsetCounter));
				}
				else if (typeEntry == null && member.Name == "void" && member.PointerLevel > 0)
				{
					// Scenario 4: Pointer to void
					var voidPtrMember =
						new StructMemberDeclaration(new DataType("void", member.PointerLevel, 8),
							member.Name, member.ArraySize, offsetCounter);
					members.Add(voidPtrMember);
				}
			}

			var result = new StructDataType(intermediateStruct.Name, 0, members);
			NameTable.Instance[intermediateStruct.Name] = result;
			return result;
		}

		private static StructMemberDeclaration GetDeclarationOfStructMember(StructDataType memberType,
			IntermediateStructMember intermediateMember, ref int offsetCounter)
		{
			var result = new StructMemberDeclaration(memberType, intermediateMember.Name,
				intermediateMember.ArraySize, offsetCounter);
			offsetCounter += result.Size;
			return result;
		}

		private GlobalVariableDeclaration GetGlobalVariableDeclaration(List<Token> globalStatement)
		{
			// Valid global variable definitions
			//  global T name;
			//  global U primitive = 137; (numeric types only)
			//  global T* ptr;
			//	global T[5] arrayMember;
			//  global @funcptr<R, P1, P2> function;
			//  global @funcptr<R, P1, P2>* pFunction;
			//  global @funcptr<R, P1, P2>[3] functionArray;

			var statementTokens = new TokenEnumerator(globalStatement, errorList);
			statementTokens.MoveNext();
		
			if (!TypeParser.TryParseType(tokens, errorList, out DataType globalType))
			{
				// TODO: AAAA
				// We really need a better pattern for a non-void returning method that
				// may fail, since we're not try-catching all over the place
				// I do NOT like the TryXXX pattern very much
				throw new NotImplementedException();
			}

			statementTokens.MoveNext();

			string variableName = statementTokens.Current.Text;
			ExpressionConstant numericLiteral = null;

			if (globalStatement.Any(t => t.Text == "=") && NameTable.IsPrimitiveType(globalType.Name))
			{
				Token numericLiteralToken = globalStatement[4];
				numericLiteral = LiteralParser.Parse(numericLiteralToken, errorList);
			}

			// Double-check that the global's type actually exists.
			bool typeExists = NameTable.Contains(globalType.Name);
			if (globalType.Name == "void" && globalType.PointerLevel == 0)
			{
				errorList.AddError(ErrorSource.ASTGenerator, 18,
					"No global may have type void, perhaps you meant pointer to void?",
					statementTokens.Current.FilePath, statementTokens.Current.LineNumber);
				return null;
			}
			else if (!typeExists && globalType.Name != "void")
			{
				errorList.AddError(ErrorSource.ASTGenerator, 20,
					$"Type {globalType.Name} for global variable {variableName} doesn't exist.",
					globalStatement[1].FilePath, globalStatement[1].LineNumber);
				return null;
			}
			else if (globalType.Name == "lpstring")
			{
				errorList.AddError(ErrorSource.ASTGenerator, 21,
					"No global variable may directly have the type of lpstring. Consider using pointer to lpstring instead.",
					globalStatement[1].FilePath, globalStatement[1].LineNumber);
				return null;
			}

			return new GlobalVariableDeclaration(globalType, variableName, numericLiteral);
		}

		private DataType GetDataTypeFromNameTable(string typeName)
		{
			// The error generation in this method is not the greatest.
			// This is called from methods that, by all means, should probably be generating the
			// errors themselves, but I'm not sure how to do that without changing this method's
			// signature to something like
			// TryGetDataTypeResult TryGetDataTypeFromNameTable(string, out DataType). So we'll
			// cheat a bit and grab the current token here, under the assumption that the enumerator
			// at least hasn't moved off this line.

			// TODO: wait, do we actually need this?

			Tuple<string, int> typeNameWithPointerLevel = typeName.SeparateTypeNameAndPointerLevel();
			Element typeInNameTable = NameTable.Instance[typeNameWithPointerLevel.Item1];

			switch (typeInNameTable)
			{
				case null:
					errorList.AddError(ErrorSource.ASTGenerator, 22, $"No type named {typeName} exists.",
						tokens.Current.FilePath, tokens.Current.LineNumber);
					return null;
				case StructDataType structDataType:
					return structDataType.WithPointerLevel(typeNameWithPointerLevel.Item2);
				case DataType dataType: return dataType.WithPointerLevel(typeNameWithPointerLevel.Item2);
				default:
					errorList.AddError(ErrorSource.ASTGenerator, 23,
						$"Tried to find a type named \"{typeName}\", but that name defines a {typeInNameTable.GetType().Name}.",
						tokens.Current.FilePath, tokens.Current.LineNumber);
					return null;
			}
		}

		private IEnumerable<FunctionParameter> ParseFunctionArguments(int startIndex, int endIndex)
		{
			// Start index and end index should point to the openparen and closeparen, respectively
			// Add 1 to startIndex and subtract 1 from endIndex to point them to the first and last
			// actual tokens in the function arguments.

			// Easy case: if the closeparen is right after the openparen, there are no arguments.
			if (startIndex + 1 == endIndex) { return Enumerable.Empty<FunctionParameter>(); }

			TokenEnumerator paramTokens = tokens.Subset(startIndex + 1, endIndex - 1);
			var result = new List<FunctionParameter>();
			IEnumerable<List<Token>> parameters = paramTokens.SplitOnComma();

			// A function argument is either "type name" or "type* name" or "type * name"
			// (with however many asterisks, of course)

			foreach (List<Token> arg in parameters)
			{
				if (!ValidateFunctionArgumentTokens(arg))
				{
					errorList.AddError(ErrorSource.ASTGenerator, 24,
						$"This function parameter is not of the correct form.",
						tokens.Current.FilePath, tokens.Current.LineNumber);
					continue;
				}

				if (!TypeParser.TryParseType(new TokenEnumerator(arg, errorList), errorList, out DataType paramType))
				{
					throw new InvalidOperationException();
				}
				Token paramNameToken = arg.Last();

				// Look up the type name in the name table to check if it's real.
				if (!NameTable.Instance.Names.ContainsKey(paramType.Name))
				{
					errorList.AddError(ErrorSource.ASTGenerator, 25,
						$"A function argument was declared with return type {paramType.Name}. No type by that name exists.",
						tokens.Current.FilePath, tokens.Current.LineNumber);
					continue;
				}

				string paramName = paramNameToken.Text;
				if (!paramName.IsIdentifier())
				{
					errorList.AddError(ErrorSource.ASTGenerator, 26,
						$"A function argument had the invalid name \"{paramName}\".",
						tokens.Current.FilePath, tokens.Current.LineNumber);
					continue;
				}

				int pointerLevel = arg.Count - 2;

				// WYLO:
				//
				// We also need a better pattern for methods that return something (i.e. a
				// parsed DataType) but may also return errors. Some options are returning
				// null (probably the easiest), using the TryXXX pattern (ugh), or using
				// exceptions (even worse). Null is probably the easiest pattern to use.
				// 
				// And we need to change how intermediate functions and their parameters parse
				// their types.

				result.Add(new FunctionParameter(paramType, paramName));
			}

			return result;
		}

		private static bool ValidateFunctionArgumentTokens(IReadOnlyList<Token> tokens)
		{
			if (tokens == null || !tokens.Any()) { return false; }

			// The first token must be an identifier
			if (!tokens[0].Text.IsIdentifier(true)) { return false; }

			// The last token must be an identifier
			if (tokens[tokens.Count - 1].Type != TokenType.Identifier) { return false; }

			return true;
		}
	}
}