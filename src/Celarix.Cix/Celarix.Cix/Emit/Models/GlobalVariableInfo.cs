using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celarix.Cix.Compiler.Parse.Models.AST.v1;

namespace Celarix.Cix.Compiler.Emit.Models
{
    public sealed class GlobalVariableInfo
    {
        public string Name { get; set; }
        public DataType Type { get; set; }
        public int OffsetFromHeaderEnd { get; set; }
    }
}
