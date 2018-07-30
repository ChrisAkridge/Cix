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

		public Expression Initializer { get; private set; }
		public Expression Condition { get; private set; }
		public Expression Iterator { get; private set; }
		public IReadOnlyList<Element> Statements => statements.AsReadOnly();

		public ForLoop(Expression initializer, Expression condition, Expression iterator, 
			IEnumerable<Element> statements)
		{
			Initializer = initializer;
			Condition = condition;
			Iterator = iterator;
			this.statements = statements.ToList();
		}

		public override void Print(StringBuilder builder, int depth)
		{
			string loopParenthetical = string.Join("; ", Initializer.ToString(), Condition.ToString(),
				Iterator.ToString());

			builder.AppendLineWithIndent($"for ({loopParenthetical}) {{", depth);

			foreach (Element statement in statements) { statement.Print(builder, depth + 1); }

			builder.AppendLineWithIndent("}", depth);
		}
	}
}
