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

		public Function WithStatements(IList<Element> statements) => new Function(ReturnType, Name, arguments, statements);

		public override void Print(StringBuilder builder, int depth)
		{
			string typeAndName = $"{ReturnType} {Name}";
			string parameters = "(" + string.Join(", ", arguments.Select(p => p.ToString()).ToArray()) + ")";;

			builder.AppendLineWithIndent(typeAndName + " " + parameters + " {", depth);

			foreach (Element statement in statements)
			{
				statement.Print(builder, depth + 1);
			}

			builder.AppendLineWithIndent("}", depth);
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
