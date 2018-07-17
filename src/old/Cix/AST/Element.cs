using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public abstract class Element
	{
		public abstract void Print(StringBuilder builder, int depth);
	}
}
