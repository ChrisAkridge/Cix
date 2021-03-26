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
            if (type is NamedDataType namedType && !declaredTypes.ContainsKey(namedType.Name))
            {
                throw new ErrorFoundException(ErrorSource.CodeGeneration, -1, $"Type {namedType.Name} not declared", null, -1);
            }

            var funcptrType = (FuncptrDataType)type;

            foreach (var funcptrChildType in funcptrType.Types)
            {
                TypesDeclaredOrThrow(funcptrChildType, declaredTypes);
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
                        funcptrDataType.Types.Select(t => GetDeclaredType(t, declaredTypes)).ToList();

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