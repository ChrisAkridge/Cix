using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Celarix.Cix.Compiler.Parse.Models.AST
{
    public abstract class ASTNode
    {
    }

    public sealed class SourceFile : ASTNode
    {
        public List<Struct> Structs { get; set; }
        public List<GlobalVariableDeclaration> GlobalVariableDeclarations { get; set; }
        public List<Function> Functions { get; set; }
    }

    public sealed class Struct : ASTNode
    {
        public string Name { get; set; }
        public List<StructMember> Members { get; set; }
    }

    public sealed class StructMember : ASTNode
    {
        public DataType Type { get; set; }
        public string Name { get; set; }
        public int StructArraySize { get; set; }
    }

    public abstract class DataType : ASTNode
    {
        public int PointerLevel { get; set; }
    }

    public sealed class NamedDataType : DataType
    {
        public string Name { get; set; }
    }

    public sealed class FuncptrDataType : DataType
    {
        public List<DataType> Types { get; set; }
    }

    public sealed class GlobalVariableDeclaration : ASTNode
    {
        public VariableDeclaration Declaration { get; set; }
    }

    public sealed class GlobalVariableDeclarationWithInitialization : ASTNode
    {
        public VariableDeclarationWithInitialization Declaration { get; set; }
    }

    public class VariableDeclaration : Statement
    {
        public DataType Type { get; set; }
        public string Name { get; set; }
    }

    public sealed class VariableDeclarationWithInitialization : VariableDeclaration
    {
        public Expression Initializer { get; set; }
    }

    public sealed class Function : ASTNode
    {
        public DataType ReturnType { get; set; }
        public string Name { get; set; }
        public List<FunctionParameter> Parameters { get; set; }
        public List<Statement> Statements { get; set; }
    }

    public sealed class FunctionParameter : ASTNode
    {
        public DataType Type { get; set; }
        public string Name { get; set; }
    }
    
    public abstract class Statement : ASTNode { }

    public sealed class Block : Statement
    {
        public List<Statement> Statements { get; set; }
    }
    
    public sealed class BreakStatement : Statement { }

    public sealed class ConditionalStatement : Statement
    {
        public Expression Condition { get; set; }
        public Statement IfTrue { get; set; }
        public Statement IfFalse { get; set; }
    }

    public sealed class ContinueStatement : Statement
    {
        
    }

    public sealed class DoWhileStatement : Statement
    {
        public Statement LoopStatement { get; set; }
        public Expression Condition { get; set; }
    }

    public sealed class ExpressionStatement : Statement
    {
        public Expression Expression { get; set; }
    }

    public sealed class ForStatement : Statement
    {
        public Expression Initializer { get; set; }
        public Expression Condition { get; set; }
        public Expression Iterator { get; set; }
        public Statement LoopStatement { get; set; }
    }

    public sealed class ReturnStatement : Statement
    {
        public Expression ReturnValue { get; set; }
    }

    public sealed class SwitchStatement : Statement
    {
        public Expression Expression { get; set; }
        public List<CaseStatement> Cases { get; set; }
    }

    public class CaseStatement : Statement
    {
        public Literal CaseLiteral { get; set; }
        public Statement Statement { get; set; }
    }

    public sealed class WhileStatement : Statement
    {
        public Expression Condition { get; set; }
        public Statement LoopStatement { get; set; }
    }

    public abstract class Expression { }

    public sealed class UnaryExpression : Expression
    {
        public Expression Operand { get; set; }
        public string Operator { get; set; }
        public bool IsPostfix { get; set; }
    }

    public sealed class BinaryExpression : Expression
    {
        public Expression Left { get; set; }
        public string Operator { get; set; }
        public Expression Right { get; set; }
    }

    public sealed class TernaryExpression : Expression
    {
        public Expression Operand1 { get; set; }
        public string Operator1 { get; set; }
        public Expression Operand2 { get; set; }
        public string Operator2 { get; set; }
        public Expression Operand3 { get; set; }
    }
    
    public sealed class CastExpression : Expression
    {
        public DataType ToType { get; set; }
        public Expression Operand { get; set; }
    }

    public sealed class SizeOfExpression : Expression
    {
        public DataType Type { get; set; }
    }

    public sealed class ArrayAccess : Expression
    {
        public Expression Operand { get; set; }
        public Expression Index { get; set; }
    }

    public sealed class FunctionInvocation : Expression
    {
        public Expression Operand { get; set; }
        public List<Expression> Arguments { get; set; }
    }
    
    public abstract class Literal : Expression { }

    public sealed class IntegerLiteral : Literal
    {
        public ulong ValueBits { get; set; }
    }

    public sealed class FloatingPointLiteral : Literal
    {
        public double ValueBits { get; set; }
    }

    public sealed class StringLiteral : Literal
    {
        public string Value { get; set; }
    }

    public sealed class Identifier : Literal
    {
        public string IdentifierText { get; set; }
    }
}
