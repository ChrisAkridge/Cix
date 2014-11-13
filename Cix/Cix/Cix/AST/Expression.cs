using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class Expression : Element
	{
		private List<ExpressionElement> elements;

		public ReadOnlyCollection<ExpressionElement> Elements
		{
			get
			{
				return this.elements.AsReadOnly() ?? null;
			}
		}

		public static Expression Parse(IEnumerable<Token> tokens)
		{
			// TODO: implement
			return null;
		}
	}
}
