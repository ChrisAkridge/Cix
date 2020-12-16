using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Antlr4.Runtime;
using CixAntlrTest.Cix;

namespace CixAntlrTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = args[0];
            var fileLines = new CommentRemover().RemoveComments(File.ReadAllLines(filePath));
            var fileText = string.Join(Environment.NewLine, fileLines);
            File.WriteAllText(Path.Combine(Path.GetDirectoryName(filePath), "processed.cix"), fileText);
            
            var lexer = new CixLexer(new AntlrInputStream(fileText));
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new CixParser(tokenStream);

            var sourceFile = parser.sourceFile();
            var functions = sourceFile.function();
        }

        private static string RemoveComments(string file)
        {
            bool[] removeCharacterAtIndex = new bool[file.Length];
            bool inSinglestringComment = false;
            bool inMultistringComment = false;
            
            for (int i = 0; i < file.Length; i++)
            {
                if (file[i] == '/')
                {
                    if (file[i + 1] == '/') { inSinglestringComment = true; }
                    else if (file[i + 1] == '*') { inMultistringComment = true;}
                }
                else if (file[i] == '\r')
                {
                    inSinglestringComment = false;
                }
                else if (file[i - 1] == '/' && file[i - 2] == '*')
                {
                    inMultistringComment = false;
                }

                if (inSinglestringComment || inMultistringComment) { removeCharacterAtIndex[i] = true; }
            }
            
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < file.Length; i++)
            {
                if (!removeCharacterAtIndex[i])
                {
                    builder.Append(file[i]);
                }
            }

            return builder.ToString();
        }
    }

	internal sealed class CommentRemover
	{
		public IList<string> RemoveComments(IList<string> file)
		{
			var fileWithoutComments = new List<string>();
			var inMultilineComment = false;

			foreach (string line in file)
			{
				if (line.Contains("//"))
				{
					string lineWithoutComment = RemoveCommentFromEndOfLine(line, "//");

					if (!string.IsNullOrEmpty(lineWithoutComment))
					{
						fileWithoutComments.Add(lineWithoutComment);
					}
				}
				else if (line.Contains("/*") && !inMultilineComment)
				{
					if (!line.Contains("*/"))
					{
						inMultilineComment = true;
					}
					else
					{
						inMultilineComment = false;
						fileWithoutComments.Add(RemoveCommentFromMiddleOfLine(line));
					}
				}
				else if (inMultilineComment)
				{
					if (line.Contains("*/"))
					{
						inMultilineComment = false;
						fileWithoutComments.Add(RemoveCommentFromStartOfLine(line));
					}
				}
				else
				{ fileWithoutComments.Add(line); }
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

    namespace Cix
    {
	    public static class StringExtensions
        {
            public static string Substring(this string s, int startIndex, int endIndex)
		    {
			    int length = endIndex - startIndex;
			    return s.Substring(startIndex, length);
		    }

		    private enum CommentKind
		    {
			    NoComment,
			    SingleLine,
			    MultipleLines
		    }
	    }
    }

}
