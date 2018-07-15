using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	internal sealed class Function : Element
	{
		private List<FunctionArgument> arguments;
		private List<Element> statements;

		public DataType ReturnType { get; }
		public string Name { get; }
		public IReadOnlyList<FunctionArgument> Arguments => arguments.AsReadOnly();
		public IReadOnlyList<Element> Statements => statements.AsReadOnly();

		public Function(DataType returnType, string name, IEnumerable<FunctionArgument> arguments, 
			IEnumerable<Element> statements)
		{
			ReturnType = returnType;
			Name = name;
			this.arguments = arguments.ToList();
			this.statements = statements.ToList();
		}

		public Function WithStatements(IList<Element> statements)
		{
			return new Function(ReturnType, Name, arguments, statements);
		}

		public override string ToString()
		{
			StringBuilder argumentBuilder = new StringBuilder();
			foreach (var arg in arguments)
			{
				argumentBuilder.Append(arg.ToString());
				argumentBuilder.Append(", ");
			}

			return $"{ReturnType} {Name}({argumentBuilder.ToString()})";
		}
	}
}
