using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class Function : Element
	{
		private readonly List<FunctionParameter> parameters;
		private readonly List<Element> statements;

		public DataType ReturnType { get; }
		public string Name { get; }
		public IReadOnlyList<FunctionParameter> Parameters => parameters.AsReadOnly();
		public IReadOnlyList<Element> Statements => statements.AsReadOnly();

		public Function(DataType returnType, string name, IEnumerable<FunctionParameter> parameters, 
			IEnumerable<Element> statements)
		{
			ReturnType = returnType;
			Name = name;
			this.parameters = parameters.ToList();
			this.statements = statements.ToList();
		}

		public Function WithStatements(IList<Element> newStatements) 
			=> new Function(ReturnType, Name, parameters, newStatements);

		public override void Print(StringBuilder builder, int depth)
		{
			string typeAndName = $"{ReturnType} {Name}";
			string parameterString = "(" + string.Join(", ", parameters.Select(p => p.ToString()).ToArray()) + ")";

			builder.AppendLineWithIndent(typeAndName + " " + parameterString + " {", depth);

			foreach (Element statement in statements)
			{
				statement.Print(builder, depth + 1);
			}

			builder.AppendLineWithIndent("}", depth);
		}
	}
}
