using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.Text
{
	internal struct LineChar
	{
		public int Column { get; }
		public char Text { get; }

		public LineChar(int column, char text)
		{
			Column = column;
			Text = text;
		}
	}

	internal sealed class LineCharEnumerator : IEnumerator<LineChar>
	{
		private readonly IList<Line> lines;
		private int currentLineIndex = -1;
		private int currentCharIndex = -1;

		object IEnumerator.Current => Current;

		public LineChar Previous { get; private set; }

		public LineChar Current => CharAt(currentLineIndex, currentCharIndex);

		public LineChar Next => PeekNext();

		public Line CurrentLine => lines[currentLineIndex];

		public LineCharEnumerator(IEnumerable<Line> lines) => this.lines = lines.ToList();

		public bool MoveNext()
		{
			if (currentLineIndex >= 0) { Previous = Current; }
			else { currentLineIndex = 0; }

			Line currentLine = lines[currentLineIndex];
			if (currentCharIndex >= currentLine.Text.Length - 1)
			{
				currentLineIndex++;
				if (currentLineIndex >= lines.Count) { return false;}
				currentCharIndex = 0;
				return true;
			}

			currentCharIndex++;
			return true;
		}

		private LineChar PeekNext()
		{
			Line currentLine = lines[currentLineIndex];
			if (currentCharIndex == currentLine.Text.Length - 1)
			{ 
				return currentLineIndex == lines.Count - 1 
				? default(LineChar) 
				: CharAt(currentLineIndex + 1, 0);
			}
			return CharAt(currentLineIndex, currentCharIndex + 1);
		}

		public void Dispose() { }

		public void Reset() => currentLineIndex = currentCharIndex = 0;

		public LineCharEnumerator GetEnumerator() => this;

		private LineChar CharAt(int line, int column) => !string.IsNullOrEmpty(lines[line].Text)
			? new LineChar(column, lines[line].Text[column])
			: new LineChar(0, ' ');
	}
}
