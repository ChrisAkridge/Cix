using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix
{
	public static class StringExtensions
	{
		public static bool IsOneOfString(this string check, params string[] values)
		{
			return new HashSet<string>(values).Contains(check);
		}
	}
}
