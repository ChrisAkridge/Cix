using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celarix.Cix.Compiler.Common;
using Celarix.Cix.Compiler.IO.Models;
using Celarix.Cix.Compiler.Preparse.Models;

namespace Celarix.Cix.Compiler.Extensions
{
    internal static class EnumerableExtensions
    {
        public static WindowedEnumerator<LineCharacter> EnumerateChars(this IEnumerable<Line> lines)
        {
            var charEnumerator = lines
                .SelectMany(l => l.Text.Select(c => new LineCharacter
                {
                    Character = c,
                    FromLine = l
                }))
                .GetEnumerator();
            
            return new WindowedEnumerator<LineCharacter>(charEnumerator);
        }

        public static IEnumerable<LineWord> EnumerateWords(this IEnumerable<Line> lines)
        {
            var wordBuilder = new StringBuilder();
            var currentWordStartIndex = 0;
            var currentWordEndIndex = 0;

            foreach (var line in lines)
            {
                foreach (var character in line.Text)
                {
                    if (character == ' ' || character == '\t')
                    {
                        if (wordBuilder.Length > 0)
                        {
                            yield return new LineWord
                            {
                                Text = wordBuilder.ToString(),
                                FromLine = line,
                                LineCharacterRange = new Range(new Index(currentWordStartIndex), new Index(currentWordEndIndex))
                            };
                        }

                        wordBuilder.Clear();
                        currentWordStartIndex += 1;
                        currentWordEndIndex = currentWordStartIndex;
                    }
                    else
                    {
                        wordBuilder.Append(character);
                        currentWordEndIndex += 1;
                    }
                }
            }
        }

        public static T SingleOrNone<T>(this IEnumerable<T> source)
        {
            switch (source)
            {
                case null:
                    throw new ArgumentNullException(nameof(source));
                case IList<T> list:
                    return list.Count switch
                    {
                        0 => default,
                        1 => list[0],
                        _ => throw new ArgumentException("Sequence contains multiple elements", nameof(source))
                    };
                default:
                {
                    using var e = source.GetEnumerator();

                    if (!e.MoveNext()) { return default; }
                    var result = e.Current;
                    if (!e.MoveNext()) { return result; }

                    break;
                }
            }

            throw new ArgumentException("Sequence contains multiple elements", nameof(source));
        }
    }
}
