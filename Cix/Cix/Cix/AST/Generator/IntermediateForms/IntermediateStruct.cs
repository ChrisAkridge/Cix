using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST.Generator.IntermediateForms
{
	public sealed class IntermediateStruct : Element
	{
		public string Name { get; }
		public int Size { get; }

		public int FirstDefinitionTokenIndex { get; }
		public int LastTokenIndex { get; }

		public List<IntermediateStructMember> Members { get; }

		public IntermediateStruct(string name, int firstTokenIndex, int lastTokenIndex)
		{
			Name = name;
			Members = new List<IntermediateStructMember>();
			FirstDefinitionTokenIndex = firstTokenIndex;
			LastTokenIndex = lastTokenIndex;
		}
	}
}
