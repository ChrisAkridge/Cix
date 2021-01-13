using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class SwitchBlock : Element
	{
		private readonly List<SwitchCase> switchCases;

		public Expression SwitchExpression { get; }
		public IReadOnlyList<SwitchCase> SwitchCases => switchCases.AsReadOnly();

		public SwitchBlock(Expression switchExpression, IEnumerable<SwitchCase> switchCases)
		{
			SwitchExpression = switchExpression;
			this.switchCases = switchCases.ToList();
		}

		public override void Print(StringBuilder builder, int depth)
		{
			builder.AppendLineWithIndent($"switch ({SwitchExpression}) {{", depth);

			foreach (SwitchCase switchCase in switchCases)
			{
				switchCase.Print(builder, depth + 1);
			}

			builder.AppendLineWithIndent("}", depth);
		}
	}
}
