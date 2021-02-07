using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celarix.Cix.Compiler.Extensions;
using Celarix.Cix.Compiler.IO.Models;
using NLog;

namespace Celarix.Cix.Compiler.Preparse
{
    internal static class TypeRewriter
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private static IList<string> GetDefinedTypes(IList<Line> lines)
        {
            var definedTypes = new List<string>
            {
                "byte",
                "sbyte",
                "short",
                "ushort",
                "int",
                "uint",
                "long",
                "ulong",
                "float",
                "double",
                "void"
            };
            string previousWord = null;

            foreach (var word in lines.EnumerateWords())
            {
                var wordLine = word.FromLine;

                if (wordLine.StringLiteralLocations.Any(sll => word.OverallCharacterRange.Start.Value >= sll.Start.Value
                    && word.OverallCharacterRange.End.Value <= sll.End.Value))
                {
                    previousWord = null;
                    continue;
                }

                if (previousWord == "struct")
                {
                    definedTypes.Add(word.Text);
                }

                previousWord = word.Text;
            }

            return definedTypes;
        }

        private static IList<Line> RewritePointerTypes(IList<Line> lines, IList<string> definedTypes)
        {
            // WYLO: ugh.
            // We need to rewrite:
            //  - type***
            //  - type ***
            //  - type
            //  ***
            //
            // somehow operating at word- and line-level at the same time
        }
    }
}
