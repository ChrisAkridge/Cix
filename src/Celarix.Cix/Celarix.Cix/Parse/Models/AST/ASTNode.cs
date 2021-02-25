using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celarix.Cix.Compiler.Parse.Models.AST
{
    public abstract class ASTNode
    {
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
        public int PointerLevel { get; set; }
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

    public class VariableDeclaration : ASTNode
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
        public List<FunctionParameter> FunctionParameters { get; set; }
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
    
    public     
}
