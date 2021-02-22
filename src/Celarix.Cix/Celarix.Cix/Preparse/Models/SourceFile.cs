using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celarix.Cix.Compiler.IO.Models;

namespace Celarix.Cix.Compiler.Preparse.Models
{
    internal sealed class SourceFile
    {
        private readonly List<Line> lines;

        public SourceFile(IEnumerable<Line> lines) =>
            this.lines = lines.ToList();
            
        public IEnumerable<LineWord> EnumerateWords()
        {
            bool lastCharacterWasWhitespace = false;
            var currentWordStartedAt = 0;
            var currentWordLength = 0;
            
            foreach (var line in lines)
            {
                currentWordStartedAt = 0;
                
                for (var i = 0; i < line.Text.Length; i++)
                {
                    var current = line.Text[i];

                    if (char.IsWhiteSpace(current))
                    {
                        lastCharacterWasWhitespace = true;
                        if (!lastCharacterWasWhitespace)
                        {
                            // We've hit a word boundary. Return the current word.

                            var lineWord = new LineWord
                            {
                                FromLine = line,
                                Text = line.Text.Substring(currentWordStartedAt, currentWordLength),
                                OverallCharacterRange = new Range(
                                    new Index(currentWordStartedAt), new Index(currentWordStartedAt + currentWordLength))
                            };

                            if (lineWord.Text != "")
                            {
                                yield return lineWord;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (lastCharacterWasWhitespace)
                        {
                            // Start of a new word.
                            lastCharacterWasWhitespace = false;
                            currentWordStartedAt = i;
                            currentWordLength = 1;
                        }
                        else
                        {
                            currentWordLength += 1;
                        }
                    }
                }
            }
        }
    }
}
