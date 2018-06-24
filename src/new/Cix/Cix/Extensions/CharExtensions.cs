using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cix.Extensions
{
	public static class CharExtensions
	{
		public static bool IsOneOfCharacter(this char check, params char[] values) =>
			values.Contains(check);
	}
}
