using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cix.Errors;

namespace Cix.AST
{
	/// <summary>
	/// Represents a constant value as an expression element.
	/// </summary>
	public sealed class ExpressionConstant : ExpressionElement
	{
		private byte[] value;

		public IReadOnlyList<byte> Value => new ReadOnlyCollection<byte>(value);
		public DataType Type { get; }

		public ExpressionConstant(bool value)
		{
			Type = new DataType("byte", 0, 1);
			this.value = new[] { value ? (byte)1 : (byte)0 };
		}

		public ExpressionConstant(byte value)
		{
			Type = new DataType("byte", 0, 1);
			this.value = new[] { value };
		}

		public ExpressionConstant(sbyte value)
		{
			Type = new DataType("sbyte", 0, 1);
			this.value = new[] { unchecked((byte)value) };
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

		public byte ToByte() => value[0];
		public sbyte ToSByte() => unchecked((sbyte)value[0]);
		public short ToShort() => BitConverter.ToInt16(value, 0);
		public ushort ToUShort() => BitConverter.ToUInt16(value, 0);
		public int ToInt() => BitConverter.ToInt32(value, 0);
		public uint ToUInt() => BitConverter.ToUInt32(value, 0);
		public long ToLong() => BitConverter.ToInt64(value, 0);
		public ulong ToULong() => BitConverter.ToUInt64(value, 0);
		public float ToFloat() => BitConverter.ToSingle(value, 0);
		public double ToDouble() => BitConverter.ToDouble(value, 0);
		public string ToCLRString() => Encoding.UTF8.GetString(value, 4, value.Length - 4);

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
