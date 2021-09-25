using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Lowering.Models
{
    public sealed class HardwareMethodParameters
    {
        public string ParameterName { get; }
        public HardwareCallDataType Type { get; }

        public HardwareMethodParameters(string parameterName, HardwareCallDataType type)
        {
            ParameterName = parameterName;
            Type = type;
        }
    }
}
