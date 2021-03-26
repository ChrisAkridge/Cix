using System;
using System.Collections.Generic;
using System.Linq;
using Celarix.Cix.Compiler.Parse.Models.AST.v1;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    internal sealed class GlobalVariableInfo
    {
        public string Name { get; set; }
        public UsageTypeInfo UsageType { get; set; }
        public int PointerLevel { get; set; }
        public int OffsetFromERPPlusHeader { get; set; }
    }
}