using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class Function : Element
	{
		public DataType ReturnType { get; private set; }
		public string Name { get; private set; }
		public List<FunctionArgument> Arguments;
		public List<Expression> Statements;

		public Function(DataType returnType, string name, IEnumerable<FunctionArgument> arguments, IEnumerable<Expression> statements)
		{
			this.ReturnType = returnType;
			this.Name = name;
			this.Arguments = arguments.ToList();
			this.Statements = statements.ToList();
		}
	}
}
