using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celarix.Cix.Compiler.Common.Models;

namespace Celarix.Cix.Compiler.Common
{
    internal sealed class WindowedEnumerator<T> : IEnumerator<EnumeratorTriplet<T>>, IDisposable
    {
        private readonly IEnumerator<T> enumerator;

        private T previous;
        private T current;
        private T next;

        /// <summary>Gets the element in the collection at the current position of the enumerator.</summary>
        /// <returns>The element in the collection at the current position of the enumerator.</returns>
        public EnumeratorTriplet<T> Current =>
            new EnumeratorTriplet<T> { Previous = previous, Current = current, Next = next };

        /// <summary>Gets the element in the collection at the current position of the enumerator.</summary>
        /// <returns>The element in the collection at the current position of the enumerator.</returns>
        object IEnumerator.Current => Current;

        public WindowedEnumerator(IEnumerator<T> enumerator)
        {
            this.enumerator = enumerator;
            
            Reset();
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose() => enumerator?.Dispose();

        /// <summary>Advances the enumerator to the next element of the collection.</summary>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
        /// <returns>
        /// <see langword="true" /> if the enumerator was successfully advanced to the next element; <see langword="false" /> if the enumerator has passed the end of the collection.</returns>
        public bool MoveNext()
        {
            previous = current;
            current = next;

            if (enumerator.MoveNext())
            {
                next = enumerator.Current;

                return true;
            }
            else
            {
                // https://stackoverflow.com/a/864860/2709212
                if (EqualityComparer<T>.Default.Equals(next, default)) { return false; }
                else
                {
                    next = default;

                    return true;
                }
            }
        }

        /// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
        public void Reset()
        {
            enumerator.Reset();
            
            // Keep enumerator one ahead of what Current returns so we can set Next.
            enumerator.MoveNext();
            current = enumerator.Current;

            if (enumerator.MoveNext()) { next = enumerator.Current; }
        }
    }
}
