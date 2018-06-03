using System;
using System.Collections.Generic;
using System.Text;

namespace Cix.Errors
{
    internal static class ErrorList
    {
	    public static readonly Error IO_NullOrEmptyFilePath =
		    new Error(ErrorSource.IO, 1, "Null or empty file path.");
    }
}
