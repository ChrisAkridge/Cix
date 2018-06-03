using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cix.Text;

namespace CixTests
{
	[TestClass]
    public class LineSplitterTests
	{
		private const string TestCaseName = "LineSplitterTests";

		[TestMethod]
		public void LineSplitterSplitsLine()
		{
			string filePath = Utilities.GetTestCaseFilePath(TestCaseName);
			string fileText = Utilities.GetTestCaseFileText(TestCaseName);

			List<Line> lines = LineSplitter.SplitFileTextIntoLines(fileText, filePath).ToList();

			Assert.AreEqual(6, lines.Count);
			Assert.AreEqual("This is the first line", lines[0].Text);
			Assert.AreEqual(100, lines[1].Segments[0].EndColumn);
			Assert.AreEqual(3, lines[2].LineNumber);
			Assert.AreEqual("", lines[3].Text);
			Assert.AreEqual(filePath, lines[4].FilePath);
		}

		[TestMethod]
		public void LineToStringTests()
		{
			var line = new Line("Hello, world!", "", 1);
			Assert.AreEqual("Hello, world!", line.ToString());
		}
	}
}
