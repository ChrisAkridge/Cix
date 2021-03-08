using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph;

namespace Celarix.Cix.Compiler.Emit.Models
{
    internal sealed class DependencyEdge : IEdge<DependencyVertex>
    {
        public DependencyGraphEdgeType EdgeType { get; set; }
        public DependencyVertex Source { get; set; }
        public DependencyVertex Target { get; set; }
    }
}
