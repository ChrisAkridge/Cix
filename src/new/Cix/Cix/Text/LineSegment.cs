using System;
using System.Collections.Generic;
using System.Text;

namespace Cix.Text
{
	/// <summary>
	/// Represents a portion of a single line of text, plus the starting and ending column
	/// indices of the segment on the original line.
	/// </summary>
	/// <remarks>
	/// The comment remover will remove inline comments from the middle of lines, separating the
	/// line into before-the-comment and after-the-comment. We want to be able to keep track of
	/// where errors occur on a given line, and we want to do that using the column indices
	/// of the original source file that includes the comments. The LineSegment class lets us
	/// keep track of where the original column indices were at without having to keep the
	/// comments.
	/// </remarks>
    public sealed class LineSegment
    {
		public string Text { get; }
		public int StartColumn { get; }
		public int EndColumn { get; }

	    public LineSegment(string text, int startColumn, int endColumn)
	    {
		    Text = text;
		    StartColumn = startColumn;
		    EndColumn = endColumn;
	    }

	    public override string ToString() => $"{Text}";
    }
}
