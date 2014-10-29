using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class FunctionArgument : Element
	{
		public DataType Type { get; private set; }
		public string ArgumentName { get; private set; }

		public FunctionArgument(DataType type, string argumentName)
		{
			if (string.IsNullOrEmpty(argumentName))
			{
				throw new ArgumentException("The name of a function argument cannot be null or empty.");
			}

			this.Type = type;
			this.ArgumentName = argumentName;
		}
	}
}
