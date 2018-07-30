using System;
using System.Collections.Generic;
using System.Globalization;
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

		public override string ToString()
		{
			switch (Type.Name)
			{
				case "byte": return value[0].ToString();
				case "sbyte": return ((sbyte)value[0]).ToString();
				case "short": return BitConverter.ToInt16(value, 0).ToString();
				case "ushort": return BitConverter.ToUInt16(value, 0).ToString();
				case "int": return BitConverter.ToInt32(value, 0).ToString();
				case "uint": return BitConverter.ToUInt32(value, 0).ToString();
				case "long": return BitConverter.ToInt64(value, 0).ToString();
				case "ulong": return BitConverter.ToUInt64(value, 0).ToString();
				case "float": return BitConverter.ToSingle(value, 0).ToString(CultureInfo.InvariantCulture);
				case "double": return BitConverter.ToDouble(value, 0).ToString(CultureInfo.InvariantCulture);
				case "lpstring":
					int stringLength = value.Length - 4;
					return Encoding.UTF8.GetString(value, 4, stringLength);
				default: return "Unknown Constant";
			}
		}
	}
}
