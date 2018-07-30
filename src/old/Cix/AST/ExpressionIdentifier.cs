using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	// why is this an element and not an expression element?
	public sealed class Identifier : Element
	{
		public string Name { get; }

		public Identifier(string name) => Name = name;

		public override void Print(StringBuilder builder, int depth)
			=> builder.AppendLineWithIndent(Name, depth);
	}
}
