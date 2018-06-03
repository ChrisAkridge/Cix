using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cix.Text
{
    internal static class LineSplitter
    {
	    public static IEnumerable<Line> SplitFileTextIntoLines(string inputText, string filePath)
	    {
		    if (string.IsNullOrWhiteSpace(inputText))
		    {
			    return new List<Line>();
		    }

		    string[] unprocessedLines =
			    inputText.Split(new[] {"\r\n", "\r", "\n"}, StringSplitOptions.None);
		    int currentLineNumber = 1;

		    List<Line> result =
			    unprocessedLines.Select(u => new Line(u.Trim(), filePath, currentLineNumber++))
				.ToList();

		    return result;
	    }
    }
}
