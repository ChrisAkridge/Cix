using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class WhileLoop : Element
	{
		private List<Element> statements;

		public Expression Condition { get; }
		public IReadOnlyList<Element> Statements => statements.AsReadOnly();

		public WhileLoop(Expression condition, IEnumerable<Element> statements)
		{
			Condition = condition;
			this.statements = statements.ToList();
		}
	}
}
