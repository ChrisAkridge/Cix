using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    internal abstract class TypeInfo : IEquatable<TypeInfo>
    {
        public int Size { get; set; }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.</returns>
        public abstract bool Equals(TypeInfo other);
    }
}