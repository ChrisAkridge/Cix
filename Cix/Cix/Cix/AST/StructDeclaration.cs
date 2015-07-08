using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class StructDeclaration : Element
	{
		public string Name { get; private set; }
		public List<StructMemberDeclaration> Members { get; private set; }

		public int Size
		{
			get
			{
				if (this.Members != null)
				{
					return this.Members.Sum(m => m.MemberType.TypeSize);
				}
				return 0;
			}
		}

		public StructDeclaration(string name, IEnumerable<StructMemberDeclaration> members)
		{
			this.Name = name;
			this.Members = members.ToList();
		}
	}
}
