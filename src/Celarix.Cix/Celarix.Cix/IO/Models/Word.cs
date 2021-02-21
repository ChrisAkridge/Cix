using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celarix.Cix.Compiler.IO.Models
{
    internal sealed class Word
    {
        public string Text { get; init; }
        public Line FromLine { get; init; }
        public int StartingCharacterIndex { get; init; }
        public int EndingCharacterIndex => StartingCharacterIndex + Text.Length;

        public void Replace(string newWord) => FromLine.ReplaceWord(StartingCharacterIndex, newWord);
    }
}
