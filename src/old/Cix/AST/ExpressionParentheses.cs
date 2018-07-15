using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	// Parentheses aren't used in postfix; maybe we can remove this?
	// no, no we can't
	// ...or can we?
	internal sealed class ExpressionParentheses : ExpressionElement
	{
		public ParenthesesType Type { get; }

		public ExpressionParentheses(ParenthesesType type)
		{
			Type = type;
		}
	}

	public enum ParenthesesType
	{
		Left,
		Right
	}
}
