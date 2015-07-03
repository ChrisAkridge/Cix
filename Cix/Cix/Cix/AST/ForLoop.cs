using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class ForLoop : Element
	{
		public Expression Initializor { get; private set; }
		public Expression Condition { get; private set; }
		public Expression Iterator { get; private set; }
		public List<Element> Statements { get; private set;}

		public ForLoop(Expression initializor, Expression condition, Expression iterator, IEnumerable<Element> statements)
		{
			this.Initializor = initializor;
			this.Condition = condition;
			this.Iterator = iterator;
			this.Statements = statements.ToList();
		}
	}
}
