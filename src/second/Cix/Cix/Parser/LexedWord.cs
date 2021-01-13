using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.Parser
{
	public sealed class LexedWord
	{
		public string FilePath { get; }
		public int LineNumber { get; }
		public int WordNumber { get; }
		public string Text { get; }

		public LexedWord(string filePath, int lineNumber, int wordNumber, string text)
		{
			FilePath = filePath;
			LineNumber = lineNumber;
			WordNumber = wordNumber;
			Text = text;
		}

		public override string ToString() => $"{LineNumber}:{WordNumber} {Text}";
	}
}
