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

        // https://stackoverflow.com/a/14087738/2709212
        public static string ToLiteral(this string input)
        {
            var literal = new StringBuilder(input.Length + 2);
            literal.Append('"');

            foreach (var c in input)
            {
                switch (c)
                {
                    case '\'':
                        literal.Append(@"\'");

                        break;
                    case '\"':
                        literal.Append("\\\"");

                        break;
                    case '\\':
                        literal.Append(@"\\");

                        break;
                    case '\0':
                        literal.Append(@"\0");

                        break;
                    case '\a':
                        literal.Append(@"\a");

                        break;
                    case '\b':
                        literal.Append(@"\b");

                        break;
                    case '\f':
                        literal.Append(@"\f");

                        break;
                    case '\n':
                        literal.Append(@"\n");

                        break;
                    case '\r':
                        literal.Append(@"\r");

                        break;
                    case '\t':
                        literal.Append(@"\t");

                        break;
                    case '\v':
                        literal.Append(@"\v");

                        break;
                    default:
                        // ASCII printable character
                        if (c >= 0x20 && c <= 0x7e)
                        {
                            literal.Append(c);
                            // As UTF16 escaped character
                        }
                        else
                        {
                            literal.Append(@"\u");
                            literal.Append(((int)c).ToString("x4"));
                        }

                        break;
                }
            }

            literal.Append('"');

            return literal.ToString();
        }
    }
}
