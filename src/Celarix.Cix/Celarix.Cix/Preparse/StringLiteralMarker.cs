using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celarix.Cix.Compiler.IO.Models;
using NLog;

namespace Celarix.Cix.Compiler.Preparse
{
    internal static class StringLiteralMarker
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static void MarkStringLiterals(IList<Line> lines)
        {
            logger.Debug($"Marking string literal locations...");

            foreach (var line in lines)
            {
                line.StringLiteralLocations = GetStringLiteralLocationsOnLine(line.Text).ToList();
            }
            
            logger.Debug("Marked string literal locations");
        }

        private static IEnumerable<Range> GetStringLiteralLocationsOnLine(string text)
        {
            var currentOpenQuoteIndex = -1;

            for (int i = 0; i < text.Length; i++)
            {
                var prev = (i > 0) ? text[i - 1] : (char?)null;
                var current = text[i];

                if (current != '\"') { continue; }
                if (prev == '\\') { continue; }

                if (currentOpenQuoteIndex == -1)
                {
                    currentOpenQuoteIndex = i;
                }
                else
                {
                    yield return new Range(new Index(currentOpenQuoteIndex), i);
                        
                    currentOpenQuoteIndex = -1;
                }
            }
        }
    }
}
