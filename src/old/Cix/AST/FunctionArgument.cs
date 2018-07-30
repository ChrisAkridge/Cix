using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class FunctionArgument : Element
	{
		public DataType Type { get; }
		public string ArgumentName { get; }

		public FunctionArgument(DataType type, string argumentName)
		{
			if (string.IsNullOrEmpty(argumentName))
			{
				throw new ArgumentException("The name of a function argument cannot be null or empty.");
			}

			Type = type;
			ArgumentName = argumentName;
		}

		public override void Print(StringBuilder builder, int depth)
		{
			builder.Append("Don't use Print() on function args, use ToString() instead");
		}

		public override string ToString() => $"{Type} {ArgumentName}";
	}
}
