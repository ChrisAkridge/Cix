using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celarix.Cix.Compiler.Parse.Models.AST.v1;

namespace Celarix.Cix.Compiler.Emit.Models
{
    public sealed class StructMemberInfo
    {
        public string Name { get; set; }
        public DataType Type { get; set; }
        public int Size { get; set; }
        public int Offset { get; set; }
    }
}
