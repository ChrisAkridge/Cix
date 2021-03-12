using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celarix.Cix.Compiler.Exceptions;
using Celarix.Cix.Compiler.Parse.Models.AST.v1;

namespace Celarix.Cix.Compiler.Emit.Models
{
    internal sealed class FunctionCodeGenerator
    {
        private readonly Function function;
        private readonly Stack<VirtualStackEntity> stack = new Stack<VirtualStackEntity>();
        private readonly IDictionary<string, TypeInfo> declaredTypes;
        private readonly IList<GlobalVariableInfo> globals;

        private string currentParentBlockName;

        private readonly Dictionary<string, int> blockNameIndices = new Dictionary<string, int>
        {
            { "block", 0 }
        };

        public FunctionCodeGenerator(Function function,
            IDictionary<string, TypeInfo> declaredTypes,
            IList<GlobalVariableInfo> globals)
        {
            this.function = function;
            this.declaredTypes = declaredTypes;
            this.globals = globals;

            var offsetFromEBP = 0;
            
            foreach (var argument in function.Parameters)
            {
                var argumentType = GetTypeInfoOrThrow(argument.Type);

                stack.Push(new VirtualStackEntity
                {
                    Name = argument.Name, OffsetFromEBP = offsetFromEBP, Size = argumentType.Size
                });

                offsetFromEBP += argumentType.Size;
            }
        }

        // I'm going to regret handling everything as strings.
        // Maybe someday I'll rewrite it to handle instructions.
        // WYLO: no, this is wrong
        // you wrote the functional spec
        // but now you need to write a technical spec.
        public List<string> GenerateCode()
        {
            var rootBlockName = $"{function.Name}:";
            var assembly = new List<string> { rootBlockName };
            currentParentBlockName = rootBlockName;

            foreach (var statement in function.Statements)
            {
                assembly.AddRange(GenerateCodeForStatement(statement));
            }

            return assembly;
        }

        private List<string> GenerateCodeForStatement(Statement statement)
        {
            var assembly = new List<string>();
            
            if (statement is Block block)
            {
                assembly.AddRange(GenerateCodeForBlock(block));
            }
            else if (statement is BreakStatement breakStatement)
            {
                
            }
            
            return assembly;
        }

        private List<string> GenerateCodeForBlock(Block block)
        {
            var index = GetNextBlockIndex("block");
            var blockAssemblyBlockName = $"{function.Name}__block_{index}";
            var blockAssemblyAfterBlockName = $"{function.Name}__block_{index}_after";
            var assembly = new List<string>
            {
                $"jmp {blockAssemblyBlockName}",
                $"{blockAssemblyBlockName}:"
            };
            currentParentBlockName = blockAssemblyBlockName;
            
            foreach (var statement in block.Statements)
            {
                assembly.AddRange(GenerateCodeForStatement(statement));
            }
            
            assembly.Add($"jmp {blockAssemblyAfterBlockName}");
            assembly.Add($"{blockAssemblyAfterBlockName}:");
            currentParentBlockName = blockAssemblyAfterBlockName;

            return assembly;
        }

        private List<string> GenerateCodeForBreakStatement(BreakStatement breakStatement)
        {
            string currentBlockNameWithoutFunction = currentParentBlockName.Substring(function.Name.Length);

            if (!currentBlockNameWithoutFunction.Contains("_case_")
                || !currentBlockNameWithoutFunction.Contains("__dowhile")
                || !currentBlockNameWithoutFunction.Contains("__while"))
            {
                throw new ErrorFoundException(ErrorSource.CodeGeneration, -1, $"Found break statement not in");
            }
        }

        private TypeInfo GetTypeInfoOrThrow(DataType type)
        {
            if (type is FuncptrDataType funcptrDataType)
            {
                var typeInfos = funcptrDataType.Types
                    .Select(t => GetTypeInfoOrThrow(t))
                    .ToList();

                return new FuncptrTypeInfo
                {
                    ReturnType = typeInfos.First(), ParameterTypes = typeInfos.Skip(1).ToList()
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

        private int GetNextBlockIndex(string blockType)
        {
            if (!blockNameIndices.TryGetValue(blockType, out var nextIndex))
            {
                throw new ErrorFoundException(ErrorSource.InternalCompilerError, -1,
                    $"Internal compiler error: Tried to get next index for assembly block type {blockType}.", null, -1);
            }

            blockNameIndices[blockType] += 1;

            return nextIndex;
        }
    }
}
