using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class GlobalVariableDeclaration : Element
	{
		public DataType Type { get; }
		public int ArraySize { get; }
		public string Name { get; }
		public ExpressionConstant InitialValue { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="GlobalVariableDeclaration"/> class.
		/// </summary>
		/// <param name="type">The type of the global variable.</param>
		/// <param name="name">The name of the global variable.</param>
		/// <param name="initialValue">The global variable's initial value. Pass null for all-bits-zero.</param>
		public GlobalVariableDeclaration(DataType type, string name, ExpressionConstant initialValue)
		{
			Type = type;
			Name = name;
			ArraySize = 1;
			InitialValue = initialValue;
		}

		public GlobalVariableDeclaration(DataType type, string name, int arraySize)
		{
			Type = type;
			Name = name;
			ArraySize = arraySize;
		}

		public override void Print(StringBuilder builder, int depth)
		{
			string declaration = $"global {Type} {Name}";
			string initializer = (InitialValue != null) ? $" = {InitialValue}" : "";
			builder.AppendLineWithIndent($"{declaration}{initializer}", depth);
		}
	}
}
