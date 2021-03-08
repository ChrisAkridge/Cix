using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celarix.Cix.Compiler.Emit.Models
{
    public abstract class DependencyVertex
    {
        public string Name { get; set; }
        
        public int Size { get; set; }
    }
}
