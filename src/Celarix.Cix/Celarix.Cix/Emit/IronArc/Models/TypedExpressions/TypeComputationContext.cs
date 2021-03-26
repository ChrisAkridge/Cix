using System;
using System.Collections.Generic;
using System.Linq;
using Celarix.Cix.Compiler.Parse.Models.AST.v1;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions
{
    internal class TypeComputationContext
    {
        VirtualStack CurrentStack { get; set; }
        IDictionary<string, NamedTypeInfo> DeclaredTypes { get; set; }
        IDictionary<string, GlobalVariableInfo> DeclaredGlobals { get; set; }
        IDictionary<string, Function> FunctionNames { get; set; }
    }
}