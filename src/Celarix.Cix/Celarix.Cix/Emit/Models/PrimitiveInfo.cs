using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celarix.Cix.Compiler.Emit.Models
{
    public sealed class PrimitiveInfo : DependencyVertex
    {
        public int Size { get; set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() => $"Primitive {Name}";
    }
}
