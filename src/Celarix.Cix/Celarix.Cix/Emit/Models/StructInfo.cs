using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celarix.Cix.Compiler.Emit.Models
{
    public sealed class StructInfo : TypeInfo
    {
        public List<StructMemberInfo> Members { get; set; }
        public override int Size { get; set; }
    }
}
