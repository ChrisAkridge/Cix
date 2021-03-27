using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    internal enum OperandSize
    {
        NotUsed,
        Byte,
        Word,
        Dword,
        Qword
    }

    internal enum Register
    {
        EAX,
        EBX,
        ECX,
        EDX,
        EEX,
        EFX,
        EGX,
        EHX,
        EBP,
        ESP,
        ERP,
        EIP,
        EFLAGS
    }

    internal enum FloatSize
    {
        Single,
        Double
    }

    internal enum FlowEdgeType
    {
        DirectFlow,
        UnconditionalJump,
        JumpIfEqual,
        JumpIfNotEqual,
        JumpIfLessThan,
        JumpIfGreaterThan,
        JumpIfLessThanOrEqualTo,
        JumpIfGreaterThanOrEqualTo
    }

    internal enum ImplicitConversionTypeKind
    {
        Struct,
        Integral,
        FloatingPoint
    }

    internal enum IdentifierReferentKind
    {
        LocalVariable,
        FunctionArgument,
        GlobalVariable,
        Function,
        StructMember
    }

    internal enum OperatorKind
    {
        Arithmetic,             // + - * / %
        Bitwise,                // & | ^
        Logical,                // && ||
        Shift,                  // << >>
        Comparison,             // == != < <= > >=
        Assignment,             // =
        ArithmeticAssignment,   // += -= *= /= %=
        BitwiseAssignment,      // &= |= ^=
        ShiftAssignment,        // <<= >>=
        MemberAccess,           // . ->
        Sign,                   // + -
        IncrementDecrement,     // ++ --
        PointerOperation,       // * &
        Conditional             // ?:
    }

    internal enum OperationKind
    {
        PrefixUnary,
        PostfixUnary,
        Binary,
        Ternary
    }
}
