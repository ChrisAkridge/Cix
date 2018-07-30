using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class ReturnValue : Element
	{
		public Expression ReturnExpression { get; }

		public ReturnValue(Expression returnExpression)
		{
			ReturnExpression = returnExpression;
		}

		public override void Print(StringBuilder builder, int depth)
		{
			builder.AppendLineWithIndent($"return {ReturnExpression}", depth);
		}
	}
}
