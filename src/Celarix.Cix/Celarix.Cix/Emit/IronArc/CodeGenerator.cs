﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celarix.Cix.Compiler.Emit.IronArc.Models;
using Celarix.Cix.Compiler.Emit.IronArc.Models.EmitStatements;
using Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions;
using Celarix.Cix.Compiler.Exceptions;
using Celarix.Cix.Compiler.Parse.Models.AST.v1;
using NLog;
using static Celarix.Cix.Compiler.Emit.IronArc.EmitHelpers;
using Block = Celarix.Cix.Compiler.Parse.Models.AST.v1.Block;
using ConditionalStatement = Celarix.Cix.Compiler.Emit.IronArc.Models.EmitStatements.ConditionalStatement;
using Register = Celarix.Cix.Compiler.Emit.IronArc.Models.Register;

namespace Celarix.Cix.Compiler.Emit.IronArc
{
    internal sealed class CodeGenerator
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        
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
            logger.Debug("Generating IronArc assembly...");

            try
            {
                logger.Trace("Adding call to __globals_init() for main()");

                var mainFunction = sourceFile.Functions
                    .Single(f =>
                        f.Name == "main"
                        && f.ReturnType is NamedDataType namedType
                        && namedType.Name == "void"
                        && namedType.PointerLevel == 0);

                mainFunction.Statements.Insert(0, new Parse.Models.AST.v1.ExpressionStatement
                {
                    Expression = new Parse.Models.AST.v1.FunctionInvocation
                    {
                        Operand = new Parse.Models.AST.v1.Identifier
                        {
                            IdentifierText = "__globals_init"
                        },
                        Arguments = new List<Expression>()
                    }
                });
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Source file must declare void main()", ex);
            }

            GenerateFunctions();
            SetStackAfterCode();
            GenerateFinalAssembly();

            logger.Debug("IronArc assembly generated");
        }

        private void GenerateFunctions()
        {
            ControlFlow = new Dictionary<string, StartEndVertices>();
            foreach (var function in sourceFile.Functions)
            {
                logger.Trace($"Generating code for function {function.Name}");
                emitContext.CurrentFunction = function;
                var emitFunction = new Models.EmitStatements.Function
                {
                    ASTFunction = function
                };
                ControlFlow[function.Name] = emitFunction.Generate(emitContext, null).ControlFlow;
            }
        }

        private void SetStackAfterCode()
        {
            var mainFlow = ControlFlow["main"];
            var setEBPAfterCode = new InstructionVertex("mov", OperandSize.Qword, Register(Register.EAX), Register(Register.EBP));
            var setESPAfterCode = new InstructionVertex("mov", OperandSize.Qword, Register(Register.EAX), Register(Register.ESP));
            
            setEBPAfterCode.ConnectTo(setESPAfterCode.ConnectionTarget, FlowEdgeType.DirectFlow);
            setESPAfterCode.ConnectTo(mainFlow.ConnectionTarget, FlowEdgeType.DirectFlow);
            ControlFlow["main"] = new StartEndVertices(setEBPAfterCode, mainFlow.End);
        }

        private void GenerateFinalAssembly()
        {
            var finalEmitters = ControlFlow.Select(kvp => new FinalEmitter(kvp.Key, kvp.Value));
            var functionAssemblyCodeBlocks = finalEmitters.Select(e => e.GenerateInstructionsForControlFlow());
            var builder = new StringBuilder();

            builder.AppendLine($"globals: {emitContext.DeclaredGlobals.Values.Sum(g => g.UsageType.Size)}");
            builder.AppendLine("jmp main");

            foreach (var codeBlock in functionAssemblyCodeBlocks) { builder.AppendLine(codeBlock); }

            IronArcAssembly = builder.ToString();
        }
    }
}
