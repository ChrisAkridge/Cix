using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class ExpressionParentheses : ExpressionElement
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
