using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Cix.AST.Generator.IntermediateForms;
using Cix.Errors;
using Cix.Parser;
using JetBrains.Annotations;

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
	internal sealed class FirstPassGenerator
	{
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
		[CanBeNull]
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

			structsTree.AddRange(NameTable.Instance.Names.Where(n => n.Value is StructDeclaration)
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

					functions.Add(new IntermediateFunction(returnType, name, functionArguments, openScopeIndex,
						closeScopeIndex));
				}

				tokens.MoveNext();
			} while (!tokens.AtEnd);

			return functions;
		}

		[SuppressMessage("ReSharper", "PossibleNullReferenceException")] // no part of the token list should ever be null
		private void ParseIntermediateStructMembers(IntermediateStruct intermediateStruct)
		{
			IEnumerable<List<Token>> statements = tokens
				.Subset(intermediateStruct.FirstDefinitionTokenIndex, intermediateStruct.LastTokenIndex)
				.SplitOnSemicolon();

			foreach (List<Token> statement in statements)
			{
				// Valid member definitions:
				// int i;
				// void* pv;
				// byte** ppb;
				// int array[50];
				// short* p_array[25];

				var memberPointerLevel = 0;
				int memberArraySize;

				List<Token>.Enumerator enumerator = statement.GetEnumerator();
				enumerator.MoveNext(); // start with the first item

				Token typeToken = enumerator.Current; // used for error tracking

				// If this struct member is a pointer...
				string memberTypeName = enumerator.Current.Text;

				enumerator.MoveNext();
				while (enumerator.Current.Text == "*")
				{
					memberPointerLevel++;
					enumerator.MoveNext();
				}

				if ((memberTypeName == "void" || memberTypeName == "lpstring") && memberPointerLevel == 0)
				{
					errorList.AddError(ErrorSource.ASTGenerator, 11,
						$"Members of type {memberTypeName} cannot appear in a struct; consider a pointer to {memberTypeName} instead.",
						enumerator.Current.FilePath, enumerator.Current.LineNumber);
					continue;
				}

				while (enumerator.Current.Text == "*")
				{
					// ...usually. This is a token with one or more asterisks, i.e. "int ** i"
					memberPointerLevel++;
					enumerator.MoveNext();
				}

				if (enumerator.Current.Type != TokenType.Identifier)
				{
					errorList.AddError(ErrorSource.ASTGenerator, 12,
						$"Invalid token {enumerator.Current.Text} of type {enumerator.Current.Type} after type.",
						typeToken.FilePath, typeToken.LineNumber);
					continue;
				}
				string memberName = enumerator.Current.Text;

				if (enumerator.MoveNext() && enumerator.Current.Type == TokenType.OpenBracket)
				{
					// This is an array member.
					if (!enumerator.MoveNext() || enumerator.Current.Type != TokenType.Identifier)
					{
						errorList.AddError(ErrorSource.ASTGenerator, 13,
							$"The size of the array {memberName} was not declared.",
							typeToken.FilePath, typeToken.LineNumber);
						continue;
					}

					// TODO: replace with ExpressionConstant
					NumericLiteral arraySizeLiteral = NumericLiteral.Parse(enumerator.Current.Text);
					if (arraySizeLiteral.UnderlyingType != typeof(int))
					{
						errorList.AddError(ErrorSource.ASTGenerator, 14,
							$"The type of the size of the array {memberName} is not valid; must be int.",
							typeToken.FilePath, typeToken.LineNumber);
						continue;
					}
					else if (arraySizeLiteral.SignedIntegralValue <= 0)
					{
						errorList.AddError(ErrorSource.ASTGenerator, 15,
							$"The size of the array {memberName} is not valid; must be positive.",
							typeToken.FilePath, typeToken.LineNumber);
						continue;
					}
					memberArraySize = (int) arraySizeLiteral.SignedIntegralValue;

					if (!enumerator.MoveNext() || enumerator.Current.Type != TokenType.CloseBracket)
					{
						errorList.AddError(ErrorSource.ASTGenerator, 16,
							"Expected closing bracket.",
							typeToken.FilePath, typeToken.LineNumber);
						continue;
					}
				}
				else
				{
					// This is not an array member, but...
					memberArraySize = 1;
					// ...it still acts like a one-member array.
				}

				enumerator.Dispose();
				intermediateStruct.Members.Add(new IntermediateStructMember(memberTypeName, memberName,
					memberPointerLevel, memberArraySize, typeToken));
			}
		}

		private StructDeclaration CreateStructDeclaration(IntermediateStruct intermediateStruct,
			ref int depthLevel)
		{
			string structName = intermediateStruct.Name;
			var members = new List<StructMemberDeclaration>();
			var offsetCounter = 0;

			foreach (IntermediateStructMember member in intermediateStruct.Members)
			{
				// Resolve the type.
				if (!NameTable.Instance.Names.ContainsKey(member.TypeName))
				{
					errorList.AddError(ErrorSource.ASTGenerator, 16,
						$"Invalid type {member.TypeName} for {structName}.{member.Name}.",
						member.SourceFilePath, member.SourceLineNumber);
					continue;
				}
				Element typeEntry = NameTable.Instance[member.TypeName];

				if (typeEntry is DataType baseType1)
				{
					// Scenario 1: Member has a primitive type or is a pointer to a primitive type
					DataType fullType = baseType1.WithPointerLevel(member.PointerLevel);
					var memberDeclaration =
						new StructMemberDeclaration(fullType, member.Name, member.ArraySize, offsetCounter);
					members.Add(memberDeclaration);
					offsetCounter += memberDeclaration.Type.Size * member.ArraySize;
				}
				else if (typeEntry is StructDeclaration baseType)
				{
					// Scenario 2: Member has a struct type or is a pointer to a struct type
					members.Add(GetDeclarationOfStructMember(baseType, member, ref offsetCounter));
				}
				else if (typeEntry is IntermediateStruct @struct)
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

					StructDeclaration newlyDefinedStruct = CreateStructDeclaration(@struct, ref depthLevel);
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

			var result = new StructDeclaration(intermediateStruct.Name, members);
			NameTable.Instance[intermediateStruct.Name] = result;
			return result;
		}

		private static StructMemberDeclaration GetDeclarationOfStructMember(StructDeclaration memberType,
			IntermediateStructMember intermediateMember, ref int offsetCounter)
		{
			var fullType = new DataType(memberType.Name, intermediateMember.PointerLevel, memberType.Size);
			var result = new StructMemberDeclaration(fullType, intermediateMember.Name,
				intermediateMember.ArraySize, offsetCounter);
			offsetCounter += result.Type.Size * result.ArraySize;
			return result;
		}

		[CanBeNull]
		private GlobalVariableDeclaration GetGlobalVariableDeclaration(List<Token> globalStatement)
		{
			// Valid global variable definitions
			//  global T name;
			//  global U primitive = 137;
			//  global T* ptr;

			int pointerLevel = globalStatement.Count(t => t.Text == "*");
			if (pointerLevel > 0) { globalStatement.RemoveAll(t => t.Text == "*"); }

			Tuple<string, int> typeData = globalStatement[1].Text.SeparateTypeNameAndPointerLevel();
			string typeName = typeData.Item1;
			pointerLevel = pointerLevel == 0 ? typeData.Item2 : pointerLevel;

			string variableName = globalStatement[2].Text;
			NumericLiteral numericLiteral = null;

			if (globalStatement.Any(t => t.Text == "=") && NameTable.IsPrimitiveType(typeName))
			{
				string numericLiteralToken = globalStatement[4].Text;
				numericLiteral = NumericLiteral.Parse(numericLiteralToken);
			}

			// Double-check that the global's type actually exists.
			bool typeNameIsActuallyAType = NameTable.Instance[typeName] is DataType;
			bool typeNameIsStruct = NameTable.Instance[typeName] is StructDeclaration;
			bool typeExists = NameTable.Instance.Names.ContainsKey(typeName);
			if (typeName == "void" && pointerLevel == 0)
			{
				errorList.AddError(ErrorSource.ASTGenerator, 18,
					"No global may have type void, perhaps you meant pointer to void?",
					globalStatement[1].FilePath, globalStatement[1].LineNumber);
				return null;
			}
			else if (!typeNameIsActuallyAType && !typeNameIsStruct && typeName != "void")
			{
				errorList.AddError(ErrorSource.ASTGenerator, 19,
					$"{typeName} is not a type; it's a {NameTable.Instance[typeName].GetType().Name}.",
					globalStatement[1].FilePath, globalStatement[1].LineNumber);
				return null;
			}
			else if (!typeExists && typeName != "void")
			{
				errorList.AddError(ErrorSource.ASTGenerator, 20,
					$"Type {typeName} for global variable {variableName} doesn't exist.",
					globalStatement[1].FilePath, globalStatement[1].LineNumber);
				return null;
			}
			else if (typeName == "lpstring")
			{
				errorList.AddError(ErrorSource.ASTGenerator, 21,
					"No global variable may directly have the type of lpstring. Consider using pointer to lpstring instead.",
					globalStatement[1].FilePath, globalStatement[1].LineNumber);
				return null;
			}

			int typeSize = pointerLevel == 0 ? ((DataType) NameTable.Instance[typeName]).Size : 8;
			return new GlobalVariableDeclaration(new DataType(typeName, pointerLevel, typeSize),
				variableName, numericLiteral);
		}

		[CanBeNull]
		private DataType GetDataTypeFromNameTable(string typeName)
		{
			// The error generation in this method is not the greatest.
			// This is called from methods that, by all means, should probably be generating the
			// errors themselves, but I'm not sure how to do that without changing this method's
			// signature to something like
			// TryGetDataTypeResult TryGetDataTypeFromNameTable(string, out DataType). So we'll
			// cheat a bit and grab the current token here, under the assumption that the enumerator
			// at least hasn't moved off this line.

			Tuple<string, int> typeNameWithPointerLevel = typeName.SeparateTypeNameAndPointerLevel();
			Element typeInNameTable = NameTable.Instance[typeNameWithPointerLevel.Item1];

			switch (typeInNameTable)
			{
				case null:
					errorList.AddError(ErrorSource.ASTGenerator, 22, $"No type named {typeName} exists.",
						tokens.Current.FilePath, tokens.Current.LineNumber);
					return null;
				case DataType dataType: return dataType.WithPointerLevel(typeNameWithPointerLevel.Item2);
				case StructDeclaration structDeclaration:
					return structDeclaration.ToDataType().WithPointerLevel(typeNameWithPointerLevel.Item2);
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

			TokenEnumerator argTokens = tokens.Subset(startIndex + 1, endIndex - 1);
			var result = new List<FunctionParameter>();
			if (argTokens.Current == null)
			{
				// No arguments
				return result;
			}

			// Easy case: if the closeparen is right after the openparen, there are no arguments.
			if (startIndex + 1 == endIndex) { return result; }

			IEnumerable<List<Token>> args = argTokens.SplitOnComma();

			// A function argument is either "type name" or "type* name" or "type * name"
			// (with however many asterisks, of course)

			foreach (List<Token> arg in args)
			{
				if (!ValidateFunctionArgumentTokens(arg))
				{
					errorList.AddError(ErrorSource.ASTGenerator, 24,
						$"There are {arg.Count} token(s) in this function argument. There should be 2 or 3.",
						tokens.Current.FilePath, tokens.Current.LineNumber);
					continue;
				}

				Token typeNameToken = arg[0];
				Token argNameToken = arg.Last();

				// Look up the type name in the name table to check if it's real.
				string typeName = typeNameToken.Text.TrimAsterisks();
				if (!NameTable.Instance.Names.ContainsKey(typeName))
				{
					errorList.AddError(ErrorSource.ASTGenerator, 25,
						$"A function argument was declared with return type {typeName}. No type by that name exists.",
						tokens.Current.FilePath, tokens.Current.LineNumber);
					continue;
				}

				string argName = argNameToken.Text;
				if (!argName.IsIdentifier())
				{
					errorList.AddError(ErrorSource.ASTGenerator, 26,
						$"A function argument had the invalid name \"{argName}\".",
						tokens.Current.FilePath, tokens.Current.LineNumber);
					continue;
				}

				int pointerLevel = arg.Count - 2;

				// Look up the data type in the name table.
				DataType argType = GetDataTypeFromNameTable(typeName);
				if (argType != null) { argType = argType.WithPointerLevel(pointerLevel); }
				else { continue; }

				result.Add(new FunctionParameter(argType, argName));
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

			// There must be either no tokens in between or all tokens in between must be asterisks
			if (tokens.Count == 2) { return true; }

			for (int i = 1; i < tokens.Count - 2; i++)
			{
				if (tokens[i].Text != "*") { return false; }
			}

			return true;
		}
	}
}