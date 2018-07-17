using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class ExpressionFunctionParameter
	{
		// Terminology note: In function declarations (int main(int argc, char** argv), they're called parameters.
		// In function calls (sort(&array)), they're called arguments.
		public DataType ParameterType { get; }
		public Expression ValueExpression { get; }
		public ExpressionElementSequence Sequence { get; }

		public ExpressionFunctionParameter(DataType parameterType, Expression valueExpression, 
			ExpressionElementSequence sequence)
		{
			ParameterType = parameterType;
			ValueExpression = valueExpression;
			Sequence = sequence;
		}
	}
}
