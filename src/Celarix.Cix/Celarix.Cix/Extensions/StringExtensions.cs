using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celarix.Cix.Compiler.Extensions
{
    internal static class StringExtensions
    {
        public static bool IsIdentifier(this string s)
        {
            return !string.IsNullOrWhiteSpace(s)
                && (char.IsLetter(s[0]) || s[0] == '_')
                && (s.Length == 1 || s.Substring(1).All(c => char.IsLetterOrDigit(c) || c == '_'));
        }
    }
}
