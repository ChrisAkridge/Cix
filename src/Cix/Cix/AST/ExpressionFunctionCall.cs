using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	// this needs changing, too
	// Function calls aren't just myFunc(i), they're also (funcArray[5])(i)
	public sealed class ExpressionFunctionCall : ExpressionElement
	{
		// TODO: you got this backwards; this is an argument, not a parameter
		private readonly List<ExpressionFunctionArgument> arguments;

		public Expression FunctionExpression { get; }
		public DataType FunctionReturnType { get; }
		public IReadOnlyList<ExpressionFunctionArgument> Arguments => arguments.AsReadOnly();

		public ExpressionFunctionCall(Expression functionExpression, 
			DataType functionReturnType, params ExpressionFunctionArgument[] arguments)
		{
			FunctionExpression = functionExpression;
			FunctionReturnType = functionReturnType;

			this.arguments = arguments.ToList();
		}

		public override string ToString()
		{
			bool functionExpressionIsFuncPtr = (FunctionExpression.Elements.Count == 1 &&
			                                    FunctionExpression.Elements[0] is ExpressionIdentifier);

			string functionExpressionString = (!functionExpressionIsFuncPtr)
				? ((ExpressionIdentifier) FunctionExpression.Elements[0]).Name
				: "(" + FunctionExpression + ")";

			return $"{functionExpressionString}({string.Join(", ", arguments.Select(p => p.ToString()).ToArray())})";
		}
	}
}
