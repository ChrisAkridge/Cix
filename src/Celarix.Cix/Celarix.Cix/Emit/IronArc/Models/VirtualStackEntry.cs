using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    internal sealed class VirtualStackEntry
    {
        public string Name { get; set; }
        public UsageTypeInfo UsageType { get; set; }
        public int OffsetFromEBP { get; set; }

        public VirtualStackEntry(string name, UsageTypeInfo usageType)
        {
            Name = name;
            UsageType = usageType;
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() => $"+{OffsetFromEBP}: {Name} ({UsageType.Size} bytes)";
    }
}