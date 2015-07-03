using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	/// <summary>
	/// A list of expression elements (operators and operands) stored in postfix form.
	/// For instance, (3 + 2) * 5 becomes 3 2 + 5 *
	/// </summary>
	public sealed class Expression : ExpressionElement
	{
		public List<ExpressionElement> Elements;

		public Expression(IEnumerable<ExpressionElement> elements)
		{
			this.Elements = elements.ToList();
		}

		public static Expression Parse(IEnumerable<ExpressionElement> infixElements)
		{
			throw new NotImplementedException();
		}
	}
}
