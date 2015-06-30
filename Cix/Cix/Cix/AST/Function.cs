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
		private List<Statement> statements;

		public DataType ReturnType { get; private set; }
		public string Name { get; private set; }
		public IReadOnlyList<FunctionArgument> Arguments { get { return this.arguments.AsReadOnly(); } }
		public IReadOnlyList<Statement> Statements { get { return this.statements.AsReadOnly(); } }

		public Function(DataType returnType, string name, IEnumerable<FunctionArgument> arguments, IEnumerable<Statement> statements)
		{
			this.ReturnType = returnType;
			this.Name = name;
			this.arguments = arguments.ToList();
			this.statements = statements.ToList();
		}
	}
}
