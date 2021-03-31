using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions;
using Celarix.Cix.Compiler.Parse.Models.AST.v1;
// ♥
using ArrayAccess = Celarix.Cix.Compiler.Parse.Models.AST.v1.ArrayAccess;
using BinaryExpression = Celarix.Cix.Compiler.Parse.Models.AST.v1.BinaryExpression;
using CastExpression = Celarix.Cix.Compiler.Parse.Models.AST.v1.CastExpression;
using FloatingPointLiteral = Celarix.Cix.Compiler.Parse.Models.AST.v1.FloatingPointLiteral;
using FunctionInvocation = Celarix.Cix.Compiler.Parse.Models.AST.v1.FunctionInvocation;
using HardwareCallReturnsInternal = Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions.HardwareCallReturnsInternal;
using HardwareCallVoidInternal = Celarix.Cix.Compiler.Parse.Models.AST.v1.HardwareCallVoidInternal;
using Identifier = Celarix.Cix.Compiler.Parse.Models.AST.v1.Identifier;
using SizeOfExpression = Celarix.Cix.Compiler.Parse.Models.AST.v1.SizeOfExpression;
using StringLiteral = Celarix.Cix.Compiler.Parse.Models.AST.v1.StringLiteral;
using TernaryExpression = Celarix.Cix.Compiler.Parse.Models.AST.v1.TernaryExpression;
using UnaryExpression = Celarix.Cix.Compiler.Parse.Models.AST.v1.UnaryExpression;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    internal sealed class TypedExpressionBuilder
    {
        private readonly ExpressionEmitContext context;

        public TypedExpressionBuilder(ExpressionEmitContext context) => this.context = context;

        public TypedExpression Build(Expression expression)
        {
            return expression switch
            {
                ArrayAccess arrayAccess => new TypedExpressions.ArrayAccess
                {
                    Operand = Build(arrayAccess.Operand), Index = Build(arrayAccess.Index)
                },
                BinaryExpression binaryExpression => new TypedExpressions.BinaryExpression
                {
                    Left = Build(binaryExpression.Left),
                    Right = Build(binaryExpression.Right),
                    Operator = binaryExpression.Operator
                },
                CastExpression castExpression => new TypedExpressions.CastExpression
                {
                    Expression = Build(castExpression.Operand),
                    ToType = context.LookupDataTypeWithPointerLevel(castExpression.ToType)
                },
                FloatingPointLiteral floatingPointLiteral => new TypedExpressions.FloatingPointLiteral
                {
                    ValueBits = floatingPointLiteral.ValueBits,
                    NumericLiteralType = floatingPointLiteral.NumericLiteralType
                },
                FunctionInvocation functionInvocation => new TypedExpressions.FunctionInvocation
                {
                    Operand = Build(functionInvocation.Operand),
                    Arguments = functionInvocation.Arguments.Select(Build).ToList()
                },
                Celarix.Cix.Compiler.Parse.Models.AST.v1.HardwareCallReturnsInternal hardwareCallReturnsInternal => new
                    HardwareCallReturnsInternal
                    {
                        ASTNode = hardwareCallReturnsInternal,
                        CallName = hardwareCallReturnsInternal.CallName,
                        ReturnType = context.LookupDataTypeWithPointerLevel(hardwareCallReturnsInternal.ReturnType),
                        ParameterTypes = hardwareCallReturnsInternal.ParameterTypes
                            .Select(context.LookupDataTypeWithPointerLevel)
                            .ToList(),
                    },
                HardwareCallVoidInternal hardwareCallVoidInternal => new TypedExpressions.HardwareCallVoidInternal
                {
                    ASTNode = hardwareCallVoidInternal,
                    CallName = hardwareCallVoidInternal.CallName,
                    ParameterTypes = hardwareCallVoidInternal.ParameterTypes
                        .Select(context.LookupDataTypeWithPointerLevel)
                        .ToList()
                },
                Identifier identifier => new TypedExpressions.Identifier
                {
                    Name = identifier.IdentifierText
                },
                SizeOfExpression sizeOfExpression => new TypedExpressions.SizeOfExpression
                {
                    Type = context.LookupDataTypeWithPointerLevel(sizeOfExpression.Type)
                },
                StringLiteral stringLiteral => new TypedExpressions.StringLiteral
                {
                    LiteralValue = stringLiteral.Value
                },
                TernaryExpression ternaryExpression => new TypedExpressions.TernaryExpression
                {
                    Operand1 = Build(ternaryExpression.Operand1),
                    Operand2 = Build(ternaryExpression.Operand2),
                    Operand3 = Build(ternaryExpression.Operand3),
                    Operator1 = ternaryExpression.Operator1,
                    Operator2 = ternaryExpression.Operator2
                },
                UnaryExpression unaryExpression => new TypedExpressions.UnaryExpression
                {
                    Operand = Build(unaryExpression.Operand),
                    Operator = unaryExpression.Operator,
                    IsPostfix = unaryExpression.IsPostfix
                },
                _ => throw new InvalidOperationException("Internal compiler error: unrecognized expression type")
            };
        }
    }
}
