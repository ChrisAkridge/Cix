using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST.Generator
{
	/// <summary>
	/// Generates AST for a structure given a token list.
	/// </summary>
	public sealed class StructureGenerator
	{
		public List<Element> Generate(TokenEnumerator enumerator, Dictionary<string, Element> nameTable)
		{
			List<Element> result = new List<Element>();

			// First, parse the structure's name. The token enumerator is still set on the "struct" token.
			Token current = enumerator.MoveNextValidate(TokenType.Identifier);
			string structName = current.Word;
			StructDeclaration structure = new StructDeclaration(structName, new List<StructMemberDeclaration>());

			// Next should be an openscope. We should only have one openscope and one closescope per struct.
			enumerator.MoveNextValidate(TokenType.OpenScope);
			enumerator.MoveNext();
			current = enumerator.Current;

			// Now we should be finding member definitions.
			List<StructMemberDeclaration> members = new List<StructMemberDeclaration>();

			while (current.Type != TokenType.CloseScope)
			{
				
			}

			return result;
		}

		private StructMemberDeclaration GenerateMember(TokenEnumerator enumerator, Dictionary<string, Element> nameTable)
		{
			
		}
	}
}
