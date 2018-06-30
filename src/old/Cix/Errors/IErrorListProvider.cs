using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cix.Text;

namespace Cix.Errors
{
	internal interface IErrorListProvider
	{
		void AddError(ErrorSource source, int errorNumber, string message, Line line);
	}
}
