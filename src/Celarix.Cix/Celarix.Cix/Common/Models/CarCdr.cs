using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celarix.Cix.Compiler.Common.Models
{
    /// <summary>
    /// A collection type that holds an item and a reference to another <see cref="CarCdr{T}"/> that
    /// holds the rest of the collection.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    public sealed class CarCdr<T> : IEnumerable<T>
    {
        public T Car { get; set; }
        public CarCdr<T> Cdr { get; set; }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            var current = this;

            while (current != null)
            {
                yield return current.Car;
                
                current = current.Cdr;
            }
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
