using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celarix.Cix.Compiler.Emit.IronArc.Models;
using Celarix.Cix.Compiler.Exceptions;
using Celarix.Cix.Compiler.Parse.Models.AST.v1;

namespace Celarix.Cix.Compiler.Emit.IronArc
{
    internal static class TypeInfoComputer
    {
        private static readonly List<NamedTypeInfo> primitiveTypes = new List<NamedTypeInfo>
        {
            new NamedTypeInfo { Name = "byte", Size = 1 },
            new NamedTypeInfo { Name = "sbyte", Size = 1 },
            new NamedTypeInfo { Name = "short", Size = 2 },
            new NamedTypeInfo { Name = "ushort", Size = 2 },
            new NamedTypeInfo { Name = "int", Size = 4 },
            new NamedTypeInfo { Name = "uint", Size = 4 },
            new NamedTypeInfo { Name = "long", Size = 8 },
            new NamedTypeInfo { Name = "ulong", Size = 8 },
            new NamedTypeInfo { Name = "float", Size = 4 },
            new NamedTypeInfo { Name = "double", Size = 8 },
            new NamedTypeInfo { Name = "void", Size = 0 }
        };

        public static IDictionary<string, NamedTypeInfo> GenerateTypeInfos(IList<Struct> structs)
        {
            var structInfos = structs
                .Select(s => new StructInfo
                {
                    Name = s.Name,
                    MemberInfos = s.Members
                        .Select(m => new StructMemberInfo
                        {
                            Name = m.Name,
                            ASTType = m.Type,
                            ArraySize = m.StructArraySize,
                            PointerLevel = m.Type.PointerLevel
                        })
                        .ToList()
                })
                .ToList();

            var declaredTypes = structInfos
                .Concat(primitiveTypes)
                .ToDictionary(dt => dt.Name, dt => dt);

            foreach (var structInfo in structInfos)
            {
                SetStructAndMemberSizes(structInfo, declaredTypes, 0);
            }

            return declaredTypes;
        }

        private static void SetStructAndMemberSizes(StructInfo structInfo,
            IDictionary<string, NamedTypeInfo> declaredTypes,
            int recursionDepth)
        {
            if (recursionDepth == 1000)
            {
                throw new ErrorFoundException(ErrorSource.CodeGeneration, -1, "struct member cycle", null, -1);
            }
            else if (structInfo.Size > 0)
            {
                return;
            }

            int memberOffsetCounter = 0;
            foreach (var member in structInfo.MemberInfos)
            {
                Helpers.TypesDeclaredOrThrow(member.ASTType, declaredTypes);

                switch (member.ASTType)
                {
                    case NamedDataType namedType:
                    {
                        member.UnderlyingType = declaredTypes[namedType.Name];

                        if (member.UnderlyingType is StructInfo memberStruct && memberStruct.Size == 0)
                        {
                            SetStructAndMemberSizes(memberStruct, declaredTypes, recursionDepth + 1);
                        }

                        break;
                    }
                    case FuncptrDataType funcptrType:
                        member.UnderlyingType = Helpers.GetDeclaredType(funcptrType, declaredTypes);

                        break;
                }

                member.Offset = memberOffsetCounter;
                memberOffsetCounter += member.Size * member.ArraySize;
            }

            structInfo.Size = structInfo.MemberInfos.Sum(m => m.Size);
        }
    }
}
