using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class DoWhileLoop : Element
	{
		public Expression Condition { get; private set; }
		public List<Element> Statements { get; private set; }

		public DoWhileLoop(Expression condition, IEnumerable<Element> statements)
		{
			Condition = condition;
			Statements = statements.ToList();
		}
	}
}
