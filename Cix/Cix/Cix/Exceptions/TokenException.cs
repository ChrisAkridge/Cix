using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.Exceptions
{
	[Serializable]
	public sealed class TokenException : Exception
	{
		private string failedWord;

		public TokenException(string failedWord) : base(string.Format("Invalid word {0} in tokenization", failedWord))
		{
			this.failedWord = failedWord;
		}

		public TokenException(string failedWord, string message) : base(string.Format("Invalid word {0} in tokenization: {1}", failedWord, message))
		{
			this.failedWord = failedWord;
		}

		public TokenException(string failedWord, string message, Exception innerException) : base(string.Format("Invalid word {0} in tokenization: {1}", failedWord, message), innerException)
		{
			this.failedWord = failedWord;
		}
	}
}
