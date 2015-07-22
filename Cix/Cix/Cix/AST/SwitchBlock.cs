using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class SwitchBlock : Element
	{
		public Expression SwitchExpression { get; private set; }
		public List<SwitchCase> SwitchCases { get; private set; }

		public SwitchBlock(Expression switchExpression, IEnumerable<SwitchCase> switchCases)
		{
			SwitchExpression = switchExpression;
			SwitchCases = switchCases.ToList();
		}
	}
}
