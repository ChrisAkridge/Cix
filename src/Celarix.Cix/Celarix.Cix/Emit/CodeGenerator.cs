using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celarix.Cix.Compiler.Emit.Models;
using Celarix.Cix.Compiler.Exceptions;
using Celarix.Cix.Compiler.Parse.Models.AST.v1;
using Celarix.Cix.Compiler.Parse.Visitor;
using QuickGraph;
using QuickGraph.Algorithms;
using QuickGraph.Algorithms.Search;

namespace Celarix.Cix.Compiler.Emit
{
    public static class CodeGenerator
    {
        private static readonly List<PrimitiveInfo> primitiveInfos = new List<PrimitiveInfo>
        {
            new PrimitiveInfo { Name = "byte", Size = 1 },
            new PrimitiveInfo { Name = "sbyte", Size = 1 },
            new PrimitiveInfo { Name = "short", Size = 2 },
            new PrimitiveInfo { Name = "ushort", Size = 2 },
            new PrimitiveInfo { Name = "int", Size = 4 },
            new PrimitiveInfo { Name = "uint", Size = 4 },
            new PrimitiveInfo { Name = "long", Size = 8 },
            new PrimitiveInfo { Name = "ulong", Size = 8 },
            new PrimitiveInfo { Name = "float", Size = 4 },
            new PrimitiveInfo { Name = "double", Size = 8 },
            new PrimitiveInfo { Name = "void", Size = 0 },
        };

        public static object GenerateCode(SourceFileRoot sourceFile)
        {
            var stringLiterals = GetAllStringLiterals(sourceFile);
            var declaredTypes = GenerateTypeInfo(sourceFile.Structs);
            var globalInfos = GenerateGlobalInfos(sourceFile.GlobalVariableDeclarations, declaredTypes);
            var functionAssemblies = sourceFile.Functions.Select(f =>
                string.Join(Environment.NewLine,
                    new FunctionCodeGenerator(f, declaredTypes, globalInfos).GenerateCode()))
                .ToList();
            
            return null;
        }

        private static List<string> GetAllStringLiterals(SourceFileRoot sourceFile)
        {
            var stringLiteralFinder = new StringLiteralFinder();
            ASTVisitor.VisitSourceFile(stringLiteralFinder, sourceFile);

            return stringLiteralFinder.FoundLiterals;
        }

        private static IDictionary<string, TypeInfo> GenerateTypeInfo(IList<Struct> structs)
        {
            // Convert all structs into StructInfos
            var structInfos = structs
                .Select(s => new StructInfo
                {
                    Name = s.Name,
                    Members = s.Members
                        .Select(m => new StructMemberInfo
                        {
                            Name = m.Name,
                            Type = m.Type
                        })
                        .ToList()
                })
                .ToList();

            var declaredTypes = structInfos
                .Concat<TypeInfo>(primitiveInfos)
                .ToDictionary(dv => dv.Name, dv => dv);
            
            foreach (var structInfo in structInfos)
            {
                SetStructAndMemberSizes(structInfo, declaredTypes, 1);
            }
            
            return declaredTypes;
        }

        private static void SetStructAndMemberSizes(StructInfo structInfo,
            IDictionary<string, TypeInfo> declaredTypes,
            int recursionDepth)
        {
            if (recursionDepth == 1000)
            {
                throw new ErrorFoundException(ErrorSource.CodeGeneration, -1, $"cycle!", null, -1);
            }
            else if (structInfo.Size > 0)
            {
                return;
            }

            foreach (var member in structInfo.Members)
            {
                if (member.Type.PointerLevel > 0 || member.Type is FuncptrDataType)
                {
                    member.Size = 8;
                    continue;
                }

                var memberTypeName = ((NamedDataType)member.Type).Name;

                if (memberTypeName == structInfo.Name)
                {
                    throw new ErrorFoundException(ErrorSource.CodeGeneration, -1, $"cycle!", null, -1);
                }
                else
                {
                    if (!declaredTypes.TryGetValue(memberTypeName, out var declaredType))
                    {
                        throw new ErrorFoundException(ErrorSource.CodeGeneration, -1, $"type not found", null, -1);
                    }
                    else if (declaredType.Name == "void")
                    {
                        throw new ErrorFoundException(ErrorSource.CodeGeneration, -1, $"type can't be void", null,
                            -1);
                    }
                    else if (declaredType.Size == 0)
                    {
                        SetStructAndMemberSizes(structInfo, declaredTypes, recursionDepth + 1);
                    }

                    member.Size = declaredType.Size;
                }
            }

            // Set it here instead of having Size be an auto-property. If Size
            // is an auto-property, partially initializing its members makes
            // the == 0 check fail and allows circular dependencies with the wrong
            // sizes.
            structInfo.Size = structInfo.Members.Sum(m => m.Size);
        }

        private static IList<GlobalVariableInfo> GenerateGlobalInfos(IList<GlobalVariableDeclaration> globals,
            IDictionary<string, TypeInfo> declaredTypes)
        {
            var globalInfos = globals
                .Select(g => new GlobalVariableInfo { Name = g.Name, Type = GetTypeInfoOrThrow(g.Type, declaredTypes) })
                .ToList();

            var globalSizeSum = 0;

            foreach (var global in globalInfos)
            {
                global.OffsetFromHeaderEnd = globalSizeSum;
                globalSizeSum += global.Type.Size;
            }

            return globalInfos;
        }

        private static List<AssemblyBlock> GenerateCodeForFunction(Function function,
            IDictionary<string, TypeInfo> declaredTypes,
            IList<GlobalVariableInfo> globals)
        {
            // Build the stack of arguments.
            var stack = new Stack<VirtualStackEntity>();

            return null;
        }
        
        private static TypeInfo GetTypeInfoOrThrow(DataType type, IDictionary<string, TypeInfo> declaredTypes)
        {
            if (type is FuncptrDataType funcptrDataType)
            {
                var typeInfos = funcptrDataType.Types
                    .Select(t => GetTypeInfoOrThrow(t, declaredTypes))
                    .ToList();

                return new FuncptrTypeInfo
                {
                    ReturnType = typeInfos.First(),
                    ParameterTypes = typeInfos.Skip(1).ToList()
                };
            }

            var namedType = (NamedDataType)type;

            if (!declaredTypes.TryGetValue(namedType.Name, out var namedTypeInfo))
            {
                throw new ErrorFoundException(ErrorSource.CodeGeneration, -1,
                    $"Type {namedType.Name} is not declared.", null, -1);
            }

            return type.PointerLevel == 0
                ? namedTypeInfo
                : new PointerTypeInfo { TypeInfo = namedTypeInfo, PointerLevel = type.PointerLevel };
        }
    }
}
