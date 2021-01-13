using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class Return : Element
	{
		public override void Print(StringBuilder builder, int depth)
		{
			builder.AppendLineWithIndent("return", depth);
		}
	}
}
