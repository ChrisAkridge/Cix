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

		public List<FunctionParameter> Arguments { get; private set; }
		public bool HasVariableArgumentsEntry { get; private set; }

		public override void Print(StringBuilder builder, int depth)
		{
			// TODO: implement
		}
	}
}
