using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix
{
	public static class CharExtensions
	{
		public static bool IsOneOfCharacter(this char check, params char[] values) =>
			new HashSet<char>(values).Contains(check);
	}
}
