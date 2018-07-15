using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	internal sealed class StructDeclaration : Element
	{
		private List<StructMemberDeclaration> members;

		public string Name { get; }
		public IReadOnlyList<StructMemberDeclaration> Members => members.AsReadOnly();

		public int Size
		{
			get
			{
				if (Members != null)
				{
					return Members.Sum(m => m.Size);
				}
				return 0;
			}
		}

		public StructDeclaration(string name, IEnumerable<StructMemberDeclaration> members)
		{
			Name = name;
			this.members = members.ToList();
		}

		public DataType ToDataType() => new DataType(Name, 0, Size);
	}
}
