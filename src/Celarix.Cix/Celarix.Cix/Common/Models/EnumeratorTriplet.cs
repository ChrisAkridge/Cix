using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celarix.Cix.Compiler.Common.Models
{
    /// <summary>
    /// A window into an enumerable, holding a reference to the current element and the elements before
    /// and after the current element.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    internal sealed class EnumeratorTriplet<T>
    {
        public T Previous { get; init; }
        public T Current { get; init; }
        public T Next { get; init; }
    }
}
