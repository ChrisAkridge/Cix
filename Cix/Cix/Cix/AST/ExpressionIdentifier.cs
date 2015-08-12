using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class Identifier : Element
	{
		public string Name { get; }

		public Identifier(string name)
		{
			Name = name;
		}
	}
}
