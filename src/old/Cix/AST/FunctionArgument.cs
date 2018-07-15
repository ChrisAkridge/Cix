using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	internal sealed class FunctionArgument : Element
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

		public override string ToString()
		{
			return $"{Type} {ArgumentName}";
		}
	}
}
