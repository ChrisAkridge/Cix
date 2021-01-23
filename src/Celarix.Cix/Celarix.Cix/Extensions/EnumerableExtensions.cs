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
    }
}
