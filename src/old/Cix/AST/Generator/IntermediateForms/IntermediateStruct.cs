using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cix.AST.Generator.IntermediateForms
{
	internal sealed class IntermediateStruct : Element
	{
		public string Name { get; }
		public int Size { get; }

		public int NameTokenIndex { get; }
		public int FirstDefinitionTokenIndex { get; }
		public int LastTokenIndex { get; }

		public List<IntermediateStructMember> Members { get; }

		public IntermediateStruct(string name, int nameTokenIndex, int firstTokenIndex, int lastTokenIndex)
		{
			Name = name;
			Members = new List<IntermediateStructMember>();

			NameTokenIndex = nameTokenIndex;
			FirstDefinitionTokenIndex = firstTokenIndex;
			LastTokenIndex = lastTokenIndex;
		}
	}
}
