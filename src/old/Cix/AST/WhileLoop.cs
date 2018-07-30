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

		public override void Print(StringBuilder builder, int depth)
		{
			builder.AppendLineWithIndent("While", depth);
			builder.AppendLineWithIndent($"Condition: {Condition}", depth + 1);
			builder.AppendLineWithIndent("Statements: {", depth + 1);

			foreach (Element statement in statements)
			{
				statement.Print(builder, depth + 2);
			}

			builder.AppendLineWithIndent("}", depth + 1);
		}
	}
}
