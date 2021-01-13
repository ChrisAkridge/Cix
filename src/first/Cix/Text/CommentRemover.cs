using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cix.Errors;

namespace Cix.Text
{
	internal sealed class CommentRemover
	{
		private IErrorListProvider errorList;

		public CommentRemover(IErrorListProvider errorList) => this.errorList = errorList;

		public IList<Line> RemoveComments(IList<Line> file)
		{
			var fileWithoutComments = new List<Line>();
			var inMultilineComment = false;

			foreach (Line line in file)
			{
				if (line.Text.Contains("//"))
				{
					string lineWithoutComment = RemoveCommentFromEndOfLine(line.Text, "//");

					if (!string.IsNullOrEmpty(lineWithoutComment))
					{
						fileWithoutComments.Add(new Line(line.FilePath, line.LineNumber,
							lineWithoutComment));
					}
				}
				else if (line.Text.Contains("/*") && !inMultilineComment)
				{
					if (!line.Text.Contains("*/"))
					{
						inMultilineComment = true;
					}
					else
					{
						inMultilineComment = false;
						fileWithoutComments.Add(new Line(line.FilePath, line.LineNumber,
							RemoveCommentFromMiddleOfLine(line.Text)));
					}
				}
				else if (inMultilineComment)
				{
					if (line.Text.Contains("*/"))
					{
						inMultilineComment = false;
						fileWithoutComments.Add(new Line(line.FilePath, line.LineNumber,
							RemoveCommentFromStartOfLine(line.Text)));
					}
				}
				else { fileWithoutComments.Add(line); }
			}

			return fileWithoutComments;
		}

		private static string RemoveCommentFromStartOfLine(string lineText)
		{
			int indexOfCommentEnd = lineText.IndexOf("*/", StringComparison.Ordinal);
			return lineText.Substring(indexOfCommentEnd + 2);
		}

		private static string RemoveCommentFromEndOfLine(string lineText, string commentStart)
		{
			int indexOfDoubleSlash = lineText.IndexOf(commentStart, StringComparison.Ordinal);
			return StringExtensions.Substring(lineText, 0, indexOfDoubleSlash);
		}

		private static string RemoveCommentFromMiddleOfLine(string lineText)
		{
			int indexOfCommentStart = lineText.IndexOf("/*", StringComparison.Ordinal);
			int indexOfCommentEnd = lineText.IndexOf("*/", StringComparison.Ordinal);

			string lineStart = StringExtensions.Substring(lineText, 0, indexOfCommentStart);
			string lineEnd = lineText.Substring(indexOfCommentEnd + 2);

			return string.Concat(lineStart, lineEnd);
		}
	}
}
