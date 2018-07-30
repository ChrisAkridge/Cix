using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class ExpressionIdentifier : ExpressionElement
	{
		public string Name { get; }

		public ExpressionIdentifier(string name) => Name = name;

		public override string ToString() => Name;
	}
}
