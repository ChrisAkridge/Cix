using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celarix.Cix.Compiler.Common.Models
{
    internal sealed class EnumeratorTriplet<T>
    {
        public T Previous { get; init; }
        public T Current { get; init; }
        public T Next { get; init; }
    }
}
