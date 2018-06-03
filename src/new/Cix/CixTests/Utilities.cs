using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CixTests
{
    internal class Utilities
    {
	    private const string TestCaseFolder = @"..\..\..\..\TestCases\";


	    public static string GetTestCaseFileText(string caseName)
		{
			string path = GetTestCaseFilePath(caseName);
			return File.ReadAllText(path);
		}

		public static string GetTestCaseFilePath(string caseName) =>
			Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), TestCaseFolder, caseName + ".cix"));
    }
}
