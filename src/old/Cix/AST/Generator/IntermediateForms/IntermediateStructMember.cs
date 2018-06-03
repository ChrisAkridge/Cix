using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cix.AST.Generator.IntermediateForms
{
	public sealed class IntermediateStructMember
	{
		public string TypeName { get; }
		public string Name { get; }
		public int PointerLevel { get; }
		public int ArraySize { get; }

		public IntermediateStructMember(string typeName, string name, int pointerLevel, int arraySize)
		{
			TypeName = typeName;
			Name = name;
			PointerLevel = pointerLevel;
			ArraySize = arraySize;
		}
	}
}
