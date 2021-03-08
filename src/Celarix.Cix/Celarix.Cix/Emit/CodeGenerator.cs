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
        public static object GenerateCode(SourceFileRoot sourceFile)
        {
            var stringLiterals = GetAllStringLiterals(sourceFile);
            var structInfos = GenerateStructInfos(sourceFile.Structs);
        
            return null;
        }

        private static List<string> GetAllStringLiterals(SourceFileRoot sourceFile)
        {
            var stringLiteralFinder = new StringLiteralFinder();
            ASTVisitor.VisitSourceFile(stringLiteralFinder, sourceFile);

            return stringLiteralFinder.FoundLiterals;
        }

        private static IList<StructInfo> GenerateStructInfos(IList<Struct> structs)
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

            // Build the list of primitives
            var primitiveInfos = new List<PrimitiveInfo>
            {
                new PrimitiveInfo
                {
                    Name = "byte",
                    Size = 1
                },
                new PrimitiveInfo
                {
                    Name = "sbyte",
                    Size = 1
                },
                new PrimitiveInfo
                {
                    Name = "short",
                    Size = 2
                },
                new PrimitiveInfo
                {
                    Name = "ushort",
                    Size = 2
                },
                new PrimitiveInfo
                {
                    Name = "int",
                    Size = 4
                },
                new PrimitiveInfo
                {
                    Name = "uint",
                    Size = 4
                },
                new PrimitiveInfo
                {
                    Name = "long",
                    Size = 8
                },
                new PrimitiveInfo
                {
                    Name = "ulong",
                    Size = 8
                },
                new PrimitiveInfo
                {
                    Name = "float",
                    Size = 4
                },
                new PrimitiveInfo
                {
                    Name = "double",
                    Size = 8
                },
                new PrimitiveInfo
                {
                    Name = "void",
                    Size = 0
                },
            };

            var declaredTypes = structInfos
                .Concat<DependencyVertex>(primitiveInfos)
                .ToDictionary(dv => dv.Name, dv => dv);
            
            foreach (var structInfo in structInfos)
            {
                SetStructAndMemberSizes(structInfo, declaredTypes, 1);
            }
            
            return null;
        }

        private static void SetStructAndMemberSizes(StructInfo structInfo,
            IDictionary<string, DependencyVertex> declaredTypes,
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
    }
}
