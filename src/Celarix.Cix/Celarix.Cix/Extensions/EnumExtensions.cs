using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celarix.Cix.Compiler.Emit.IronArc.Models;

namespace Celarix.Cix.Compiler.Extensions
{
    internal static class EnumExtensions
    {
        public static string GetEmitName(this Register register) => register.ToString().ToLowerInvariant();
    }
}
