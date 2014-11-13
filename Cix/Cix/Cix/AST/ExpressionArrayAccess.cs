using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class ExpressionArrayAccess : ExpressionElement
	{
		public string ArrayName { get; private set; }
		public DataType ArrayType { get; private set; }
		public Expression IndexExpression { get; private set; }

		public ExpressionArrayAccess(string arrayName, DataType arrayType, Expression indexExpression)
		{
			this.ArrayName = arrayName;
			this.ArrayType = arrayType;
			this.IndexExpression = indexExpression;
		}
	}
}
