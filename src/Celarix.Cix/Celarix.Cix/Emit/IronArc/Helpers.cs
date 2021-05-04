using System.Collections.Generic;
using System.Linq;
using Celarix.Cix.Compiler.Emit.IronArc.Models;
using Celarix.Cix.Compiler.Exceptions;
using Celarix.Cix.Compiler.Parse.Models.AST.v1;

namespace Celarix.Cix.Compiler.Emit.IronArc
{
    internal static class Helpers
    {
        public static void TypesDeclaredOrThrow(DataType type, IDictionary<string, NamedTypeInfo> declaredTypes)
        {
            switch (type)
            {
                case NamedDataType namedType when !declaredTypes.ContainsKey(namedType.Name):
                    throw new ErrorFoundException(ErrorSource.CodeGeneration, -1, $"Type {namedType.Name} not declared", null, -1);
                case FuncptrDataType funcptrType:
                {
                    foreach (var funcptrChildType in funcptrType.Types)
                    {
                        TypesDeclaredOrThrow(funcptrChildType, declaredTypes);
                    }

                    break;
                }
            }
        }

        public static TypeInfo GetDeclaredType(DataType dataType, IDictionary<string, NamedTypeInfo> declaredTypes)
        {
            switch (dataType)
            {
                case NamedDataType namedType:
                    return declaredTypes[namedType.Name];
                case FuncptrDataType funcptrDataType:
                {
                    var funcptrUnderlyingTypes =
                        funcptrDataType.Types
                            .Select(t => new UsageTypeInfo
                            {
                                DeclaredType = GetDeclaredType(t, declaredTypes),
                                PointerLevel = t.PointerLevel
                            })
                            .ToList();

                    return new FuncptrTypeInfo
                    {
                        ReturnType = funcptrUnderlyingTypes.First(),
                        ParameterTypes = funcptrUnderlyingTypes.Skip(1).ToList(),
                        Size = 8
                    };
                }
                default:
                    throw new ErrorFoundException(ErrorSource.InternalCompilerError, -1,
                        $"DataType was of type {dataType.GetType().Name}", null, -1);
            }
        }
    }
}