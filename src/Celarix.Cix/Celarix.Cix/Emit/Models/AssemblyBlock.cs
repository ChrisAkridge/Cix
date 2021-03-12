using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celarix.Cix.Compiler.Emit.Models
{
    public sealed class AssemblyBlock
    {
        public string Name { get; set; }
        public List<AssemblyInstruction> Instructions { get; set; } = new List<AssemblyInstruction>();
    }
}
