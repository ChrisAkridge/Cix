using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celarix.Cix.Compiler.Parse.Models.AST.v1
{
    public sealed class InternalEndStatement : Statement
    {
        public override string PrettyPrint(int indentLevel) => "/* end of program */";
    }
}
