using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class StringLiteral : ExpressionElement
	{
		public string Literal { get; }

		public StringLiteral(string literal)
		{
			Literal = literal;
		}
	}
}
