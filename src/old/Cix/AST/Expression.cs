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
		private readonly List<ExpressionElement> elements;
		public IReadOnlyList<ExpressionElement> Elements => elements.AsReadOnly();

		public override string ToString() => string.Join(" ", elements.Select(e => e.ToString()));

		public Expression(IEnumerable<ExpressionElement> infixElements) => elements = infixElements.ToList();
	}
}
