using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Lowering.Models
{
    public sealed class HardwareMethod
    {
        private readonly List<HardwareMethodParameters> parameters;

        public HardwareCallDataType ReturnType { get; }
        public string CallName { get; }
        public IReadOnlyList<HardwareMethodParameters> Parameters => parameters.AsReadOnly();

        public HardwareMethod(HardwareCallDataType returnType, string callName, IList<HardwareMethodParameters> parameters)
        {
            ReturnType = returnType;
            CallName = callName;
            this.parameters = (List<HardwareMethodParameters>)parameters;
        }
    }
}
