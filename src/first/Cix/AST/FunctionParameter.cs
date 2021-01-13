using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class FunctionParameter : Element
	{
		public DataType Type { get; }
		public string Name { get; }

		public FunctionParameter(DataType type, string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException("The name of a function argument cannot be null or empty.");
			}

			Type = type;
			Name = name;
		}

		public override void Print(StringBuilder builder, int depth)
		{
			builder.Append("Don't use Print() on function args, use ToString() instead");
		}

		public override string ToString() => $"{Type} {Name}";
	}
}
