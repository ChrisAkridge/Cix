using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cix.Text;

namespace Cix.Errors
{
    public sealed class LineError : Error
    {
        public string FilePath { get; }
        public int LineNumber { get; }

        public LineError(ErrorSource source, int errorNumber, string message, Line line) : base(source, errorNumber, message)
        {
            FilePath = line.FilePath;
            LineNumber = line.LineNumber;
        }

        public LineError(ErrorSource source, int errorNumber, string message, string filePath, int lineNumber) : base(source, errorNumber, message)
        {
            FilePath = filePath;
            LineNumber = lineNumber;
        }

        public override string ToString() =>
            $"{GetErrorSourceAbbreviation(Source)}{ErrorNumber:D3}: {Message} ({Path.GetFileName(FilePath)}:{LineNumber + 1})";
    }
}
