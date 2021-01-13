using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cix.Errors
{
    public sealed class CharError : Error
    {
        public string FilePath { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        
        public CharError(ErrorSource source, int errorNumber, string message, string filePath, int startIndex, int endIndex) : base(source, errorNumber, message)
        {
            FilePath = filePath;
            StartIndex = startIndex;
            EndIndex = endIndex;
        }

        public override string ToString() =>
            $"{GetErrorSourceAbbreviation(Source)}{ErrorNumber:D3}: {Message} ({Path.GetFileName(FilePath)}:{StartIndex + 1}-{EndIndex + 1})";
    }
}
