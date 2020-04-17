using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class StructDeclaration : Element
	{
		private readonly List<StructMemberDeclaration> members;

		public string Name { get; }
		public IReadOnlyList<StructMemberDeclaration> Members => members.AsReadOnly();

		public int Size => Members?.Sum(m => m.Size) ?? 0;

		public StructDeclaration(string name, IEnumerable<StructMemberDeclaration> members)
		{
			Name = name;
			this.members = members.ToList();
		}

		public DataType ToDataType() => new DataType(Name, 0, Size);

		public override void Print(StringBuilder builder, int depth)
		{
			builder.AppendLineWithIndent($"struct {Name} {{", depth);

			foreach (StructMemberDeclaration member in members)
			{
				member.Print(builder, depth + 1);
			}

			builder.AppendLineWithIndent("}", depth);
		}
	}
}
