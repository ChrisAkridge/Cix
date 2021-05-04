using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    internal sealed class FuncptrTypeInfo : TypeInfo
    {
        public UsageTypeInfo ReturnType { get; set; }
        public List<UsageTypeInfo> ParameterTypes { get; set; }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.</returns>
        public override bool Equals(TypeInfo other) =>
            other is FuncptrTypeInfo funcptrOther
            && (ReturnType.Equals(funcptrOther.ReturnType)
                && ParameterTypes.Zip(funcptrOther.ParameterTypes).All(types => types.First.Equals(types.Second)));

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            var parameterNames = (ParameterTypes.Any()) ? string.Join(",", ParameterTypes.Select(pt => pt.ToString())) : "";

            return $"@funcptr<{ReturnType}, {parameterNames}>";
        }
    }
}