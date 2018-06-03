using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Cix.Errors;
using Cix.Text;

namespace Cix
{
    public static class CommentRemover
    {
	    public static IEnumerable<Line> RemoveComments(IEnumerable<Line> lines)
	    {
		    int lineNumber = 0;
		    var uncommentedLines = new List<Line>();
		    bool inMultilineComment = false;

		    foreach (Line line in lines)
		    {
			    lineNumber++;

				IList<(int startIndex, int endIndex)> commentLocations =
					FindCommentsOnLine(line.Text, ref inMultilineComment, Path.GetFileName(line.FilePath), lineNumber)
					.ToList();

			    uncommentedLines.Add(RemoveCommentsFromLine(line, commentLocations));
		    }

		    return uncommentedLines;
	    }

	    private static IEnumerable<(int startIndex, int endIndex)> FindCommentsOnLine(string line, 
		    ref bool inMultilineComment, string fileName, int lineNumber)
	    {
		    var commentIndices = new List<(int startIndex, int endIndex)>();
		    int currentCommentStartIndex = -1;
		    bool inComment = false;

		    if (inMultilineComment)
		    {
			    currentCommentStartIndex = 0;
			    inComment = true;
		    }

			for (int i = 0; i < line.Length; i++)
			{
			    if (line[i] == '/' && !inMultilineComment)
			    {
				    if (i == line.Length - 1)
				    {
					    // A single / was found at the end of the line. It doesn't start a
					    // comment, and it isn't part of a string literal, so it's a syntax
					    // error.
					    ErrorContext.AddError(
						    ErrorSource.CommentRemover, 1, "Single forward slash at end of line is not a valid comment.",
						    fileName, lineNumber, i);
					    continue;
				    }
				    else if (line[i + 1] == '/' && !inComment)
				    {
					    inComment = true;
					    currentCommentStartIndex = i;
				    }
				    else if (line[i + 1] == '*')
				    {
					    inComment = true;
					    inMultilineComment = true;
					    currentCommentStartIndex = i;
				    }
			    }
			    else if (line[i] == '*')
			    {
				    if (i < line.Length - 1 && line[i + 1] == '/')
				    {
					    if (!inMultilineComment)
					    {
							// We can't end a multiline comment before we start it.
							ErrorContext.AddError(
								ErrorSource.CommentRemover, 2, "Found a */ without a /* to start it.",
								fileName, lineNumber, i);
						    continue;
					    }

					    inComment = false;
					    inMultilineComment = false;
					    commentIndices.Add((currentCommentStartIndex, i + 1));
					    currentCommentStartIndex = -1;
				    }
			    }
		    }

		    if (currentCommentStartIndex > -1)
		    {
				commentIndices.Add((currentCommentStartIndex, line.Length - 1));
		    }

		    return commentIndices;
	    }

	    private static Line RemoveCommentsFromLine(Line line,
		    IList<(int startIndex, int endIndex)> commentLocations)
	    {
		    int currentCommentIndex = 0;
		    int includedTextStartIndex = 0;
		    var segments = new List<LineSegment>();
		    var segmentBuilder = new StringBuilder();

			if (!commentLocations.Any())
		    {
			    return new Line(line.Text, line.FilePath, line.LineNumber);
		    }

		    for (int column = 0; column < line.Text.Length; column++)
		    {
			    if (column == commentLocations[currentCommentIndex].startIndex)
			    {
				    segments.Add(new LineSegment(segmentBuilder.ToString(), includedTextStartIndex, column));
				    segmentBuilder.Clear();

				    // We've found the start of a comment and should skip to the end of it.
				    if (currentCommentIndex < commentLocations.Count - 1)
				    {
					    includedTextStartIndex = column = commentLocations[currentCommentIndex].endIndex + 1;
					    currentCommentIndex++;
				    }
				    else
				    {
					    // If there is one, that is.
					    column = commentLocations[currentCommentIndex].endIndex;
					    if (column == line.Text.Length - 1)
					    {
						    segments.Add(new LineSegment(segmentBuilder.ToString(), includedTextStartIndex, column));
							break;
						}
				    }
			    }
			    else
			    {
				    // We're not in a comment, so include the column.
				    segmentBuilder.Append(line.Text[column]);
			    }
		    }

		    if (segmentBuilder.Length > 0)
		    {
			    segments.Add(new LineSegment(segmentBuilder.ToString(), includedTextStartIndex,
				    line.Text.Length - 1));
		    }
		    return new Line(segments, line.FilePath, line.LineNumber);
	    }
    }
}
