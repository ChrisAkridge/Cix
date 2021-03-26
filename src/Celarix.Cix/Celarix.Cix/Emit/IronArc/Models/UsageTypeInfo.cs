using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    internal sealed class UsageTypeInfo
    {
        public TypeInfo DeclaredType { get; set; }
        public int PointerLevel { get; set; }
        public bool IsAssignable { get; set; }
        public int Size => (PointerLevel > 0) ? 8 : DeclaredType.Size;

        public static UsageTypeInfo FromTypeInfo(TypeInfo type, int pointerLevel, bool isAssignable = false) =>
            new UsageTypeInfo
            {
                DeclaredType = type,
                PointerLevel = pointerLevel,
                IsAssignable = isAssignable
            };
    }
}
