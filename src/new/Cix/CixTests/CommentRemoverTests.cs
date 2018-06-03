using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cix;
using Cix.Errors;
using Cix.Extensions;
using Cix.Text;

namespace CixTests
{
	[TestClass]
    public class CommentRemoverTests
    {
	    [TestMethod]
	    public void RemoveComments()
	    {
		    string originalFilePath = Utilities.GetTestCaseFilePath("RemoveCommentTests");
		    string expectedFilePath = Utilities.GetTestCaseFilePath("RemoveCommentTestsAfter");

		    string originalFileText = Utilities.GetTestCaseFileText("RemoveCommentTests");
		    string expectedFileText = Utilities.GetTestCaseFileText("RemoveCommentTestsAfter");

		    IEnumerable<Line> originalLines =
			    LineSplitter.SplitFileTextIntoLines(originalFileText, originalFilePath);
		    IEnumerable<Line> withoutComments = CommentRemover.RemoveComments(originalLines);

			Assert.AreEqual(expectedFileText, withoutComments.LinesToString());
	    }

	    [TestMethod]
	    public void ErrorWhenSingleSlashEndsLine()
	    {
		    string original = "invalid comment /";
		    var lines = new[] {new Line(original, "file.cix", 1)};
		    IEnumerable<Line> withoutComments = CommentRemover.RemoveComments(lines);

			Assert.IsTrue(ErrorContext.Errors.Count == 1);
		    Assert.AreEqual(ErrorSource.CommentRemover, ErrorContext.Errors[0].Source);
			Assert.AreEqual(1, ErrorContext.Errors[0].Number);

		    ErrorContext.ClearErrors();
	    }

	    [TestMethod]
	    public void ErrorWhenClosingMultilineCommentHasNoOpening()
	    {
		    string original = "invalid comment */ see";
		    var lines = new[] { new Line(original, "file.cix", 1) };
		    IEnumerable<Line> withoutComments = CommentRemover.RemoveComments(lines);

		    Assert.AreEqual(1, ErrorContext.Errors.Count);
		    Assert.AreEqual(ErrorSource.CommentRemover, ErrorContext.Errors[0].Source);
		    Assert.AreEqual(2, ErrorContext.Errors[0].Number);

		    ErrorContext.ClearErrors();
	    }
	}
}
