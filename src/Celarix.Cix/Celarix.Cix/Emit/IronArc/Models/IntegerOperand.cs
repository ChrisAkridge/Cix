﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    internal sealed class IntegerOperand : InstructionOperand
    {
        public ulong ValueBits { get; set; }

        public IntegerOperand(int valueBits) : this((ulong)valueBits) { }
        
        public IntegerOperand(ulong valueBits) => ValueBits = valueBits;
        public override string GenerateOperandText() => $"{((IsPointer) ? "*" : "")}{ValueBits}";
    }
}