using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cix.AST.Generator.IntermediateForms
{
	public sealed class IntermediateStruct : Element
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

		public override void Print(StringBuilder builder, int depth)
		{
			builder.AppendLineWithIndent($"Intermediate Struct {Name} {{", depth);

			foreach (IntermediateStructMember member in Members)
			{
				member.Print(builder, depth + 1);
			}

			builder.AppendLineWithIndent("}", depth);
		}
	}
}
