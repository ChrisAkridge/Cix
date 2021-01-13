using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class HardwareCall : ExpressionElement
	{
		private readonly List<ExpressionFunctionArgument> arguments;
		
		public Expression HardwareFunctionNameExpression { get; }
		public IReadOnlyList<ExpressionFunctionArgument> Arguments => arguments.AsReadOnly();

		public HardwareCall(Expression hardwareFunctionNameExpression,
			IEnumerable<ExpressionFunctionArgument> arguments)
		{
			HardwareFunctionNameExpression = hardwareFunctionNameExpression;
			this.arguments = arguments.ToList();
		}
	}
}
