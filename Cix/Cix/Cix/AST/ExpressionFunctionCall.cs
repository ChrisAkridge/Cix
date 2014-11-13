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

		private List<ExpressionFunctionParameter> parameters;
		
		public ReadOnlyCollection<ExpressionFunctionParameter> Parameters
		{
			get
			{
				return (this.parameters != null) ? this.parameters.AsReadOnly() : null;
			}
		}

		public ExpressionFunctionCall(string functionName, DataType functionReturnType, params ExpressionFunctionParameter[] parameters)
		{
			this.FunctionName = functionName;
			this.FunctionReturnType = functionReturnType;

			this.parameters = new List<ExpressionFunctionParameter>();
			foreach (var parameter in parameters)
			{
				this.parameters.Add(parameter);
			}
		}
	}
}
