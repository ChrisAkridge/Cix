using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celarix.Cix.Compiler.Emit.Models
{
    public sealed class FuncptrTypeInfo : TypeInfo
    {
        public override int Size => 8;
        public TypeInfo ReturnType { get; set; }
        public List<TypeInfo> ParameterTypes { get; set; }
    }
}
