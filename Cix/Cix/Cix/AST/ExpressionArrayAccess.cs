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

		public IReadOnlyList<ExpressionElement> Elements
		{
			get
			{
				return elements.AsReadOnly();
			}
		}

		public ExpressionElementSequence Sequence { get; }

		public ExpressionArrayAccess(IEnumerable<ExpressionElement> cElements, ExpressionElementSequence sequence)
		{
			elements = cElements.ToList();
			Sequence = sequence;
		}
	}

	public enum ExpressionElementSequence
	{
		Infix,
		PostFix
	}
}
