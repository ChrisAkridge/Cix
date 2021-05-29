using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celarix.Cix.Compiler.Emit.IronArc.Models;
using Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions;
using Celarix.Cix.Compiler.Parse.Models.AST.v1;
// ♥
using ArrayAccess = Celarix.Cix.Compiler.Parse.Models.AST.v1.ArrayAccess;
using BinaryExpression = Celarix.Cix.Compiler.Parse.Models.AST.v1.BinaryExpression;
using CastExpression = Celarix.Cix.Compiler.Parse.Models.AST.v1.CastExpression;
using FloatingPointLiteral = Celarix.Cix.Compiler.Parse.Models.AST.v1.FloatingPointLiteral;
using FunctionInvocation = Celarix.Cix.Compiler.Parse.Models.AST.v1.FunctionInvocation;
using HardwareCallReturnsInternal = Celarix.Cix.Compiler.Parse.Models.AST.v1.HardwareCallReturnsInternal;
using HardwareCallVoidInternal = Celarix.Cix.Compiler.Parse.Models.AST.v1.HardwareCallVoidInternal;
using Identifier = Celarix.Cix.Compiler.Parse.Models.AST.v1.Identifier;
using IntegerLiteral = Celarix.Cix.Compiler.Parse.Models.AST.v1.IntegerLiteral;
using SizeOfExpression = Celarix.Cix.Compiler.Parse.Models.AST.v1.SizeOfExpression;
using StringLiteral = Celarix.Cix.Compiler.Parse.Models.AST.v1.StringLiteral;
using TernaryExpression = Celarix.Cix.Compiler.Parse.Models.AST.v1.TernaryExpression;
using UnaryExpression = Celarix.Cix.Compiler.Parse.Models.AST.v1.UnaryExpression;
using TypedArrayAccess = Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions.ArrayAccess;
using TypedBinaryExpression = Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions.BinaryExpression;
using TypedCastExpression = Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions.CastExpression;
using TypedFloatingPointLiteral = Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions.FloatingPointLiteral;
using TypedFunctionInvocation = Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions.FunctionInvocation;
using TypedIntegerLiteral = Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions.IntegerLiteral;
using TypedHardwareCallReturnsInternal = Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions.HardwareCallReturnsInternal;
using TypedHardwareCallVoidInternal = Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions.HardwareCallVoidInternal;
using TypedIdentifier = Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions.Identifier;
using TypedSizeOfExpression = Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions.SizeOfExpression;
using TypedStringLiteral = Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions.StringLiteral;
using TypedTernaryExpression = Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions.TernaryExpression;
using TypedUnaryExpression = Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions.UnaryExpression;

namespace Celarix.Cix.Compiler.Emit.IronArc
{
    internal sealed class TypedExpressionBuilder
    {
        private readonly EmitContext context;

        public TypedExpressionBuilder(EmitContext context) => this.context = context;

        public TypedExpression Build(Expression expression)
        {
            return expression switch
            {
                ArrayAccess arrayAccess => new TypedArrayAccess
                {
                    Operand = Build(arrayAccess.Operand),
                    Index = Build(arrayAccess.Index),
                    OriginalCode = arrayAccess.PrettyPrint()
                },
                BinaryExpression binaryExpression => new TypedBinaryExpression
                {
                    Left = Build(binaryExpression.Left),
                    Right = Build(binaryExpression.Right),
                    Operator = binaryExpression.Operator,
                    OriginalCode = binaryExpression.PrettyPrint()
                },
                CastExpression castExpression => new TypedCastExpression
                {
                    Expression = Build(castExpression.Operand),
                    ToType = context.LookupDataTypeWithPointerLevel(castExpression.ToType),
                    OriginalCode = castExpression.PrettyPrint()
                },
                FloatingPointLiteral floatingPointLiteral => new TypedFloatingPointLiteral
                {
                    ValueBits = floatingPointLiteral.ValueBits,
                    NumericLiteralType = floatingPointLiteral.NumericLiteralType,
                    OriginalCode = floatingPointLiteral.PrettyPrint()
                },
                FunctionInvocation functionInvocation => new TypedFunctionInvocation
                {
                    Operand = Build(functionInvocation.Operand),
                    Arguments = functionInvocation.Arguments.Select(Build).ToList(),
                    OriginalCode = functionInvocation.PrettyPrint()
                },
                HardwareCallReturnsInternal hardwareCallReturnsInternal => new
                    TypedHardwareCallReturnsInternal
                    {
                        ASTNode = hardwareCallReturnsInternal,
                        CallName = hardwareCallReturnsInternal.CallName,
                        ReturnType = context.LookupDataTypeWithPointerLevel(hardwareCallReturnsInternal.ReturnType),
                        ParameterTypes = hardwareCallReturnsInternal.ParameterTypes
                            .Select(context.LookupDataTypeWithPointerLevel)
                            .ToList(),
                        OriginalCode = hardwareCallReturnsInternal.PrettyPrint()
                    },
                HardwareCallVoidInternal hardwareCallVoidInternal => new TypedHardwareCallVoidInternal
                {
                    ASTNode = hardwareCallVoidInternal,
                    CallName = hardwareCallVoidInternal.CallName,
                    ParameterTypes = hardwareCallVoidInternal.ParameterTypes
                        .Select(context.LookupDataTypeWithPointerLevel)
                        .ToList(),
                    OriginalCode = hardwareCallVoidInternal.PrettyPrint()
                },
                Identifier identifier => new TypedIdentifier
                {
                    Name = identifier.IdentifierText,
                    OriginalCode = identifier.PrettyPrint()
                },
                IntegerLiteral integerLiteral => new TypedIntegerLiteral
                {
                    ValueBits = integerLiteral.ValueBits,
                    LiteralType = integerLiteral.LiteralType,
                    OriginalCode = integerLiteral.PrettyPrint()
                },
                SizeOfExpression sizeOfExpression => new TypedSizeOfExpression
                {
                    Type = context.LookupDataTypeWithPointerLevel(sizeOfExpression.Type),
                    OriginalCode = sizeOfExpression.PrettyPrint()
                },
                StringLiteral stringLiteral => new TypedStringLiteral
                {
                    LiteralValue = stringLiteral.Value,
                    OriginalCode = stringLiteral.PrettyPrint()
                },
                TernaryExpression ternaryExpression => new TypedTernaryExpression
                {
                    Operand1 = Build(ternaryExpression.Operand1),
                    Operand2 = Build(ternaryExpression.Operand2),
                    Operand3 = Build(ternaryExpression.Operand3),
                    Operator1 = ternaryExpression.Operator1,
                    Operator2 = ternaryExpression.Operator2,
                    OriginalCode = ternaryExpression.PrettyPrint()
                },
                UnaryExpression unaryExpression => new TypedUnaryExpression
                {
                    Operand = Build(unaryExpression.Operand),
                    Operator = unaryExpression.Operator,
                    IsPostfix = unaryExpression.IsPostfix,
                    OriginalCode = unaryExpression.PrettyPrint()
                },
                _ => throw new InvalidOperationException("Internal compiler error: unrecognized expression type")
            };
        }
    }
}
