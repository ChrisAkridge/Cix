using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celarix.Cix.Compiler.Emit.IronArc
{
    internal static class JumpLabelGenerator
    {
        private static uint lastGeneratedLabelIndex;

        public static string GenerateLabel() => $"block_{++lastGeneratedLabelIndex:X}";
    }
}
