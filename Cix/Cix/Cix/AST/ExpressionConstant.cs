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

		// Note: never mind, user-defined types can't be constants

		[Obsolete]
		public ExpressionConstant(DataType type)
		{
			// Use this constructor for user-defined types
			Type = type;
			value = new byte[type.TypeSize];
		}

		public ExpressionConstant(bool value)
		{
			Type = new DataType("byte", 0, 1);
			this.value = new byte[] { value ? (byte)1 : (byte)0 };
		}

		public ExpressionConstant(byte value)
		{
			Type = new DataType("byte", 0, 1);
			this.value = new byte[] { value };
		}

		public ExpressionConstant(sbyte value)
		{
			Type = new DataType("sbyte", 0, 1);
			this.value = new byte[] { unchecked((byte)value) };
		}

		public ExpressionConstant(short value)
		{
			Type = new DataType("short", 0, 2);
			this.value = BitConverter.GetBytes(value);
		}

		public ExpressionConstant(ushort value)
		{
			Type = new DataType("ushort", 0, 2);
			this.value = BitConverter.GetBytes(value);
		}

		public ExpressionConstant(int value)
		{
			Type = new DataType("int", 0, 4);
			this.value = BitConverter.GetBytes(value);
		}

		public ExpressionConstant(uint value)
		{
			Type = new DataType("uint", 0, 4);
			this.value = BitConverter.GetBytes(value);
		}

		public ExpressionConstant(long value)
		{
			Type = new DataType("long", 0, 8);
			this.value = BitConverter.GetBytes(value);
		}

		public ExpressionConstant(ulong value)
		{
			Type = new DataType("ulong", 0, 8);
			this.value = BitConverter.GetBytes(value);
		}

		public ExpressionConstant(float value)
		{
			Type = new DataType("float", 0, 4);
			this.value = BitConverter.GetBytes(value);
		}

		public ExpressionConstant(double value)
		{
			Type = new DataType("double", 0, 8);
			this.value = BitConverter.GetBytes(value);
		}

		public ExpressionConstant(string value)
		{
			byte[] utf8 = Encoding.UTF8.GetBytes(value);
			uint stringLength = (uint)utf8.Length;

			Type = new DataType("lpstring", 0, (int)stringLength + 4);
			this.value = BitConverter.GetBytes(stringLength).Concat(utf8).ToArray();
		}
	}
}
