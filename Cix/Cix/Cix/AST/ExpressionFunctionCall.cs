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
		public string FunctionName { get; private set; }
		public DataType FunctionReturnType { get; private set; }
		public List<ExpressionFunctionParameter> Parameters { get; }

		public ExpressionFunctionCall(string functionName, DataType functionReturnType, params ExpressionFunctionParameter[] parameters)
		{
			FunctionName = functionName;
			FunctionReturnType = functionReturnType;

			this.Parameters = new List<ExpressionFunctionParameter>();
			foreach (var parameter in parameters)
			{
				this.Parameters.Add(parameter);
			}
		}
	}
}
