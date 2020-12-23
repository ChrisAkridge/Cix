using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace CixAntlrTest
{
    public sealed class CixListenerImpl : ICixListener
    {
        public void VisitTerminal(ITerminalNode node) { }

        public void VisitErrorNode(IErrorNode node) { throw new NotImplementedException(); }

        public void EnterEveryRule(ParserRuleContext ctx) { }

        public void ExitEveryRule(ParserRuleContext ctx) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.primaryExpression"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterPrimaryExpression(CixParser.PrimaryExpressionContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.primaryExpression"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitPrimaryExpression(CixParser.PrimaryExpressionContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.postfixExpression"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterPostfixExpression(CixParser.PostfixExpressionContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.postfixExpression"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitPostfixExpression(CixParser.PostfixExpressionContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.argumentExpressionList"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterArgumentExpressionList(CixParser.ArgumentExpressionListContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.argumentExpressionList"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitArgumentExpressionList(CixParser.ArgumentExpressionListContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.unaryExpression"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterUnaryExpression(CixParser.UnaryExpressionContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.unaryExpression"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitUnaryExpression(CixParser.UnaryExpressionContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.unaryOperator"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterUnaryOperator(CixParser.UnaryOperatorContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.unaryOperator"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitUnaryOperator(CixParser.UnaryOperatorContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.castExpression"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterCastExpression(CixParser.CastExpressionContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.castExpression"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitCastExpression(CixParser.CastExpressionContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.multiplicativeExpression"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterMultiplicativeExpression(CixParser.MultiplicativeExpressionContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.multiplicativeExpression"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitMultiplicativeExpression(CixParser.MultiplicativeExpressionContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.additiveExpression"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterAdditiveExpression(CixParser.AdditiveExpressionContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.additiveExpression"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitAdditiveExpression(CixParser.AdditiveExpressionContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.shiftExpression"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterShiftExpression(CixParser.ShiftExpressionContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.shiftExpression"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitShiftExpression(CixParser.ShiftExpressionContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.relationalExpression"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterRelationalExpression(CixParser.RelationalExpressionContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.relationalExpression"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitRelationalExpression(CixParser.RelationalExpressionContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.equalityExpression"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterEqualityExpression(CixParser.EqualityExpressionContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.equalityExpression"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitEqualityExpression(CixParser.EqualityExpressionContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.andExpression"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterAndExpression(CixParser.AndExpressionContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.andExpression"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitAndExpression(CixParser.AndExpressionContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.exclusiveOrExpression"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterExclusiveOrExpression(CixParser.ExclusiveOrExpressionContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.exclusiveOrExpression"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitExclusiveOrExpression(CixParser.ExclusiveOrExpressionContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.inclusiveOrExpression"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterInclusiveOrExpression(CixParser.InclusiveOrExpressionContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.inclusiveOrExpression"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitInclusiveOrExpression(CixParser.InclusiveOrExpressionContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.logicalAndExpression"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterLogicalAndExpression(CixParser.LogicalAndExpressionContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.logicalAndExpression"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitLogicalAndExpression(CixParser.LogicalAndExpressionContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.logicalOrExpression"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterLogicalOrExpression(CixParser.LogicalOrExpressionContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.logicalOrExpression"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitLogicalOrExpression(CixParser.LogicalOrExpressionContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.conditionalExpression"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterConditionalExpression(CixParser.ConditionalExpressionContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.conditionalExpression"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitConditionalExpression(CixParser.ConditionalExpressionContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.assignmentExpression"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterAssignmentExpression(CixParser.AssignmentExpressionContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.assignmentExpression"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitAssignmentExpression(CixParser.AssignmentExpressionContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.assignmentOperator"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterAssignmentOperator(CixParser.AssignmentOperatorContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.assignmentOperator"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitAssignmentOperator(CixParser.AssignmentOperatorContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.expression"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterExpression(CixParser.ExpressionContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.expression"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitExpression(CixParser.ExpressionContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.constantExpression"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterConstantExpression(CixParser.ConstantExpressionContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.constantExpression"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitConstantExpression(CixParser.ConstantExpressionContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.typeName"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterTypeName(CixParser.TypeNameContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.typeName"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitTypeName(CixParser.TypeNameContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.funcptrTypeName"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterFuncptrTypeName(CixParser.FuncptrTypeNameContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.funcptrTypeName"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitFuncptrTypeName(CixParser.FuncptrTypeNameContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.typeNameList"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterTypeNameList(CixParser.TypeNameListContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.typeNameList"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitTypeNameList(CixParser.TypeNameListContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.primitiveType"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterPrimitiveType(CixParser.PrimitiveTypeContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.primitiveType"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitPrimitiveType(CixParser.PrimitiveTypeContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.pointerAsteriskList"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterPointerAsteriskList(CixParser.PointerAsteriskListContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.pointerAsteriskList"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitPointerAsteriskList(CixParser.PointerAsteriskListContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.variableDeclarationStatement"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterVariableDeclarationStatement(CixParser.VariableDeclarationStatementContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.variableDeclarationStatement"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitVariableDeclarationStatement(CixParser.VariableDeclarationStatementContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.variableDeclarationWithInitializationStatement"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterVariableDeclarationWithInitializationStatement(CixParser.VariableDeclarationWithInitializationStatementContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.variableDeclarationWithInitializationStatement"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitVariableDeclarationWithInitializationStatement(CixParser.VariableDeclarationWithInitializationStatementContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.struct"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterStruct(CixParser.StructContext context) { Console.WriteLine($"Struct {context.Identifier()} with {context.structMember().Length} members"); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.struct"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitStruct(CixParser.StructContext context) { }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.structMember"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterStructMember(CixParser.StructMemberContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.structMember"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitStructMember(CixParser.StructMemberContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.structArraySize"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterStructArraySize(CixParser.StructArraySizeContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.structArraySize"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitStructArraySize(CixParser.StructArraySizeContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.globalVariableDeclaration"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterGlobalVariableDeclaration(CixParser.GlobalVariableDeclarationContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.globalVariableDeclaration"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitGlobalVariableDeclaration(CixParser.GlobalVariableDeclarationContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.function"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterFunction(CixParser.FunctionContext context) { Console.WriteLine($"Function {context.Identifier()}"); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.function"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitFunction(CixParser.FunctionContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.functionParameterList"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterFunctionParameterList(CixParser.FunctionParameterListContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.functionParameterList"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitFunctionParameterList(CixParser.FunctionParameterListContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.functionParameter"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterFunctionParameter(CixParser.FunctionParameterContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.functionParameter"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitFunctionParameter(CixParser.FunctionParameterContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.statement"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterStatement(CixParser.StatementContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.statement"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitStatement(CixParser.StatementContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.block"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterBlock(CixParser.BlockContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.block"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitBlock(CixParser.BlockContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.breakStatement"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterBreakStatement(CixParser.BreakStatementContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.breakStatement"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitBreakStatement(CixParser.BreakStatementContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.conditionalStatement"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterConditionalStatement(CixParser.ConditionalStatementContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.conditionalStatement"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitConditionalStatement(CixParser.ConditionalStatementContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.continueStatement"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterContinueStatement(CixParser.ContinueStatementContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.continueStatement"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitContinueStatement(CixParser.ContinueStatementContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.elseStatement"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterElseStatement(CixParser.ElseStatementContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.elseStatement"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitElseStatement(CixParser.ElseStatementContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.doWhileStatement"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterDoWhileStatement(CixParser.DoWhileStatementContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.doWhileStatement"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitDoWhileStatement(CixParser.DoWhileStatementContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.expressionStatement"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterExpressionStatement(CixParser.ExpressionStatementContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.expressionStatement"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitExpressionStatement(CixParser.ExpressionStatementContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.forStatement"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterForStatement(CixParser.ForStatementContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.forStatement"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitForStatement(CixParser.ForStatementContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.returnStatement"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterReturnStatement(CixParser.ReturnStatementContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.returnStatement"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitReturnStatement(CixParser.ReturnStatementContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.switchStatement"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterSwitchStatement(CixParser.SwitchStatementContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.switchStatement"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitSwitchStatement(CixParser.SwitchStatementContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.caseStatement"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterCaseStatement(CixParser.CaseStatementContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.caseStatement"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitCaseStatement(CixParser.CaseStatementContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.literalCaseStatement"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterLiteralCaseStatement(CixParser.LiteralCaseStatementContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.literalCaseStatement"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitLiteralCaseStatement(CixParser.LiteralCaseStatementContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.defaultCaseStatement"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterDefaultCaseStatement(CixParser.DefaultCaseStatementContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.defaultCaseStatement"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitDefaultCaseStatement(CixParser.DefaultCaseStatementContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.whileStatement"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterWhileStatement(CixParser.WhileStatementContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.whileStatement"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitWhileStatement(CixParser.WhileStatementContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.number"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterNumber(CixParser.NumberContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.number"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitNumber(CixParser.NumberContext context) { throw new NotImplementedException(); }

        /// <summary>
        /// Enter a parse tree produced by <see cref="CixParser.sourceFile"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void EnterSourceFile(CixParser.SourceFileContext context)
        {
            Console.WriteLine($"Source file with {context.function().Length} functions, {context.@struct().Length} structs, and {context.globalVariableDeclaration().Length} globals");
        }

        /// <summary>
        /// Exit a parse tree produced by <see cref="CixParser.sourceFile"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public void ExitSourceFile(CixParser.SourceFileContext context) { }
    }
}
