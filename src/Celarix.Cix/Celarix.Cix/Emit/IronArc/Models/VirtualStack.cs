using System;
using System.Collections.Generic;
using System.Linq;
using NLog;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    internal sealed class VirtualStack
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        
        public Stack<VirtualStackEntry> Entries { get; set; } = new Stack<VirtualStackEntry>();
        public int Size { get; private set; }

        public VirtualStackEntry GetEntry(string name) => Entries.Single(e => e.Name == name);

        public void Push(VirtualStackEntry stackEntry)
        {
            Entries.Push(stackEntry);
            stackEntry.OffsetFromEBP = Size;
            Size += stackEntry.UsageType.Size;

            logger.Trace($"Pushed {stackEntry.Name ?? "stack entry"} ({stackEntry.UsageType.Size} bytes) at EBP+{stackEntry.OffsetFromEBP} ({Size} bytes of stack)");
        }

        public VirtualStackEntry Pop()
        {
            var entry = Entries.Pop();
            Size -= entry.UsageType.Size;
            
            logger.Trace($"Popped {entry.Name ?? "stack entry"} ({entry.UsageType.Size} bytes) from EBP+{entry.OffsetFromEBP}  ({Size} bytes of stack)");

            return entry;
        }

        public VirtualStackEntry Peek() => Entries.Peek();

        public void Clear()
        {
            logger.Trace(Size > 0
                ? $"Stack cleared, was {Peek().OffsetFromEBP} bytes in size"
                : $"Stack cleared, was empty");
            Entries.Clear();
            Size = 0;
        }
    }
}