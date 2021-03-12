using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celarix.Cix.Compiler.Emit.Models
{
    public sealed class VirtualStackEntity
    {
        public string Name { get; set; }
        public int OffsetFromEBP { get; set; }
        public int Size { get; set; }
    }
}
