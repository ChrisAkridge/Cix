using System;
using System.Collections.Generic;
using System.Linq;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public sealed class FuncptrDataType : DataType
    {
        public List<DataType> Types { get; set; }

        public override string PrettyPrint(int indentLevel)
        {
            var prettyTypes = string.Join(", ", Types.Select(t => t.PrettyPrint(0)));

            return $"@funcptr<{prettyTypes}>{new string('*', PointerLevel)}";
        }
    }
}
