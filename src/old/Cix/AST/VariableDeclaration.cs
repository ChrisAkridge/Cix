using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	internal sealed class VariableDeclaration : Element
	{
		public DataType Type { get; }
		public string Name { get; }

		public VariableDeclaration(DataType type, string name)
		{
			Type = type;
			Name = name;
		}
	}
}
