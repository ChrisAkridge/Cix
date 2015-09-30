using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class ExpressionFunctionParameter
	{
		// Terminology note: In function declarations (int main(int argc, char** argv), they're called arguments.
		// In function calls (sort(&array)), they're called parameters.
		public DataType ParameterType { get; private set; }
		public Expression ValueExpression { get; private set; }
		public ExpressionElementSequence Sequence { get; private set; }

		public ExpressionFunctionParameter(DataType parameterType, Expression valueExpression, ExpressionElementSequence sequence)
		{
			ParameterType = parameterType;
			ValueExpression = valueExpression;
			Sequence = sequence;
		}
	}
}
