using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cix.Text
{
	/// <summary>
	/// Represents a single line of text, its line number, and what file it came from.
	/// </summary>
	/// <remarks>
	/// When the preprocessor runs, it can include the text of other files in place of #include
	/// directives. Lines from included files will have a filepath of the file it originally came
	/// from, along with the line number of that included file.
	/// </remarks>
    public sealed class Line
	{
		private readonly List<LineSegment> segments;

		public IReadOnlyList<LineSegment> Segments => segments.AsReadOnly();
		public string FilePath { get; }
		public int LineNumber { get;}

		public string Text => string.Join("", segments.Select(s => s.Text));

		public Line(IEnumerable<LineSegment> segments, string filePath, int lineNumber)
		{
			this.segments = segments.Where(s => !string.IsNullOrEmpty(s.Text)).ToList();
			FilePath = filePath;
			LineNumber = lineNumber;
		}

		public Line(string lineText, string filePath, int lineNumber)
			: this(new[] {new LineSegment(lineText, 1, lineText.Length)}, filePath, lineNumber)
		{ }

		public override string ToString() => $"{Text}";
	}
}
