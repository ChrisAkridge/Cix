using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class SwitchCase : Element
	{
		private List<Element> statements;

		public ExpressionConstant CaseConstant { get; }
		public bool IsDefaultCase { get; }
		public IReadOnlyList<Element> Statements => statements.AsReadOnly();

		public SwitchCase(ExpressionConstant caseConstant, bool isDefaultCase, 
			IEnumerable<Element> statements)
		{
			CaseConstant = caseConstant;
			IsDefaultCase = isDefaultCase;
			this.statements = statements.ToList();
		}
	}
}
