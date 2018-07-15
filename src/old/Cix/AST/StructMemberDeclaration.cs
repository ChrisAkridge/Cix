using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	internal sealed class StructMemberDeclaration : Element
	{
		public DataType Type { get; }
		public string Name { get; }
		public int ArraySize { get; }
		public int Offset { get; }

		public int Size => Type.TypeSize * ArraySize;

		public StructMemberDeclaration(DataType type, string name, int arraySize, int offset)
		{
			Type = type;
			Name = name;
			ArraySize = arraySize;
			Offset = offset;
		}
	}
}
