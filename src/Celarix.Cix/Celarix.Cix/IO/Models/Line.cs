using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Celarix.Cix.Compiler.IO.Models
{
	public sealed class Line
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
    }
}
