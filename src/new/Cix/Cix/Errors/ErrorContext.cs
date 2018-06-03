using System;
using System.Collections.Generic;
using System.Text;

namespace Cix.Errors
{
    public static class ErrorContext
    {
	    private static readonly List<Error> errors = new List<Error>();

	    public static IReadOnlyList<Error> Errors = errors.AsReadOnly();

	    public static void AddError(Error error) => errors.Add(error);

	    public static void AddError(ErrorSource source, int number, string message,
			string sourceFileName, int lineNumber, int columnNumber) 
		    => AddError(new Error(source, number, message, sourceFileName, lineNumber, columnNumber));

	    public static void ClearErrors() => errors.Clear();
    }
}
