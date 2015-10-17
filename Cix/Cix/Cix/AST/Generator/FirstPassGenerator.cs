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
					if (enumerator.Current.Word == "void" || enumerator.Current.Word == "lpstring") { throw new ASTException("Members of type void or lpstring may not appear in structs."); }
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
	}
}
