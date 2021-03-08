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

            // Build a dictionary to quickly lookup vertices without having to do .First() a billion times
            var vertices = structInfos
                .Concat<DependencyVertex>(primitiveInfos)
                .ToDictionary(v => v.Name, v => v);
            
            // Make the graph
            var dependencyGraph = new BidirectionalGraph<DependencyVertex, Edge<DependencyVertex>>(allowParallelEdges: false);
            dependencyGraph.AddVertexRange(vertices.Values);
            
            // Make a root element so that we don't end up with disjoint graphs
            var rootVertex = new StructInfo
            {
                Name = "<>root",
            };
            dependencyGraph.AddVertex(rootVertex);
            dependencyGraph.AddEdgeRange(structInfos.Select(si => new Edge<DependencyVertex>(rootVertex, si)));

            // ReSharper disable once PossibleInvalidCastExceptionInForeachLoop
            foreach (StructInfo @struct in dependencyGraph.Vertices.Where(v => v is StructInfo && v.Name != "<>root" /* ugh */))
            {
                foreach (var member in @struct.Members)
                {
                    // Set size if pointer
                    if (member.Type.PointerLevel > 0 || member.Type is FuncptrDataType)
                    {
                        member.Size = 8;

                        continue;
                    }

                    // Get type vertex, or throw if not present
                    var namedType = (NamedDataType)member.Type;

                    if (!vertices.TryGetValue(namedType.Name, out var typeVertex))
                    {
                        throw new ErrorFoundException(ErrorSource.CodeGeneration, -1,
                            $"Struct member {@struct.Name}.{member.Name} is of undeclared type {namedType.Name}", null,
                            -1);
                    }
                    
                    dependencyGraph.AddEdge(new Edge<DependencyVertex>(@struct, (DependencyVertex)typeVertex));
                }
            }

            // Determine if graph is acyclic and throw if it isn't
            try
            {
                dependencyGraph.SourceFirstTopologicalSort();
            }
            catch (NonAcyclicGraphException nagex)
            {
                throw new ErrorFoundException(ErrorSource.CodeGeneration, -1, $"cycle!", null, -1);

                throw;
            }

            return null;
        }
    }
}
