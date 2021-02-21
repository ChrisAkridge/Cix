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

            // We need a better enumeration system.
            // It needs to represent an entire preprocessed source file and be
            // able to enumerate both lines and words, complete with line- and
            // file-level information. It also needs to support modifications - 
            // not adding or removing items from the collection, but modifying
            // the underlying text of the line. The only transformation we need
            // for the type rewriter is one where the content of the line changes,
            // but the length does not.
            //
            // This can be done by adding an optional data structure atop each line,
            // one that is a BitArray of character indices from 0 to Length. A set
            // bit indicates that this character will be transformed.
            //
            // We'd prefer to keep the entire structure for the file, rather than
            // one per line, because the mark and replace phases are different.
            // Or are they?
            //
            // Let's map this as a state graph.
            // States: { Default, ReplaceAsterisksInNextWord }
            // Oh. That's simple. We know what our types are, so all we need to do
            // is enumerate words and then change to ReplaceAsterisksInNextWord
            // when we come across a type. Well, somewhat. Here's the full algorithm.
            //
            // - A source file is a list of lines
            // - A line is what it is already defined as
            // - A source file can be enumerated in words
            // - A word is its text plus the line it's defined on, as well as its start position on the line
            // 1. For each word in the source file,
            //  a. Look at the current word. If it's:
            //      i. A type name: Switch to Default state. Does it end in asterisks?
            //         Replace the word with a word ending in backticks. Otherwise,
            //         switch to the ReplaceAsterisksInNextWord state.
            //      ii. A word starting with asterisks, or is entirely asterisks?
            //          Replace the asterisks with backticks and set the state to
            //          Default.
        }
    }
}
