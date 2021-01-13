using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cix.Text;

namespace Cix.Errors
{
	public interface IErrorListProvider
	{
		void AddLineError(ErrorSource source, int errorNumber, string message, Line line);

		void AddLineError(ErrorSource source, int errorNumber, string message, string filePath,
			int lineNumber);

        void AddCharError(ErrorSource source, int errorNumber, string message, string filePath,
            int startIndex, int endIndex);
    }
}
