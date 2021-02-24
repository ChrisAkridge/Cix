using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Celarix.Cix.Compiler.IO.Models;

namespace Celarix.Cix.Compiler.Preparse.Models
{
    internal sealed class SourceFile
    {
        private readonly List<Line> lines;

        public SourceFile(IEnumerable<Line> lines) =>
            this.lines = lines.ToList();

        public string JoinLines() => string.Join(Environment.NewLine, lines.Select(l => l.Text));
        
        public IEnumerable<LineWord> EnumerateWords()
        {
            foreach (var line in lines)
            {
                // States: { InsideWord, OutsideWord }
                // Transitions:
                //  - Start of line => OutsideWord
                //  - Non-WS character in OutsideWord => InsideWord
                //  - Non-WS character in InsideWord => InsideWord
                //  - WS character in OutsideWord => OutsideWord
                //  - WS character in InsideWord => OutsideWord, yield current word
                //  - End of line => yield current word, if any
                var insideWord = false;
                var currentWordStartsAt = 0;
                var currentWordLength = 0;

                for (int i = 0; i < line.Text.Length; i++)
                {
                    var current = line.Text[i];

                    if (i == line.Text.Length - 1)
                    {
                        if (insideWord)
                        {
                            yield return new LineWord
                            {
                                FromLine = line,
                                Text = line.Text.Substring(currentWordStartsAt, currentWordLength + 1),
                                LineCharacterRange = new Range(new Index(currentWordStartsAt),
                                    new Index(currentWordStartsAt + currentWordLength + 1))
                            };
                        }
                        else if (!char.IsWhiteSpace(current))
                        {
                            yield return new LineWord
                            {
                                FromLine = line,
                                Text = line.Text.Substring(i),
                                LineCharacterRange = new Range(new Index(i), new Index(i))
                            };
                        }
                    }
                    else if (!char.IsWhiteSpace(current))
                    {
                        if (insideWord)
                        {
                            currentWordLength++;
                        }
                        else
                        {
                            insideWord = true;
                            currentWordStartsAt = i;
                            currentWordLength = 1;
                        }
                    }
                    else
                    {
                        if (insideWord)
                        {
                            insideWord = false;

                            yield return new LineWord
                            {
                                FromLine = line,
                                Text = line.Text.Substring(currentWordStartsAt, currentWordLength),
                                LineCharacterRange = new Range(new Index(currentWordStartsAt),
                                    new Index(currentWordStartsAt + currentWordLength))
                            };

                            currentWordStartsAt = 0;
                            currentWordLength = 0;
                        }
                    }
                }
            }
        }
    }
}
