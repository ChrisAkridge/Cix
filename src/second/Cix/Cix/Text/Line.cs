using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.Text
{
	public sealed class Line
	{
		public string FilePath { get; }
		public int LineNumber { get; }
		public string Text { get; }

		public Line(string filePath, int lineNumber, string text)
		{
			FilePath = filePath;
			LineNumber = lineNumber;
			Text = text;
		}

		public override string ToString() => $"{LineNumber + 1}: {Text}";
	}
}
