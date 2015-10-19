using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.Exceptions
{
	[Serializable]
	public sealed class PreprocessingException : Exception
	{
		public PreprocessingException() : base("An exception has occurred in the preprocessor.")
		{
		}

		public PreprocessingException(string message) : base(string.Format("An exception has occurred in the preprocessor: {0}", message))
		{
		}

		public PreprocessingException(string message, Exception innerException) : base(string.Format("An exception has occurred in the preprocessor: {0}", message), innerException)
		{
		}
	}
}
