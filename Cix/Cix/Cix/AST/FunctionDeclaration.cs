using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public class FunctionDeclaration : Element
	{
		public DataType ReturnType { get; private set; }
		public string Name { get; private set; }

		public List<FunctionArgument> Arguments { get; private set; }
		public bool HasVariableArgumentsEntry { get; private set; }

		// public List<Statement> Statements
	}
}
