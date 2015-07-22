using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class SwitchCase : Element
	{
		public ExpressionConstant CaseConstant { get; private set; }
		public bool IsDefaultCase { get; private set; }
		public List<Element> Statements { get; private set; }

		public SwitchCase(ExpressionConstant caseConstant, bool isDefaultCase, IEnumerable<Element> statements)
		{
			CaseConstant = caseConstant;
			IsDefaultCase = isDefaultCase;
			Statements = statements.ToList();
		}
	}
}
