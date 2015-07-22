using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Cix.Exceptions
{
	public sealed class ASTException : Exception
	{
		public ASTException() : base("A semantically invalid portion of code has been passed to the AST generator.") { }
		public ASTException(string message) : base(message) { }
		public ASTException(string message, Exception innerException) : base(message, innerException) { }
		private ASTException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}
