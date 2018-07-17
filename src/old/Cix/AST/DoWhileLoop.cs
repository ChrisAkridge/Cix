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

		public override void Print(StringBuilder builder, int depth)
		{
			builder.AppendLineWithIndent(depth, "Do {");

			foreach (Element statement in statements)
			{
				statement.Print(builder, depth + 1);
			}

			builder.AppendLineWithIndent(depth, $"}} While ({Condition})");
		}
	}
}
