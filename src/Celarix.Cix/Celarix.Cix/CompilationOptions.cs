using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celarix.Cix.Compiler
{
    public sealed class CompilationOptions
    {
        private List<string> declaredSymbols;
        
        public string InputFilePath { get; init; }
        public string OutputFilePath { get; init; }
        public bool SaveTemps { get; init; }

        public IEnumerable<string> DeclaredSymbols
        {
            get => declaredSymbols;
            init => declaredSymbols = value.ToList();
        }
    }
}
