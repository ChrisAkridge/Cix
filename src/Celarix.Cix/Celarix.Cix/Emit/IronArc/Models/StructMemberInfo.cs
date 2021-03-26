using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celarix.Cix.Compiler.Parse.Models.AST.v1;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    internal sealed class StructMemberInfo
    {
        public DataType ASTType { get; set; }
        public TypeInfo UnderlyingType { get; set; }

        public string Name { get; set; }
        public int PointerLevel { get; set; }
        public int Size => (PointerLevel > 0) ? 8 : (UnderlyingType?.Size ?? 0);
        public int ArraySize { get; set; }
        public int Offset { get; set; }
    }
}
