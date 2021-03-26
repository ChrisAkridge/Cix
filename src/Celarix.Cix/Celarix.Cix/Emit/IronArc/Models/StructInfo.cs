using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    internal sealed class StructInfo : NamedTypeInfo
    {
        public List<StructMemberInfo> MemberInfos { get; set; }
    }
}
