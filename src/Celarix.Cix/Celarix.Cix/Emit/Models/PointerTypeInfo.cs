using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celarix.Cix.Compiler.Emit.Models
{
    public sealed class PointerTypeInfo : TypeInfo
    {
        public TypeInfo TypeInfo { get; set; }
        public int PointerLevel { get; set; }
        public override int Size => 8;
    }
}
