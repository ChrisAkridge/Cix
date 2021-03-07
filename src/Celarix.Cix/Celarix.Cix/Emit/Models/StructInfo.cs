using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celarix.Cix.Compiler.Emit.Models
{
    public sealed class StructInfo
    {
        public string Name { get; set; }
        public List<StructMemberInfo> Members { get; set; }
        public int Size => Members.Sum(m => m.Size);
    }
}
