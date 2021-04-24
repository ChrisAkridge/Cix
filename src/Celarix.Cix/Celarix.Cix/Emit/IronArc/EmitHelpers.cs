using System.Collections.Generic;
using System.Linq;
using Celarix.Cix.Compiler.Emit.IronArc.Models;
using Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions;
using Celarix.Cix.Compiler.Exceptions;

namespace Celarix.Cix.Compiler.Emit.IronArc
{
    internal sealed class EmitHelpers
    {
        public static InstructionVertex ResetStack() =>
            new InstructionVertex("mov", OperandSize.Qword, Register(Models.Register.EBP),
                Register(Models.Register.ESP));

        public static InstructionVertex ZeroEAX() =>
            new InstructionVertex("mov", OperandSize.Qword, new IntegerOperand(0), Register(Models.Register.EAX));

        public static RegisterOperand Register(Register register, bool isPointer = false, int offset = 0) =>
            new RegisterOperand
            {
                Register = register, IsPointer = isPointer, Offset = offset
            };

        public static StartEndVertices ConnectWithDirectFlow(IEnumerable<IConnectable> vertices)
        {
            IConnectable start = null;
            IConnectable last = null;
            IConnectable end = null;

            foreach (var current in vertices.Where(v => v != null))
            {
                start ??= current;
                end = current;

                last?.ConnectTo(current, FlowEdgeType.DirectFlow);

                last = current;
            }

            return new StartEndVertices
            {
                Start = start.ConnectionTarget, End = end.ConnectionTarget
            };
        }

        public static StartEndVertices ChangeWidthOfTopOfStack(OperandSize oldSize, OperandSize newSize) =>
            ConnectWithDirectFlow(new List<ControlFlowVertex>
            {
                new InstructionVertex("mov", OperandSize.Qword, new IntegerOperand(0), Register(Models.Register.ECX)),
                new InstructionVertex("pop", oldSize, Register(Models.Register.ECX)),
                new InstructionVertex("push", newSize, Register(Models.Register.ECX))
            });

        public static OperandSize ToOperandSize(int size)
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

        public static bool IsIronArcOperandSize(int size) => size == 1 || (size == 2) | (size == 4) || size == 8;

        public static bool IsFloatingPointType(TypeInfo typeInfo) =>
            (typeInfo is NamedTypeInfo namedType)
            && ((namedType.Name == "float")
                || (namedType.Name == "double"));

        public static bool ExpressionRequiresPointer(TypedExpression expression)
        {
            switch (expression)
            {
                case UnaryExpression unaryExpression
                    when (unaryExpression.Operator == "++" || unaryExpression.Operator == "--" || unaryExpression.Operator == "&"):
                case BinaryExpression binaryExpression when (binaryExpression.Operator == "="
                    || binaryExpression.Operator == "+="
                    || binaryExpression.Operator == "-="
                    || binaryExpression.Operator == "*="
                    || binaryExpression.Operator == "/="
                    || binaryExpression.Operator == "%="
                    || binaryExpression.Operator == "&="
                    || binaryExpression.Operator == "|="
                    || binaryExpression.Operator == "^="
                    || binaryExpression.Operator == "<<="
                    || binaryExpression.Operator == ">>="
                    || binaryExpression.Operator == "."
                    || binaryExpression.Operator == "->"):
                    return true;
                default:
                    return false;
            }
        }
    }
}
