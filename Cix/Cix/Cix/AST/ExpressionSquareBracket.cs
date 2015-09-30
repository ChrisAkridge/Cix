using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class ExpressionSquareBracket : ExpressionElement
	{
		public ParenthesesType Type { get; }

		public ExpressionSquareBracket(ParenthesesType type)
		{
			Type = type;
		}
	}
}
