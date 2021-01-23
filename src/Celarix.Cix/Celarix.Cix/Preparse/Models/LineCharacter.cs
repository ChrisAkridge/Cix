using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celarix.Cix.Compiler.IO.Models;

namespace Celarix.Cix.Compiler.Preparse.Models
{
    internal sealed class LineCharacter
    {
        public char Character { get; init; }
        public Line FromLine { get; init; }
    }
}
