using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celarix.Cix.Compiler.Exceptions;

namespace Celarix.Cix.Compiler.Emit.Models
{
    public sealed class AssemblyInstruction
    {
        public string Mnemonic { get; set; }
        public OperatorSize? OperatorSize { get; set; }
        public string Operand1 { get; set; }
        public string Operand2 { get; set; }
        public string Operand3 { get; set; }

        public static AssemblyInstruction Parse(string instruction)
        {
            var parts = instruction.Split(' ');

            var operatorSize = (parts.Length >= 2)
                ? parts[1].ToLowerInvariant() switch
                {
                    "byte" => Compiler.OperatorSize.Byte,
                    "word" => Compiler.OperatorSize.Word,
                    "dword" => Compiler.OperatorSize.Dword,
                    "qword" => Compiler.OperatorSize.Qword,
                    _ => (OperatorSize?)null
                }
                : null;

            return new AssemblyInstruction
            {
                Mnemonic = parts[0],
                OperatorSize = operatorSize,
                Operand1 = (operatorSize != null)
                    ? (parts.Length >= 3)
                        ? parts[2]
                        : null
                    : parts[1],
                Operand2 = (operatorSize != null)
                    ? (parts.Length >= 4)
                        ? parts[3]
                        : null
                    : parts[2],
                Operand3 = (operatorSize != null)
                    ? (parts.Length >= 5)
                        ? parts[3]
                        : null
                    : parts[3],
            };
        }
    }
}
