using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	internal sealed class DataType : Element
	{
		public string TypeName { get; }
		public int PointerLevel { get; }
		public int TypeSize { get; }

		public DataType(string typeName, int pointerLevel, int typeSize)
		{
			if (string.IsNullOrEmpty(typeName))
			{
				throw new ArgumentException("Invalid name for data type.");
			}

			if (pointerLevel < 0)
			{
				throw new ArgumentException($"Invalid pointer level {pointerLevel}.");
			}

			if (typeSize <= 0)
			{
				throw new ArgumentException($"Invalid type size {typeSize}.");
			}

			TypeName = typeName;
			PointerLevel = pointerLevel;
			TypeSize = (pointerLevel == 0) ? typeSize : 8;
		}

		public Type GetBCLType()
		{
			if (PointerLevel != 0)
			{
				throw new InvalidOperationException("This type is a pointer type; for type safety Cix cannot return the analogous BCL type.");
			}

			switch (TypeName)
			{
				case "byte":
					return typeof(byte);
				case "sbyte":
					return typeof(sbyte);
				case "short":
					return typeof(short);
				case "ushort":
					return typeof(ushort);
				case "int":
					return typeof(int);
				case "uint":
					return typeof(uint);
				case "long":
					return typeof(long);
				case "ulong":
					return typeof(ulong);
				case "float":
					return typeof(float);
				case "double":
					return typeof(double);
				case "char":
					return typeof(char);
				case "void":
					return typeof(void);
				default:
					throw new InvalidOperationException($"The Cix type {TypeName} does not have an analogous BCL type.");
			}
		}

		public DataType WithPointerLevel(int pointerLevel)
		{
			return new DataType(TypeName, pointerLevel, TypeSize);
		}

		public override string ToString()
		{
			return $"{TypeName}{new string('*', PointerLevel)} ({TypeSize} byte(s))";
		}
	}
}
