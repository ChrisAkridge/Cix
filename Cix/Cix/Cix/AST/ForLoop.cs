using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class ForLoop : Element
	{
		private List<Element> statements;

		public Expression Initializor { get; private set; }
		public Expression Condition { get; private set; }
		public Expression Iterator { get; private set; }
		public IReadOnlyList<Element> Statements => statements.AsReadOnly();

		public ForLoop(Expression initializor, Expression condition, Expression iterator, 
			IEnumerable<Element> statements)
		{
			Initializor = initializor;
			Condition = condition;
			Iterator = iterator;
			this.statements = statements.ToList();
		}
	}
}
