using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	/// <summary>
	/// Represents a parameter in a function call.
	/// </summary>
	public sealed class ExpressionFunctionParameter : ExpressionElement
	{
		public Expression Value { get; private set; }

		public ExpressionFunctionParameter(Expression value)
		{
			this.Value = value;
		}
	}
}
