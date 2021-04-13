using System;
using System.Collections.Generic;
using System.Linq;
using Celarix.Cix.Compiler.Parse.Models.AST.v1;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    internal class EmitContext
    {
        public VirtualStack CurrentStack { get; set; }
        public Function CurrentFunction { get; set; }
        public IDictionary<string, NamedTypeInfo> DeclaredTypes { get; set; }
        public IDictionary<string, GlobalVariableInfo> DeclaredGlobals { get; set; }
        public IDictionary<string, Function> Functions { get; set; }
        public Stack<ControlFlowVertex> BreakTargets { get; set; } = new Stack<ControlFlowVertex>();
        public Stack<ControlFlowVertex> ContinueTargets { get; set; } = new Stack<ControlFlowVertex>();

        public TypeInfo LookupDataType(DataType dataType)
        {
            return dataType switch
            {
                NamedDataType namedType => !DeclaredTypes.TryGetValue(namedType.Name, out var namedTypeInfo)
                    ? throw new InvalidOperationException("No type with this name exists")
                    : namedTypeInfo,
                FuncptrDataType funcptrType => new FuncptrTypeInfo
                {
                    ReturnType = LookupDataType(funcptrType.Types.First()),
                    ParameterTypes = funcptrType.Types.Skip(1).Select(LookupDataType).ToList()
                },
                _ => throw new InvalidOperationException("Internal compiler error: Unrecognized type")
            };
        }

        public UsageTypeInfo LookupDataTypeWithPointerLevel(DataType dataType) =>
            new UsageTypeInfo
            {
                DeclaredType = LookupDataType(dataType), PointerLevel = dataType.PointerLevel
            };
    }
}