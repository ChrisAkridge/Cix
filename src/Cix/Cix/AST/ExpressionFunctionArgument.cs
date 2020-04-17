using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class ExpressionFunctionArgument
	{
		// Terminology note: In function declarations (int main(int argc, char** argv), they're called parameters.
		// In function calls (sort(&array)), they're called arguments.
		public DataType Type { get; }
		public Expression ValueExpression { get; }

		public ExpressionFunctionArgument(DataType type, Expression valueExpression)
		{
			Type = type;
			ValueExpression = valueExpression;
		}

		public override string ToString() => ValueExpression.ToString();
	}
}
