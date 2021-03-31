using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    internal sealed class UsageTypeInfo : IEquatable<UsageTypeInfo>
    {
        public TypeInfo DeclaredType { get; set; }
        public int PointerLevel { get; set; }
        public int Size => (PointerLevel > 0) ? 8 : DeclaredType.Size;

        public static UsageTypeInfo FromTypeInfo(TypeInfo type, int pointerLevel) =>
            new UsageTypeInfo
            {
                DeclaredType = type,
                PointerLevel = pointerLevel
            };

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(UsageTypeInfo other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            return ReferenceEquals(this, other)
                || (Equals(DeclaredType, other.DeclaredType)
                    && PointerLevel == other.PointerLevel);
        }

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        /// <see langword="true" /> if the specified object  is equal to the current object; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj) => ReferenceEquals(this, obj) || obj is UsageTypeInfo other && Equals(other);

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => HashCode.Combine(DeclaredType, PointerLevel);
    }
}
