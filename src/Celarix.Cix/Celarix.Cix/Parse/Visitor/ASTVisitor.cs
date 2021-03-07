using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celarix.Cix.Compiler.Parse.Models.AST.v1;

namespace Celarix.Cix.Compiler.Parse.Visitor
{
    internal abstract class ASTVisitor
    {
        public virtual void VisitBlock(Block block) { }
        public virtual void VisitBreakStatement(BreakStatement breakStatement) { }
        public virtual void VisitCaseStatement(CaseStatement caseStatement) { }
        public virtual void VisitConditionalStatement(ConditionalStatement conditionalStatement) { }
        public virtual void VisitContinueStatement(ContinueStatement continueStatement) { }
        public virtual void VisitDoWhileStatement(DoWhileStatement doWhileStatement) { }
        public virtual void VisitExpressionStatement(ExpressionStatement expressionStatement) { }
        public virtual void VisitForStatement(ForStatement forStatement) { }
        public virtual void VisitFunction(Function function) { }
        public virtual void VisitFunctionParameter(FunctionParameter functionParameter) { }
        public virtual void VisitGlobalVariableDeclaration(GlobalVariableDeclaration globalVariableDeclaration) { }
        public virtual void VisitGlobalVariableDeclarationWithInitialization(GlobalVariableDeclarationWithInitialization globalVariableDeclarationWithInitialization) { }
        public virtual void VisitReturnStatement(ReturnStatement returnStatement) { }
        public virtual void VisitStruct(Struct @struct) { }
        public virtual void VisitStructMember(StructMember structMember) { }
        public virtual void VisitSwitchStatement(SwitchStatement switchStatement) { }
        public virtual void VisitVariableDeclaration(VariableDeclaration variableDeclaration) { }
        public virtual void VisitVariableDeclarationWithInitialization(VariableDeclarationWithInitialization variableDeclarationWithInitialization) { }
        public virtual void VisitWhileStatement(WhileStatement whileStatement) { }
        
        public virtual void VisitArrayAccess(ArrayAccess arrayAccess) { }
        public virtual void VisitBinaryExpression(BinaryExpression binaryExpression) { }
        public virtual void VisitCastExpression(CastExpression castExpression) { }
        public virtual void VisitFloatingPointLiteral(FloatingPointLiteral floatingPointLiteral) { }
        public virtual void VisitFunctionInvocation(FunctionInvocation functionInvocation) { }
        public virtual void VisitHardwareCallReturnsInternal(HardwareCallReturnsInternal hardwareCallReturnsInternal) { }
        public virtual void VisitHardwareCallVoidInternal(HardwareCallVoidInternal hardwareCallVoidInternal) { }
        public virtual void VisitIdentifier(Identifier identifier) { }
        public virtual void VisitIntegerLiteral(IntegerLiteral integerLiteral) { }
        public virtual void VisitSizeOfExpression(SizeOfExpression sizeOfExpression) { }
        public virtual void VisitStringLiteral(StringLiteral stringLiteral) { }
        public virtual void VisitTernaryExpression(TernaryExpression ternaryExpression) { }
        public virtual void VisitUnaryExpression(UnaryExpression unaryExpression) { }

        public static void VisitSourceFile(ASTVisitor visitor, SourceFileRoot sourceFile)
        {
            foreach (var @struct in sourceFile.Structs)
            {
                visitor.VisitStruct(@struct);

                foreach (var member in @struct.Members)
                {
                    visitor.VisitStructMember(member);
                }
            }

            foreach (var globalVariableDeclaration in sourceFile.GlobalVariableDeclarations)
            {
                if (globalVariableDeclaration is GlobalVariableDeclarationWithInitialization globalVariableDeclarationWithInitialization)
                {
                    visitor.VisitGlobalVariableDeclarationWithInitialization(globalVariableDeclarationWithInitialization);
                }
                else
                {
                    visitor.VisitGlobalVariableDeclaration(globalVariableDeclaration);
                }
            }

            foreach (var function in sourceFile.Functions)
            {
                visitor.VisitFunction(function);

                foreach (var functionParameter in function.Parameters)
                {
                    visitor.VisitFunctionParameter(functionParameter);
                }

                foreach (var statement in function.Statements)
                {
                    VisitStatement(visitor, statement);
                }
            }
        }

        private static void VisitStatement(ASTVisitor visitor, Statement statement)
        {
            while (true)
            {
                switch (statement)
                {
                    case Block block:
                    {
                        foreach (var blockStatement in block.Statements) { VisitStatement(visitor, blockStatement); }

                        break;
                    }
                    case BreakStatement breakStatement:
                        visitor.VisitBreakStatement(breakStatement);

                        break;
                    case CaseStatement caseStatement:
                        visitor.VisitCaseStatement(caseStatement);

                        VisitLiteral(visitor, caseStatement.CaseLiteral);
                        statement = caseStatement.Statement;

                        continue;

                    case ConditionalStatement conditionalStatement:
                    {
                        visitor.VisitConditionalStatement(conditionalStatement);

                        VisitExpression(visitor, conditionalStatement.Condition);
                        VisitStatement(visitor, conditionalStatement.IfTrue);

                        if (conditionalStatement.IfFalse != null)
                        {
                            statement = conditionalStatement.IfFalse;

                            continue;
                        }

                        break;
                    }
                    case ContinueStatement continueStatement:
                        visitor.VisitContinueStatement(continueStatement);

                        break;
                    case DoWhileStatement doWhileStatement:
                        visitor.VisitDoWhileStatement(doWhileStatement);

                        VisitStatement(visitor, doWhileStatement.LoopStatement);
                        VisitExpression(visitor, doWhileStatement.Condition);

                        break;
                    case ExpressionStatement expressionStatement:
                        visitor.VisitExpressionStatement(expressionStatement);

                        VisitExpression(visitor, expressionStatement.Expression);

                        break;
                    case ForStatement forStatement:
                        visitor.VisitForStatement(forStatement);

                        VisitExpression(visitor, forStatement.Initializer);
                        VisitExpression(visitor, forStatement.Condition);
                        VisitExpression(visitor, forStatement.Iterator);
                        statement = forStatement.LoopStatement;

                        continue;

                    case ReturnStatement returnStatement:
                    {
                        visitor.VisitReturnStatement(returnStatement);

                        if (returnStatement.ReturnValue != null) { VisitExpression(visitor, returnStatement.ReturnValue); }

                        break;
                    }
                    case SwitchStatement switchStatement:
                    {
                        visitor.VisitSwitchStatement(switchStatement);

                        VisitExpression(visitor, switchStatement.Expression);

                        foreach (var switchCaseStatement in switchStatement.Cases) { VisitStatement(visitor, switchCaseStatement); }

                        break;
                    }
                    case VariableDeclaration variableDeclaration when variableDeclaration is VariableDeclarationWithInitialization variableDeclarationWithInitialization:
                        visitor.VisitVariableDeclarationWithInitialization(variableDeclarationWithInitialization);

                        break;
                    case VariableDeclaration variableDeclaration:
                        visitor.VisitVariableDeclaration(variableDeclaration);

                        break;
                    case WhileStatement whileStatement:
                        visitor.VisitWhileStatement(whileStatement);

                        VisitExpression(visitor, whileStatement.Condition);
                        statement = whileStatement.LoopStatement;

                        continue;
                }

                break;
            }
        }

        private static void VisitExpression(ASTVisitor visitor, Expression expression)
        {
            while (true)
            {
                switch (expression)
                {
                    case ArrayAccess arrayAccess:
                        visitor.VisitArrayAccess(arrayAccess);

                        VisitExpression(visitor, arrayAccess.Operand);
                        expression = arrayAccess.Index;

                        continue;

                    case BinaryExpression binaryExpression:
                        visitor.VisitBinaryExpression(binaryExpression);

                        VisitExpression(visitor, binaryExpression.Left);
                        expression = binaryExpression.Right;

                        continue;

                    case CastExpression castExpression:
                        visitor.VisitCastExpression(castExpression);

                        expression = castExpression.Operand;

                        continue;

                    case FunctionInvocation functionInvocation:
                    {
                        visitor.VisitFunctionInvocation(functionInvocation);

                        VisitExpression(visitor, functionInvocation.Operand);

                        foreach (var argument in functionInvocation.Arguments) { VisitExpression(visitor, argument); }

                        break;
                    }
                    case HardwareCallReturnsInternal hardwareCallReturnsInternal:
                        visitor.VisitHardwareCallReturnsInternal(hardwareCallReturnsInternal);

                        break;
                    case HardwareCallVoidInternal hardwareCallVoidInternal:
                        visitor.VisitHardwareCallVoidInternal(hardwareCallVoidInternal);

                        break;
                    case Identifier identifier:
                        visitor.VisitIdentifier(identifier);

                        break;
                    case SizeOfExpression sizeOfExpression:
                        visitor.VisitSizeOfExpression(sizeOfExpression);

                        break;
                    case TernaryExpression ternaryExpression:
                        visitor.VisitTernaryExpression(ternaryExpression);

                        VisitExpression(visitor, ternaryExpression.Operand1);
                        VisitExpression(visitor, ternaryExpression.Operand2);
                        expression = ternaryExpression.Operand3;

                        continue;

                    case UnaryExpression unaryExpression:
                        visitor.VisitUnaryExpression(unaryExpression);

                        expression = unaryExpression.Operand;

                        continue;
                }

                break;
            }
        }

        private static void VisitLiteral(ASTVisitor visitor, Literal literal)
        {
            switch (literal)
            {
                case IntegerLiteral integerLiteral:
                    visitor.VisitIntegerLiteral(integerLiteral);
                    break;
                case FloatingPointLiteral floatingPointLiteral:
                    visitor.VisitFloatingPointLiteral(floatingPointLiteral);
                    break;
                case StringLiteral stringLiteral:
                    visitor.VisitStringLiteral(stringLiteral);
                    break;
            }
        }
    }
}
