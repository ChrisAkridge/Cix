using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class VariableDeclarationWithInitialization : Element
	{
		public DataType VariableType { get; private set; }
		public string VariableName { get; private set; }

		// Notes: in, say, int i = 4 + y, the OpAssignment operator is implied;
		// it should not be included in the assignment expression
		public Expression AssignmentExpression { get; private set; }

		public VariableDeclarationWithInitialization(DataType variableType, string variableName, Expression assignmentExpression)
		{
			VariableType = variableType;
			VariableName = variableName;
			AssignmentExpression = assignmentExpression;
		}
	}
}
