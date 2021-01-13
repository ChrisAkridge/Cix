using System;
using System.Collections.Generic;
using System.Text;

namespace Cix.Models
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
