using System;
using System.Collections.Generic;
using System.Text;

namespace Cix.Errors
{
    public sealed class Error
    {
	    public ErrorSource Source { get; }
		public int Number { get; }
	    public string Code => Source.ToErrorSourceAbbreviation() + Number.ToString("D3");
	    public string Message { get; }

		public string SourceFileName { get; }
		public int LineNumber { get; }
		public int ColumnNumber { get; }

	    public Error(ErrorSource source, int number, string message, string sourceFileName,
		    int lineNumber, int columnNumber)
	    {
		    Source = source;
		    Number = number;
		    Message = message;
		    SourceFileName = sourceFileName;
		    LineNumber = lineNumber;
		    ColumnNumber = columnNumber;
	    }
    }
}
