using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celarix.Cix.Compiler.Emit.IronArc.Models;
using Celarix.Cix.Compiler.Emit.IronArc.Models.EmitStatements;
using Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions;
using Celarix.Cix.Compiler.Exceptions;
using Celarix.Cix.Compiler.Parse.Models.AST.v1;
using static Celarix.Cix.Compiler.Emit.IronArc.EmitHelpers;
using Block = Celarix.Cix.Compiler.Parse.Models.AST.v1.Block;
using ConditionalStatement = Celarix.Cix.Compiler.Emit.IronArc.Models.EmitStatements.ConditionalStatement;

namespace Celarix.Cix.Compiler.Emit.IronArc
{
    internal sealed class CodeGenerator
    {
        private const string InternalReturnResult = "<returnResult>";

        private readonly SourceFileRoot sourceFile;
        private readonly IDictionary<string, NamedTypeInfo> declaredTypes;
        private IDictionary<string, GlobalVariableInfo> declaredGlobals;
        private readonly EmitContext emitContext;
        
        public CodeGenerator(SourceFileRoot sourceFile)
        {
            this.sourceFile = sourceFile;
            declaredTypes = TypeInfoComputer.GenerateTypeInfos(sourceFile.Structs);
            declaredGlobals =
                GlobalVariableInfoComputer.ComputeGlobalVariableInfos(sourceFile.GlobalVariableDeclarations,
                    declaredTypes);
            emitContext = new EmitContext
            {
                CurrentStack = new VirtualStack(),
                DeclaredGlobals = declaredGlobals,
                DeclaredTypes = declaredTypes,
                Functions = sourceFile.Functions.ToDictionary(f => f.Name, f => f)
            };
        }

        private StartEndVertices GenerateFunction(Function function)
        {
            foreach (var parameter in function.Parameters)
            {
                emitContext.CurrentStack.Push(new VirtualStackEntry(parameter.Name, emitContext.LookupDataTypeWithPointerLevel(parameter.Type)));
            }
            
            var emitStatementBuilder = new EmitStatementBuilder(emitContext);
            var emitStatements = function.Statements.Select(emitStatementBuilder.Build);


            return null;
        }
    }
}
