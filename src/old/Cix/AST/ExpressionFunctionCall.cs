using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class ExpressionFunctionCall : ExpressionElement
	{
		// TODO: you got this backwards; this is an argument, not a parameter
		private List<ExpressionFunctionParameter> parameters;

		public string FunctionName { get; }
		public DataType FunctionReturnType { get; }
		public IReadOnlyList<ExpressionFunctionParameter> Parameters => parameters.AsReadOnly();

		public ExpressionFunctionCall(string functionName, 
			DataType functionReturnType, params ExpressionFunctionParameter[] parameters)
		{
			FunctionName = functionName;
			FunctionReturnType = functionReturnType;

			this.parameters = parameters.ToList();
		}
	}
}
