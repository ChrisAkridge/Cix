using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class WhileLoop : Element
	{
		public Expression Condition { get; private set; }
		public List<Element> Statements { get; private set; }

		public WhileLoop(Expression condition, IEnumerable<Element> statements)
		{
			this.Condition = condition;
			this.Statements = statements.ToList();
		}
	}
}
