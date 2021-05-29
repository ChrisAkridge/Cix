using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celarix.Cix.Compiler.Emit.IronArc.Models;

namespace Celarix.Cix.Compiler.Emit.IronArc
{
    internal static class ExpressionHelpers
    {
        public static UsageTypeInfo GetCommonType(UsageTypeInfo left, UsageTypeInfo right)
        {
            var leftNamedType = left.DeclaredType as NamedTypeInfo;
            var rightNamedType = right.DeclaredType as NamedTypeInfo;

            if (left.Equals(right)) { return left; }
            else if (left.PointerLevel > 0 && right.PointerLevel > 0)
            {
                return leftNamedType?.Name == "void"
                    ? right
                    : rightNamedType?.Name == "void"
                        ? left
                        : throw new InvalidOperationException("No conversion exists between types");
            }
            else if (leftNamedType != null && rightNamedType != null)
            {
                var leftKind = GetTypeKind(leftNamedType.Name);
                var rightKind = GetTypeKind(rightNamedType.Name);

                if (leftNamedType.Name == rightNamedType.Name) { return left; }

                var (leftMin, leftMax) = GetNumericTypeLog2Ranges(leftNamedType.Name);
                var (rightMin, rightMax) = GetNumericTypeLog2Ranges(rightNamedType.Name);
                bool leftInsideRight = leftMin >= rightMin && leftMax <= rightMax;
                var rightInsideLeft = rightMin >= leftMin && rightMax <= rightMin;

                switch (leftKind)
                {
                    case ImplicitConversionTypeKind.Integral when rightKind == ImplicitConversionTypeKind.Integral:
                    {
                        if (leftInsideRight) { return right; }
                        else if (rightInsideLeft) { return left; }

                        break;
                    }
                    case ImplicitConversionTypeKind.Integral when rightKind == ImplicitConversionTypeKind.FloatingPoint:
                        return leftInsideRight
                            ? right
                            : throw new InvalidOperationException(
                                "Values of type {left} cannot be implicitly converted to values of type {right} without loss of precision");
                    case ImplicitConversionTypeKind.FloatingPoint when rightKind == ImplicitConversionTypeKind.Integral:
                        return rightInsideLeft
                            ? left
                            : throw new InvalidOperationException(
                                "Values of type {right} cannot be implicitly converted to values of type {left} without loss of precision");
                    case ImplicitConversionTypeKind.FloatingPoint when rightKind == ImplicitConversionTypeKind.FloatingPoint:
                        return leftNamedType.Name == "double" ? left : right;
                    case ImplicitConversionTypeKind.Struct:
                        throw new InvalidOperationException("No conversion exists between structs and any other type");
                    default:
                        throw new InvalidOperationException("Internal compiler error: type {left} was not marked as integral, floating point, or struct");
                }
            }
            else
            {
                throw new InvalidOperationException("No implicit conversion exists");
            }
            
            throw new InvalidOperationException("Unreachable");
        }

        public static bool IsNumeric(UsageTypeInfo type) =>
            (type.DeclaredType is NamedTypeInfo namedType)
            && (type.PointerLevel == 0)
            && (namedType.Name == "byte"
                || namedType.Name == "sbyte"
                || namedType.Name == "short"
                || namedType.Name == "ushort"
                || namedType.Name == "int"
                || namedType.Name == "uint"
                || namedType.Name == "long"
                || namedType.Name == "ulong"
                || namedType.Name == "float"
                || namedType.Name == "double");

        public static bool IsStruct(UsageTypeInfo type, IDictionary<string, NamedTypeInfo> declaredTypes) =>
            (type.DeclaredType is NamedTypeInfo namedType
                && declaredTypes.TryGetValue(namedType.Name, out var declaredType)
                && declaredType is StructInfo);

        public static bool ImplicitlyConvertibleToInt(UsageTypeInfo type) =>
            (type.DeclaredType is NamedTypeInfo namedType)
            && (type.PointerLevel == 0)
            && (namedType.Name == "byte"
                || namedType.Name == "sbyte"
                || namedType.Name == "short"
                || namedType.Name == "ushort"
                || namedType.Name == "int");

        public static OperatorKind GetOperatorKind(string operatorSymbol, OperationKind operationKind)
        {
            return operatorSymbol switch
            {
                "+" when operationKind == OperationKind.PrefixUnary => OperatorKind.Sign,
                "-" when operationKind == OperationKind.PrefixUnary => OperatorKind.Sign,
                "+" => OperatorKind.Arithmetic,
                "-" => OperatorKind.Arithmetic,
                "*" when operationKind == OperationKind.Binary => OperatorKind.Arithmetic,
                "/" => OperatorKind.Arithmetic,
                "%" => OperatorKind.Arithmetic,
                "&" when operationKind == OperationKind.Binary => OperatorKind.Bitwise,
                "|" => OperatorKind.Bitwise,
                "^" => OperatorKind.Bitwise,
                "&&" => OperatorKind.Logical,
                "||" => OperatorKind.Logical,
                "<<" => OperatorKind.Shift,
                ">>" => OperatorKind.Shift,
                "==" => OperatorKind.Comparison,
                "!=" => OperatorKind.Comparison,
                "<" => OperatorKind.Comparison,
                ">" => OperatorKind.Comparison,
                "<=" => OperatorKind.Comparison,
                ">=" => OperatorKind.Comparison,
                "=" => OperatorKind.Assignment,
                "+=" => OperatorKind.ArithmeticAssignment,
                "-=" => OperatorKind.ArithmeticAssignment,
                "*=" => OperatorKind.ArithmeticAssignment,
                "/=" => OperatorKind.ArithmeticAssignment,
                "%=" => OperatorKind.ArithmeticAssignment,
                "&=" => OperatorKind.BitwiseAssignment,
                "|=" => OperatorKind.BitwiseAssignment,
                "^=" => OperatorKind.BitwiseAssignment,
                "<<=" => OperatorKind.ShiftAssignment,
                ">>=" => OperatorKind.ShiftAssignment,
                "." => OperatorKind.MemberAccess,
                "->" => OperatorKind.MemberAccess,
                "++" => OperatorKind.IncrementDecrement,
                "--" => OperatorKind.IncrementDecrement,
                "*" when operationKind == OperationKind.PrefixUnary => OperatorKind.PointerOperation,
                "&" when operationKind == OperationKind.PrefixUnary => OperatorKind.PointerOperation,
                "?" => OperatorKind.Comparison,
                ":" => OperatorKind.Comparison,
                "sizeof" => OperatorKind.SizeOf,
                _ => throw new InvalidOperationException("Unrecognized operator")
            };
        }

        private static ImplicitConversionTypeKind GetTypeKind(string typeName) =>
            typeName switch
            {
                "byte" => ImplicitConversionTypeKind.Integral,
                "sbyte" => ImplicitConversionTypeKind.Integral,
                "short" => ImplicitConversionTypeKind.Integral,
                "ushort" => ImplicitConversionTypeKind.Integral,
                "int" => ImplicitConversionTypeKind.Integral,
                "uint" => ImplicitConversionTypeKind.Integral,
                "long" => ImplicitConversionTypeKind.Integral,
                "ulong" => ImplicitConversionTypeKind.Integral,
                "float" => ImplicitConversionTypeKind.FloatingPoint,
                "double" => ImplicitConversionTypeKind.FloatingPoint,
                _ => ImplicitConversionTypeKind.Struct
            };

        private static (int min, int max) GetNumericTypeLog2Ranges(string typeName) =>
            typeName switch
            {
                "byte" => (0, 8),
                "sbyte" => (-7, 7),
                "short" => (-15, 15),
                "ushort" => (0, 16),
                "int" => (-31, 31),
                "uint" => (0, 32),
                "long" => (-63, 63),
                "ulong" => (0, 64),
                "float" => (-23, 23),
                "double" => (-53, 53),
                _ => throw new InvalidOperationException("Internal compiler error: type was thought to be numeric but wasn't")
            };
    }
}
