using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CixTests
{
	internal static class Utilities
	{
		private const string TestCasePath = @"../../TestCases/";

		public static string GetTestCasePath(string caseName)
		{
			string path = Path.Combine(TestCasePath, caseName + ".cix");
			return path;
		}
	}
}
