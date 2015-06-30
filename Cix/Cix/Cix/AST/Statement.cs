using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class Statement
	{
		private List<Element> elements;

		public IReadOnlyList<Element> Elements { get { return this.elements.AsReadOnly(); } }

		public Statement(IEnumerable<Element> elements)
		{
			this.elements = elements.ToList();
		}
	}
}
