using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions; // wow I don't use this one often
using Cix.Exceptions;

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
			bool? conditionalValue = null;						// we evaluate the condition as soon as we find it; this field holds the result

			// First, grab the lines of the file. 
			// Every preprocessor directive is guaranteed to be on one line, so we can only look at the lines instead of lexing it.
			string[] fileLines = this.file.Split(new string[] { "\r", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

			foreach (string line in fileLines)
			{
				string trimmedLine = line.Trim();
				if (trimmedLine.StartsWith("#else"))
				{
					conditionalValue = true;
				}
				else if (trimmedLine.StartsWith("#endif"))
				{
					conditionalValue = null;
				}

				if (conditionalValue.HasValue && conditionalValue.Value == false)
				{
					continue;
				}

				if (trimmedLine.StartsWith("#"))
				{
					// This line is a preprocessor directive.

					if (trimmedLine.StartsWith("#define"))
					{
						// #define <identifier>: Defines a constant named <identifier>.
						// If <identifier> is used in an #ifdef, it will evaluate to true. 
						// If <identifier> is used in an #ifndef, it will evaluate to false.

						// #define <substitution> <substitute>: Defines two values, the first of which is an identifier, the second of which is an identifier or a number.
						// All instances of <substitution> will be replaced with <substitute> AFTER the preprocessor has processed all lines.

						string[] words = line.Split(' ');
						if (words.Length == 1 || words.Length > 3)
						{
							throw new PreprocessingException(string.Format("Invalid number of words in #define directive. Found {0} words, expected two or three.", words.Length));
						}
						else if (words.Length == 2)
						{
							if (words[1].IsIdentifier())
							{
								// This is a valid preprocessor constant defintion. Add it to this list of constants.
								this.definedConstants.Add(words[1]);
							}
							else
							{
								throw new PreprocessingException(string.Format("Invalid preprocessor constant {0}. Constant must be an identifier.", words[1]));
							}
						}
						else if (words.Length == 3)
						{
							if (words[1].IsIdentifier() && (words[2].IsIdentifier() || Regex.IsMatch(words[2], @"\d")))
							{
								// The first word must be a valid identifer. The second word must be a valid identifier OR composed only of digits.
								// Credit to http://stackoverflow.com/a/894567 for the Regex solution.

								if (this.definedSubstitutions.ContainsKey(words[1]))
								{
									throw new PreprocessingException(string.Format("The substitution word {0} may not be defined multiple times.", words[1]));
								}

								this.definedSubstitutions.Add(words[1], words[2]);
							}
							else
							{
								throw new PreprocessingException(string.Format("Invalid identifiers in substitution definition. Found: {0} and {1}.", words[1], words[2]));
							}
						}
					}
					else if (trimmedLine.StartsWith("#undefine"))
					{
						// #undefine <identifier>: Undefines a defined constant.

						string[] words = line.Split(' ');
						if (words.Length == 1 || words.Length > 2)
						{
							throw new PreprocessingException(string.Format("Invalid number of words in undefintion directive. Found {0} words, expected two.", words.Length));
						}
						else
						{
							string constantToUndefine = words[1];
							if (!this.definedConstants.Contains(constantToUndefine))
							{
								throw new PreprocessingException(string.Format("Could not undefine {0}, it was never defined to begin with.", constantToUndefine));
							}
							this.definedConstants.Remove(constantToUndefine);
						}
					}
					else if (trimmedLine.StartsWith("#ifdef"))
					{
						// #ifdef <identifier>: If <identifier> is #defined, we continue processing everything between this #ifdef and the next #else/#endif.
						// Otherwise, we ignore every line up to the next #else or #endif.
						string[] words = line.Split(' ');
						if (words.Length == 1 || words.Length > 2)
						{
							throw new PreprocessingException(string.Format("Invalid number of words in conditional. Found {0} words, expected two.", words.Length));
						}
						else
						{
							conditionalValue = definedConstants.Contains(words[1]);
						}
					}
					else if (trimmedLine.StartsWith("#ifndef"))
					{
						// #ifndef <identifier>: If <identifier> is not #defined, we continue processing everything between this #ifndef and the next #else/#endif.
						// Otherwise, we ignore every line up to the next #else or #endif.
						string[] words = line.Split(' ');
						if (words.Length != 2)
						{
							throw new PreprocessingException(string.Format("Invalid number of words in conditional. Found {0} words, expected two.", words.Length));
						}
						else
						{
							conditionalValue = !definedConstants.Contains(words[1]);
						}
					}
					// We've already taken care of the #else and #endif at the top.
					else if (trimmedLine.StartsWith("#include"))
					{
						// #include "file" or #include <file>: Loads and preprocesses a source file in this directory. Substitutes this line for a newline, the file, and another newline.
						string[] words = line.Split(' ');
						if (words.Length != 2)
						{
							throw new PreprocessingException(string.Format("Invalid number of words in include statement. Found {0} words, expected two.", words.Length));
						}
						string fileName = words[1].Substring(1, words[1].Length - 2); // get all the text from after the first char and before the last one
						string file = this.LoadIncludeFile(fileName);
						resultBuilder.Append(file);
					}
				}
				else
				{
					// This line is a normal line. We'll include it if conditionalValue is true 
					// (meaning the #ifdef/#ifndef we're in is true) or null (if we're not in an #ifdef/#ifndef at all).
					// If conditional value is false, we won't include it.

					if (!conditionalValue.HasValue || conditionalValue.Value)
					{
						string resultLine = line;
						foreach (var substitution in definedSubstitutions)
						{
							if (resultLine.Contains(substitution.Key))
							{
								resultLine = resultLine.Replace(substitution.Key, substitution.Value);
							}
						}

						resultBuilder.Append(resultLine);
					}
				}
			}

			return resultBuilder.ToString();
		}

		public string LoadIncludeFile(string fileName)
		{
			// For now, we'll just make #include "file" and #include <file> do the same thing
			// They'll look in the current dir for the file to include
			// Then they'll load and preprocess the file
			// And then return it.

			string filePath = Path.Combine(this.basePath, fileName);
			if (!File.Exists(filePath))
			{
				throw new PreprocessingException(string.Format("The include file at {0} does not exist.", filePath));
			}

			this.includedFilePaths.Add(filePath);

			string file = File.ReadAllText(filePath);
			Preprocessor filePreprocessor = new Preprocessor(file, filePath);
			file = filePreprocessor.Preprocess();

			return string.Concat(Environment.NewLine, file, Environment.NewLine);
		}
	}
}