using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    internal sealed class FloatingPointOperand : InstructionOperand
    {
        public FloatSize FloatSize { get; set; }
        public ulong ValueBits { get; set; }
        
        public static FloatingPointOperand FromSingle(float value) =>
            new FloatingPointOperand
            {
                FloatSize = FloatSize.Single,
                ValueBits = (ulong)BitConverter.SingleToInt32Bits(value)
            };

        public static FloatingPointOperand FromDouble(double value) =>
            new FloatingPointOperand
            {
                FloatSize = FloatSize.Double,
                ValueBits = (ulong)BitConverter.DoubleToInt64Bits(value)
            };
    }
}