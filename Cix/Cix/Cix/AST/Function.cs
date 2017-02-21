using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class Function : Element
	{
		private List<FunctionArgument> arguments;
		private List<Expression> statements;

		public DataType ReturnType { get; }
		public string Name { get; }
		public IReadOnlyList<FunctionArgument> Arguments => arguments.AsReadOnly();
		public IReadOnlyList<Expression> Statements => statements.AsReadOnly();

		public Function(DataType returnType, string name, IEnumerable<FunctionArgument> arguments, 
			IEnumerable<Expression> statements)
		{
			ReturnType = returnType;
			Name = name;
			this.arguments = arguments.ToList();
			this.statements = statements.ToList();
		}
	}
}
