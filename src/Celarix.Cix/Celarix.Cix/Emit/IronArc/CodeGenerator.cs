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
        private readonly SourceFileRoot sourceFile;
        private readonly EmitContext emitContext;
        
        public Dictionary<string, StartEndVertices> ControlFlow { get; private set; }
        public string IronArcAssembly { get; private set; }
        
        public CodeGenerator(SourceFileRoot sourceFile)
        {
            this.sourceFile = sourceFile;
            var declaredTypes = TypeInfoComputer.GenerateTypeInfos(sourceFile.Structs);
            var declaredGlobals = GlobalVariableInfoComputer.ComputeGlobalVariableInfos(sourceFile.GlobalVariableDeclarations,
                declaredTypes);
            emitContext = new EmitContext
            {
                CurrentStack = new VirtualStack(),
                DeclaredGlobals = declaredGlobals,
                DeclaredTypes = declaredTypes,
                Functions = sourceFile.Functions.ToDictionary(f => f.Name, f => f)
            };
        }

        public void GenerateCode()
        {
            GenerateFunctions();
        }

        private void GenerateFunctions()
        {
            ControlFlow = new Dictionary<string, StartEndVertices>();
            foreach (var function in sourceFile.Functions)
            {
                emitContext.CurrentFunction = function;
                var emitFunction = new Models.EmitStatements.Function
                {
                    ASTFunction = function
                };
                ControlFlow[function.Name] = emitFunction.Generate(emitContext, null).ControlFlow;
            }
        }
    }
}
