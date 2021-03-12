using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celarix.Cix.Compiler.Emit.Models
{
    public abstract class TypeInfo
    {
        public string Name { get; set; }
        
        public virtual int Size { get; set; }
    }
}
