using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    internal sealed class VirtualStack
    {
        public Stack<VirtualStackEntry> Entries { get; set; }
        public int Size { get; private set; }

        public VirtualStackEntry GetEntry(string name) => Entries.Single(e => e.Name == name);

        public void Push(VirtualStackEntry stackEntry)
        {
            Entries.Push(stackEntry);
            stackEntry.OffsetFromEBP = Size;
            Size += stackEntry.UsageType.Size;
        }

        public VirtualStackEntry Pop()
        {
            var entry = Entries.Pop();
            Size -= entry.UsageType.Size;

            return entry;
        }

        public VirtualStackEntry Peek() => Entries.Peek();
    }
}