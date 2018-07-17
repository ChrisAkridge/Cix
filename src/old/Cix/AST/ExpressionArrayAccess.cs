using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class ExpressionArrayAccess : ExpressionElement
	{
		public Expression Indexer { get; }

		public ExpressionArrayAccess(Expression indexer) => Indexer = indexer;

		public override string ToString() =>
			"[" + Indexer.ToString() + "]";
	}
}
