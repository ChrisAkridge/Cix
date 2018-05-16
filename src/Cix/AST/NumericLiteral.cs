using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	// Probably will remove in favor of ExpressionConstant
	public sealed class NumericLiteral : ExpressionElement
	{
		private ulong integralValue;
		private double floatingValue;

		// TODO: store the text of the literal instead of the value
		// we can get the value on demand
		public LiteralType LiteralType { get; }
		public Type UnderlyingType { get; }

		public long SignedIntegralValue
		{
			get
			{
				if (LiteralType != LiteralType.SignedIntegral)
				{
					throw new InvalidOperationException($"Tried to read a numeric literal as a signed integer, but it is actually a {LiteralType}.");
				}

				unchecked
				{
					return (long)integralValue;
				}
			}
		}

		public ulong UnsignedIntegralValue
		{
			get
			{
				if (LiteralType != LiteralType.UnsignedIntegral)
				{
					throw new InvalidOperationException($"Tried to read a numeric literal as an unsigned integer, but it is actually a {LiteralType}.");
				}

				return integralValue;
			}
		}

		public double FloatingValue
		{
			get
			{
				if (LiteralType != LiteralType.Floating)
				{
					throw new InvalidOperationException($"Tried to read a numeric literal as a floating point number, but it is actually a {LiteralType}.");
				}

				return floatingValue;
			}
		}

		public NumericLiteral(byte value)
		{
			LiteralType = LiteralType.UnsignedIntegral;
			UnderlyingType = typeof(byte);
			integralValue = value;
		}

		public NumericLiteral(sbyte value)
		{
			LiteralType = LiteralType.SignedIntegral;
			UnderlyingType = typeof(sbyte);
			integralValue = unchecked((ulong)value);
		}

		public NumericLiteral(short value)
		{
			LiteralType = LiteralType.SignedIntegral;
			UnderlyingType = typeof(short);
			integralValue = unchecked((ulong)value);
		}

		public NumericLiteral(ushort value)
		{
			LiteralType = LiteralType.UnsignedIntegral;
			UnderlyingType = typeof(ushort);
			integralValue = value;
		}

		public NumericLiteral(int value)
		{
			LiteralType = LiteralType.SignedIntegral;
			UnderlyingType = typeof(int);
			integralValue = unchecked((ulong)value);
		}

		public NumericLiteral(uint value)
		{
			LiteralType = LiteralType.UnsignedIntegral;
			UnderlyingType = typeof(uint);
			integralValue = value;
		}

		public NumericLiteral(long value)
		{
			LiteralType = LiteralType.SignedIntegral;
			UnderlyingType = typeof(long);
			integralValue = unchecked((ulong)value);
		}

		public NumericLiteral(ulong value)
		{
			LiteralType = LiteralType.UnsignedIntegral;
			UnderlyingType = typeof(ulong);
			integralValue = value;
		}

		public NumericLiteral(float value)
		{
			LiteralType = LiteralType.Floating;
			UnderlyingType = typeof(float);
			floatingValue = value;
		}

		public NumericLiteral(double value)
		{
			LiteralType = LiteralType.Floating;
			UnderlyingType = typeof(double);
			floatingValue = value;
		}

		public static NumericLiteral Parse(string value)
		{
			if (!value.IsNumericLiteral())
			{
				throw new ArgumentException("Tried to parse a numeric literal that wasn't actually a numeric literal.");
			}

			value = value.ToLowerInvariant();

			if (value.EndsWith("f"))
			{
				float result = 0f;
				float.TryParse(value, out result);
				return new NumericLiteral(result);
			}
			else if (value.EndsWith("d") || value.Contains('.'))
			{
				double result = 0d;
				double.TryParse(value, out result);
				return new NumericLiteral(result);
			}
			else if (value.EndsWith("l"))
			{
				long result = 0L;
				long.TryParse(value, out result);
				return new NumericLiteral(result);
			}
			else if (value.EndsWith("ul"))
			{
				ulong result = 0UL;
				ulong.TryParse(value, out result);
				return new NumericLiteral(result);
			}
			else if (value.EndsWith("u"))
			{
				ulong result = 0UL;
				ulong.TryParse(value, out result);

				if (result > uint.MaxValue)
				{
					return new NumericLiteral(result);
				}
				else
				{
					return new NumericLiteral((uint)result);
				}
			}
			else
			{
				long result = 0L;
				long.TryParse(value, out result);

				if (result > int.MaxValue)
				{
					return new NumericLiteral(result);
				}
				else
				{
					return new NumericLiteral((int)result);
				}
			}

			throw new InvalidProgramException("Unreachable.");
		}
	}

	public enum LiteralType
	{
		SignedIntegral,
		UnsignedIntegral,
		Floating
	}
}
