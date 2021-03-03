using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Lowering.Models
{
    public sealed class HardwareCallParameter
    {
        public string ParameterName { get; }
        public HardwareCallDataType Type { get; }

        public HardwareCallParameter(string parameterName, HardwareCallDataType type)
        {
            ParameterName = parameterName;
            Type = type;
        }
    }
}
