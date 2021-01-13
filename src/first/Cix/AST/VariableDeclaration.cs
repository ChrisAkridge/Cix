using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class VariableDeclaration : Element
	{
		public DataType Type { get; }
		public string Name { get; }

		public VariableDeclaration(DataType type, string name)
		{
			Type = type;
			Name = name;
		}

		public override void Print(StringBuilder builder, int depth)
		{
			builder.AppendLineWithIndent($"{Type} {Name}", depth);
		}
	}
}
