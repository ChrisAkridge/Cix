using System;
using System.Collections.Generic;
using System.Text;

namespace Celarix.Cix.Compiler.IO.Models
{
	public sealed class Line
    {
        private List<Range> stringLiteralLocations;
        
		public string Text { get; internal set; }
		public string SourceFilePath { get; init; }
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
    }
}
