using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class ExpressionArrayAccess : ExpressionElement
	{
		private List<ExpressionElement> elements;
		public IReadOnlyList<ExpressionElement> Elements => elements.AsReadOnly();

		public ExpressionElementSequence Sequence { get; }

		// TODO: this should own an expression, not a list of elements
		public ExpressionArrayAccess(IEnumerable<ExpressionElement> elements,
			ExpressionElementSequence sequence)
		{
			this.elements = elements.ToList();
			Sequence = sequence;
		}
	}

	public enum ExpressionElementSequence
	{
		Infix,
		PostFix
	}
}
