using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class ExpressionFunctionParameter
	{
		public DataType ParameterType { get; private set; }
		public Expression ValueExpression { get; private set; }

		public ExpressionFunctionParameter(DataType parameterType, Expression valueExpression)
		{
			ParameterType = parameterType;
			ValueExpression = valueExpression;
		}
	}
}
