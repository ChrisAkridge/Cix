using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	// Probably will remove in favor of ExpressionConstant
	public sealed class StringLiteral : ExpressionElement
	{
		public string Literal { get; }

		public StringLiteral(string literal)
		{
			Literal = literal;
		}
	}
}
