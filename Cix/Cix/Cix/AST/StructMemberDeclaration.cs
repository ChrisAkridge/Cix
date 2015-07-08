using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class StructMemberDeclaration : Element
	{
		public DataType MemberType { get; private set; }
		public string MemberName { get; private set; }

		public StructMemberDeclaration(DataType memberType, string memberName)
		{
			this.MemberType = memberType;
			this.MemberName = memberName;
		}
	}
}
