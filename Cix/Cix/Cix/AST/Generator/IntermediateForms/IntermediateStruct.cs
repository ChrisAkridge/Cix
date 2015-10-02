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
		public int Size { get; set; }

		public int FirstDefinitionTokenIndex { get; set; }
		public int LastTokenIndex { get; set; }

		public List<IntermediateStructMember> Members { get; set; }

		public IntermediateStruct(string name)
		{
			Name = name;
			Members = new List<IntermediateStructMember>();
		}
	}
}
