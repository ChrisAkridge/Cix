using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Cix.AST
{
    public sealed class StructDataType : DataType
    {
        private List<StructMemberDeclaration> members;
        public IReadOnlyList<StructMemberDeclaration> Members => members.AsReadOnly();
        public override int Size => Members.Sum(m => m.Type.Size);

        public StructDataType(string name, int pointerLevel, IList<StructMemberDeclaration> members)
            : base(name, pointerLevel, 0)
        {
            this.members = (List<StructMemberDeclaration>)members;
        }

        public override DataType WithPointerLevel(int pointerLevel) =>
            new StructDataType(Name, pointerLevel, members);
    }
}
