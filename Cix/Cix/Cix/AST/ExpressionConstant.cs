using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	/// <summary>
	/// Represents a constant value as an expression element.
	/// </summary>
	public sealed class ExpressionConstant : ExpressionElement
	{
		private byte[] value;
		private DataType Type;

		// Note: when emitting assembly for dealing with user-defined types, 
		// access to the value is going to be handled differently than with
		// BCL-analogous types.

		public ExpressionConstant(DataType type)
		{
			// Use this constructor for user-defined types
			this.Type = type;
			this.value = new byte[type.TypeSize];
		}

		public ExpressionConstant(bool value)
		{
			this.Type = new DataType("byte", 0, 1);
			this.value = new byte[] { value ? (byte)1 : (byte)0 };
		}

		public ExpressionConstant(byte value)
		{
			this.Type = new DataType("byte", 0, 1);
			this.value = new byte[] { value };
		}
	}
}
