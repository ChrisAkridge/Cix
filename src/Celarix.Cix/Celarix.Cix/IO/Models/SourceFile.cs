using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celarix.Cix.Compiler.IO.Models
{
    internal sealed class SourceFile
    {
        private readonly List<Line> lines;

        public SourceFile(IEnumerable<Line> lines) => this.lines = lines.ToList();

        public IEnumerable<Word> EnumerateWords()
        {
            bool lastCharacterWasWhitespace = false;
            var currentWordStartedAt = 0;
            var currentWordLength = 0;
            
            foreach (var line in lines)
            {
                for (var i = 0; i < line.Text.Length; i++)
                {
                    var current = line.Text[i];

                    if (char.IsWhiteSpace(current))
                    {
                        if (!lastCharacterWasWhitespace)
                        {
                            // We've hit a word boundary. Return the current word.
                            lastCharacterWasWhitespace = true;

                            yield return new Word
                            {
                                FromLine = line,
                                Text = line.Text.Substring(currentWordStartedAt, currentWordLength)
                            };
                        }
                    }
                }
            }
        }
    }
}
