using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class VariableDeclaration : Element
	{
		public DataType Type { get; private set; }
		public string Name { get; private set; }

		public VariableDeclaration(DataType type, string name)
		{
			this.Type = type;
			this.Name = name;
		}
	}
}
