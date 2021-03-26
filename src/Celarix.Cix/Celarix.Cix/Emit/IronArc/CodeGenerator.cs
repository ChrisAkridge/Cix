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
    internal sealed class CodeGenerator
    {
        private const string InternalReturnResult = "<returnResult>";

        private readonly SourceFileRoot sourceFile;
        private readonly IDictionary<string, NamedTypeInfo> declaredTypes;
        private IDictionary<string, GlobalVariableInfo> declaredGlobals;
        private FunctionEmitContext currentFunctionContext;
        
        public CodeGenerator(SourceFileRoot sourceFile)
        {
            this.sourceFile = sourceFile;
            declaredTypes = TypeInfoComputer.GenerateTypeInfos(sourceFile.Structs);
            declaredGlobals =
                GlobalVariableInfoComputer.ComputeGlobalVariableInfos(sourceFile.GlobalVariableDeclarations,
                    declaredTypes);
        }

        private object GenerateFunction(Function function)
        {
            currentFunctionContext = new FunctionEmitContext
            {
                Function = function
            };

            var argumentOffsetCounter = 0;
            foreach (var argument in function.Parameters)
            {
                var argumentEntry = new VirtualStackEntry
                {
                    Name = argument.Name,
                    UsageType = UsageTypeInfo.FromTypeInfo(Helpers.GetDeclaredType(argument.Type, declaredTypes), argument.Type.PointerLevel),
                    OffsetFromEBP = argumentOffsetCounter
                };
                currentFunctionContext.Stack.Entries.Push(argumentEntry);

                argumentOffsetCounter += argumentEntry.UsageType.Size;
            }

            return null;
        }

        private StartEndVertices GenerateStatement(Statement statement)
        {
            if (!IsLinearFlowElement(statement))
            {
                var ungeneratedVertex = new UngeneratedVertex { NodeToGenerateFor = statement };

                return new StartEndVertices { Start = ungeneratedVertex, End = ungeneratedVertex };
            }
            else
            {
                return GenerateLinearFlowElement(statement);
            }
        }

        private StartEndVertices GenerateLinearFlowElement(Statement statement)
        {
            switch (statement)
            {
                case BreakStatement breakStatement:
                    return GenerateBreakStatement();
                case ContinueStatement continueStatement:
                    return GenerateContinueStatement();
                case ExpressionStatement expressionStatement:
                    return GenerateExpressionStatement(expressionStatement);
                default:
                    throw new InvalidOperationException();
            }
        }

        private StartEndVertices GenerateBreakStatement() => GenerateBreakContinueStatement(currentFunctionContext.BreakTargets);

        private StartEndVertices GenerateContinueStatement() => GenerateBreakContinueStatement(currentFunctionContext.ContinueTargets);

        private static StartEndVertices GenerateBreakContinueStatement(Stack<ControlFlowVertex> targetStack)
        {
            if (!targetStack.TryPop(out var target))
            {
                throw new ErrorFoundException(ErrorSource.CodeGeneration, -1, "no continue target", null, -1);
            }

            target.IsJumpTarget = true;

            var jump = new InstructionVertex("jmp", OperandSize.NotUsed, new JumpTargetOperand { Target = target });

            jump.OutboundEdge = new FlowEdge
            {
                Source = jump, Destination = target, FlowEdgeType = FlowEdgeType.UnconditionalJump
            };

            return new StartEndVertices { Start = jump, End = jump };
        }

        private static StartEndVertices GenerateExpressionStatement(ExpressionStatement expressionStatement)
        {
            // code to compute expression
            var removeResult = new InstructionVertex("subl", OperandSize.Qword,
                Register(Models.Register.ESP), /* size of expression */ new IntegerOperand(0),
                Register(Models.Register.ESP));

            return new StartEndVertices { Start = /* expression start */ null, End = removeResult };
        }

        private StartEndVertices GenerateReturnStatement(ReturnStatement returnStatement) =>
            (returnStatement.ReturnValue == null)
                ? GenerateVoidReturnStatement()
                : GenerateNonVoidReturnStatement(returnStatement.ReturnValue);

        private StartEndVertices GenerateVoidReturnStatement()
        {
            var returnInstruction = new InstructionVertex("ret");

            return new StartEndVertices { Start = returnInstruction, End = returnInstruction };
        }

        private StartEndVertices GenerateNonVoidReturnStatement(Expression returnValue)
        {
            // generate instructions for expression
            var expressionInstructions = (StartEndVertices)null;
            var returnTypeInfo = (TypeInfo)null;

            if (returnTypeInfo is StructInfo structInfo)
            {
                var structStackEntry = currentFunctionContext.Stack.GetEntry(InternalReturnResult);

                var postProcessing = ConnectWithDirectFlow(new[]
                {
                    new InstructionVertex("movln",
                        OperandSize.NotUsed,
                        Register(Models.Register.EBP,
                            isPointer: true),
                        Register(Models.Register.EBP,
                            isPointer: true,
                            offset: structStackEntry.OffsetFromEBP),
                        new IntegerOperand((ulong)returnTypeInfo.Size)),
                    ResetStack()
                });
                
                expressionInstructions.End.ConnectTo(postProcessing.Start, FlowEdgeType.DirectFlow);
                return new StartEndVertices { Start = expressionInstructions.Start, End = postProcessing.End };
            }
            else
            {
                var postProcessing = ConnectWithDirectFlow(new[]
                {
                    ZeroEAX(),
                    new InstructionVertex("pop", ToOperandSize(returnTypeInfo.Size), Register(Models.Register.EAX)),
                    ResetStack(),
                    new InstructionVertex("push", ToOperandSize(returnTypeInfo.Size), Register(Models.Register.EAX)),
                    new InstructionVertex("ret")
                });
                
                expressionInstructions.End.ConnectTo(postProcessing.Start, FlowEdgeType.DirectFlow);

                return new StartEndVertices { Start = expressionInstructions.Start, End = postProcessing.End };
            }
        }

        private static InstructionVertex ResetStack() =>
            new InstructionVertex("mov", OperandSize.Qword, Register(Models.Register.EBP),
                Register(Models.Register.ESP));

        private static InstructionVertex ZeroEAX() =>
            new InstructionVertex("mov", OperandSize.Qword, new IntegerOperand(0), Register(Models.Register.EAX));

        private static RegisterOperand Register(Register register, bool isPointer = false, int offset = 0) =>
            new RegisterOperand { Register = register, IsPointer = isPointer, Offset = offset };

        private static StartEndVertices ConnectWithDirectFlow(IEnumerable<ControlFlowVertex> vertices)
        {
            ControlFlowVertex start = null;
            ControlFlowVertex last = null;
            ControlFlowVertex end = null;

            foreach (var current in vertices)
            {
                start ??= current;
                end = current;

                last?.ConnectTo(current, FlowEdgeType.DirectFlow);

                last = current;
            }

            return new StartEndVertices { Start = start, End = end };
        }

        private static bool IsLinearFlowElement(Statement statement) =>
            !((statement is Block)
                || (statement is ConditionalStatement)
                || (statement is DoWhileStatement)
                || (statement is SwitchStatement)
                || (statement is CaseStatement)
                || (statement is WhileStatement));

        private static OperandSize ToOperandSize(int size)
        {
            return size switch
            {
                1 => OperandSize.Byte,
                2 => OperandSize.Word,
                4 => OperandSize.Dword,
                8 => OperandSize.Qword,
                _ => throw new ErrorFoundException(ErrorSource.InternalCompilerError, -1, $"Size {size} isn't an IronArc operand size", null, -1)
            };
        }
        
        // Expressions
        
    }
}
