using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public class DataType : Element
	{
		public virtual string Name { get; }
		public int PointerLevel { get; }
		public int Size { get; }

		public DataType(string name, int pointerLevel, int size)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException("Invalid name for data type.");
			}

			if (pointerLevel < 0)
			{
				throw new ArgumentException($"Invalid pointer level {pointerLevel}.");
			}

			if (size <= 0)
			{
				throw new ArgumentException($"Invalid type size {size}.");
			}

			Name = name;
			PointerLevel = pointerLevel;
			Size = (pointerLevel == 0) ? size : 8;
		}

		public Type GetBCLType()
		{
			if (PointerLevel != 0)
			{
				throw new InvalidOperationException("This type is a pointer type; for type safety Cix cannot return the analogous BCL type.");
			}

			switch (Name)
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
					throw new InvalidOperationException($"The Cix type {Name} does not have an analogous BCL type.");
			}
		}

		public DataType WithPointerLevel(int pointerLevel) => new DataType(Name, pointerLevel, Size);

		public override string ToString() => $"{Name}{new string('*', PointerLevel)}";

		public override void Print(StringBuilder builder, int depth) =>
			builder.AppendLineWithIndent(ToString(), depth);
	}
}
