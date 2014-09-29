using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cix
{
	public sealed class Preprocessor
	{
		private string file;										// the contents of the file itself
		private string filePath;									// the full path to the file
		private string basePath;									// the path to the directory holding the file
		private List<string> definedConstants;						// a list of all defined constants (i.e. #define __SOME_FILE__)
		private Dictionary<string, string> definedSubstitutions;	// a list of all defined substitutions (i.e. #define THIS THAT)
		private List<string> includedFilePaths;						// paths to all files included by #include

		public Preprocessor(string file, string filePath)
		{
			if (!File.Exists(filePath))
			{
				throw new FileNotFoundException(string.Format("The file at {0} does not exist.", filePath));
			}

			this.file = file;
			this.filePath = filePath;
			this.basePath = Path.GetDirectoryName(this.filePath);
			this.definedConstants = new List<string>();
			this.definedSubstitutions = new Dictionary<string, string>();
			this.includedFilePaths = new List<string>();
		}

		public string Preprocess()
		{
			StringBuilder resultBuilder = new StringBuilder();	// we'll append every preprocessed line to this builder
			bool withinConditional = false;						// set when we find a #ifdef or #ifndef directive; cleared when we find an #endif directive
			bool conditionalValue = false;						// we evaluate the condition as soon as we find it; this field holds the result

			// First, grab the lines of the file. 
			// Every preprocessor directive is guaranteed to be on one line, so we can only look at the lines instead of lexing it.
			string[] fileLines = this.file.Split(new string[] { "\r", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

			foreach (string line in fileLines)
			{
				if (line.StartsWith("#"))
				{
					
				}
				else
				{

				}
			}

			return null;
		}
	}
}