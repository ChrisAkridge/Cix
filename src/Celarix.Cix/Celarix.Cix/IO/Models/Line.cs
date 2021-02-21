using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Celarix.Cix.Compiler.Exceptions;

namespace Celarix.Cix.Compiler.IO.Models
{
	internal sealed class Line
    {
        private List<Range> stringLiteralLocations;
        
		public string Text { get; internal set; }
		public string SourceFilePath { get; init; }
        public string SourceFileName => Path.GetFileName(SourceFilePath);
		public int FileLineNumber { get; init; }
		public int OverallLineNumber { get; internal set; }
		public int FileStartCharacterIndex { get; init; }
        public int FileEndCharacterIndex => FileStartCharacterIndex + (Text?.Length ?? 0);
		public int OverallStartCharacterIndex { get; internal set; }
        public int OverallEndCharacterIndex => OverallStartCharacterIndex + (Text?.Length ?? 0);

        public IReadOnlyList<Range> StringLiteralLocations
        {
            get => stringLiteralLocations.AsReadOnly();
            set => stringLiteralLocations = new List<Range>(value);
        }

        public static Line WithText(Line original, string newText) =>
            new Line
            {
                Text = newText,
                SourceFilePath = original.SourceFilePath,
                FileLineNumber = original.FileLineNumber,
                OverallLineNumber = original.OverallLineNumber,
                FileStartCharacterIndex = original.FileStartCharacterIndex,
                OverallStartCharacterIndex = original.OverallStartCharacterIndex,
                stringLiteralLocations = original.stringLiteralLocations    /* oh god this works */
            };

        public void ReplaceWord(int oldWordPosition, string newWordText)
        {
            var textBeforeNewWord = Text.Substring(0, oldWordPosition);
            var textAfterNewWord = Text.Substring(oldWordPosition + newWordText.Length);
            var newText = textBeforeNewWord + newWordText + textAfterNewWord;

            if (newText.Length != Text.Length)
            {
                throw new ErrorFoundException(ErrorSource.InternalCompilerError,
                    1,
                    $"Internal compiler error: rewrite phase changed length of source file, desynchronizing all future errors.",
                    this,
                    oldWordPosition);
            }

            Text = newText;
        }
    }
}
