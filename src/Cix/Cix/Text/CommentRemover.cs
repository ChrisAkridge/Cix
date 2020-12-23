using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cix.Errors;

namespace Cix.Text
{
	internal sealed class CommentRemover
	{
        private enum CommentRemoverStates
        {
            Default,
            StringLiteral,
            SingleLineComment,
            MultilineComment
        }
        
		private readonly IErrorListProvider errorList;

		public CommentRemover(IErrorListProvider errorList) => this.errorList = errorList;

		public string RemoveComments(string filePath, string fileText)
		{
			// States:
            //	- Default
            //	- StringLiteral
            //	- SingleLineComment
            //	- MultilineComment
            // State transitions:
            //	Default => " => StringLiteral
            //	Default => \" => (error, escaped double quote outside of literal)
            //	Default => // => SingleLineComment
            //	Default => /* => MultilineComment
            //	Default => */ => (error, multiline comment end without start)
            //	StringLiteral => " => Default
            //	SingleLineComment => \r\n, \r, or \n => Default
            //	MultilineComment => */ => Default
            
            var spansToRemove = new List<SpanIndices>();
            int currentSpanStart = -1;
            var state = CommentRemoverStates.Default;

            for (var i = 0; i < fileText.Length; i++)
            {
                var current = fileText[i];
                var prev = (i > 0) ? fileText[i - 1] : (char?)null;
                var next = (i < fileText.Length) ? fileText[i + 1] : (char?)null;

                switch (current)
                {
                    case '\"' when prev != '\\':
                        continue;
                    case '\"' when state == CommentRemoverStates.Default:
                        errorList.AddCharError(ErrorSource.CommentRemover, 1, "Escaped double quote outside of string literal.", filePath, i, i + 1);

                        continue;
                    case '\"':
                        state = CommentRemoverStates.StringLiteral;

                        break;
                    case '/' when state == CommentRemoverStates.Default:
                    {
                        if (next != '/') { continue; }

                        state = CommentRemoverStates.SingleLineComment;
                        currentSpanStart = i;

                        break;
                    }
                    case '/' when next == '*' && state == CommentRemoverStates.Default:
                        state = CommentRemoverStates.MultilineComment;
                        currentSpanStart = i;

                        break;
                    case '*' when next == '/' && state == CommentRemoverStates.MultilineComment:
                        state = CommentRemoverStates.Default;
                        spansToRemove.Add(new SpanIndices(currentSpanStart, i + 1));

                        break;
                    case '\r' when state == CommentRemoverStates.SingleLineComment:
                        state = CommentRemoverStates.Default;
                        spansToRemove.Add(new SpanIndices(currentSpanStart, i));

                        break;
                }
            }

            return RemoveSpans(fileText, spansToRemove);
        }

        private static string RemoveSpans(string text, IList<SpanIndices> spansToRemove)
        {
            if (!spansToRemove.Any()) { return text; }
            
            var builder = new StringBuilder(text.Length);
            var removedChars = new BitArray(text.Length);

            foreach (var span in spansToRemove)
            {
                for (var i = span.Start; i <= span.End; i++)
                {
                    removedChars[i] = true;
                }
            }

            for (var i = 0; i < text.Length; i++)
            {
                if (!removedChars[i])
                {
                    builder.Append(text[i]);
                }
            }

            return builder.ToString();
        }
	}
}
