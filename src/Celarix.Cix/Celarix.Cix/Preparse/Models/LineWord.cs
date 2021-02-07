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
        public Range OverallCharacterRange { get; init; }
    }
}
