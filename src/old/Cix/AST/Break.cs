using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class Break : Element
	{
		public override string ToString() => "Break";

		public override void Print(StringBuilder builder, int depth) =>
			builder.AppendLineWithIndent("Break", depth);
	}
}
