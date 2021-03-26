using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    internal sealed class VirtualStack
    {
        public Stack<VirtualStackEntry> Entries { get; set; }

        public VirtualStackEntry GetEntry(string name) => Entries.Single(e => e.Name == name);
    }
}