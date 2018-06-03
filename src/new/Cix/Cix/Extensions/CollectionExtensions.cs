using System;
using System.Collections.Generic;
using System.Text;
using Cix.Text;

namespace Cix.Extensions
{
    public static class CollectionExtensions
    {
	    public static string LinesToString(this IEnumerable<Line> lines)
	    {
		    var resultBuilder = new StringBuilder();

		    foreach (Line line in lines) { resultBuilder.AppendLine(line.Text); }
		    return resultBuilder.ToString();
	    }
    }
}
