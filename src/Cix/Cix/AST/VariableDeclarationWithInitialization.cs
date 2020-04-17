using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class VariableDeclarationWithInitialization : Element
	{
		public DataType VariableType { get; }
		public string VariableName { get; }

		// Note: in, say, int i = 4 + y, the OpAssignment operator is implied;
		// it should not be included in the assignment expression
		public Expression AssignmentExpression { get; }

		public VariableDeclarationWithInitialization(DataType variableType, string variableName, Expression assignmentExpression)
		{
			VariableType = variableType;
			VariableName = variableName;
			AssignmentExpression = assignmentExpression;
		}

		public override void Print(StringBuilder builder, int depth)
		{
			builder.AppendLineWithIndent($"{VariableType} {VariableName} = {AssignmentExpression}", depth);
		}
	}
}
