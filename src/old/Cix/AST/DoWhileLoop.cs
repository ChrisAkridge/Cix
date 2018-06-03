using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class DoWhileLoop : Element
	{
		private List<Element> statements;
		
		public Expression Condition { get; }
		public IReadOnlyList<Element> Statements => statements.AsReadOnly();

		public DoWhileLoop(Expression condition, IEnumerable<Element> statements)
		{
			Condition = condition;
			statements = statements.ToList();
		}
	}
}
