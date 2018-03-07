using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class ExpressionTypecast : ExpressionElement
	{
		public DataType ResultType { get; }

		public ExpressionTypecast(DataType resultType)
		{
			ResultType = resultType;
		}
	}
}
