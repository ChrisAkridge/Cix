using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	internal sealed class GlobalVariableDeclaration : Element
	{
		public DataType Type { get; }
		public string Name { get; }
		public NumericLiteral InitialValue { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="GlobalVariableDeclaration"/> class.
		/// </summary>
		/// <param name="type">The type of the global variable.</param>
		/// <param name="name">The name of the global variable.</param>
		/// <param name="initialValue">The global variable's initial value. Pass null for all-bits-zero.</param>
		public GlobalVariableDeclaration(DataType type, string name, NumericLiteral initialValue)
		{
			Type = type;
			Name = name;
			InitialValue = initialValue;
		}
	}
}
