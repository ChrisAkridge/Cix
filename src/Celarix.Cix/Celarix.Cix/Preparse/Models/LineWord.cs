using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celarix.Cix.Compiler.IO.Models;

namespace Celarix.Cix.Compiler.Preparse.Models
{
    internal sealed class LineWord
    {
        public string Text { get; init; }
        public Line FromLine { get; init; }
        public Range LineCharacterRange { get; init; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() => Text;
    }
}
