using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;	// not as true any more
using Cix.Exceptions;

namespace Cix
{
	/// <summary>
	/// Performs preprocessing on a single Cix file, including token substitution and loading of
	/// included files.
	/// </summary>
    public sealed class Preprocessor
	{
		private readonly string file;										// the contents of the file itself
		private string filePath;											// the full path to the file
		private readonly string basePath;									// the path to the directory holding the file
		private readonly List<string> definedConstants;						// a list of all defined constants (i.e. #define __SOME_FILE__)
		private readonly Dictionary<string, string> definedSubstitutions;	// a list of all defined substitutions (i.e. #define THIS THAT)
		private List<string> includedFilePaths;								// paths to all files included by #include

		/// <summary>
		/// Initializes a new instance of the <see cref="Preprocessor"/> class.
		/// </summary>
		/// <param name="file">The text of the Cix file to preprocess.</param>
		/// <param name="filePath">The path of the Cix file to preprocess, used to load #includes.</param>
		public Preprocessor(string file, string filePath)
		{
			this.file = file;
			this.filePath = filePath;
			basePath = Path.GetDirectoryName(this.filePath);
			definedConstants = new List<string>();
			definedSubstitutions = new Dictionary<string, string>();
			includedFilePaths = new List<string>();
		}

		/// <summary>
		/// Performs preprocessing on the provided Cix file.
		/// </summary>
		/// <returns>
		/// A string containing the preprocessed file, including any token substitutions, code
		/// that was conditionally included, and any #included files.
		/// </returns>
		public string Preprocess()
		{
			var resultBuilder = new StringBuilder();							// we'll append every preprocessed line to this builder
			var conditionalValue = ConditionalInclustionState.NotInConditional;	// we evaluate the condition as soon as we find it; this field holds the result

			// First, grab the lines of the file.
			// Every preprocessor directive is guaranteed to be on one line, so we can only look at the lines instead of lexing it.
			string[] fileLines = file.Split(new[] { "\r", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

			foreach (string line in fileLines)
			{
				string trimmedLine = line.Trim();
				if (trimmedLine.StartsWith("#else"))
				{
					conditionalValue = ConditionalInclustionState.ConditionalTrue;
				}
				else if (trimmedLine.StartsWith("#endif"))
				{
					conditionalValue = ConditionalInclustionState.NotInConditional;
				}

				if (conditionalValue == ConditionalInclustionState.ConditionalFalse)
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

						string[] words = trimmedLine.Split(' ');
						if (words.Length == 1 || words.Length > 3)
						{
							throw new PreprocessingException(
								$"Invalid number of words in #define directive. Found {words.Length} words, expected two or three.");
						}
						else if (words.Length == 2)
						{
							if (words[1].IsIdentifier())
							{
								// This is a valid preprocessor constant defintion. Add it to this list of constants.
								definedConstants.Add(words[1]);
							}
							else
							{
								throw new PreprocessingException(
									$"Invalid preprocessor constant {words[1]}. Constant must be an identifier.");
							}
						}
						else if (words.Length == 3)
						{
							if (words[1].IsIdentifier() && (words[2].IsIdentifier() || Regex.IsMatch(words[2], @"\d")))
							{
								// The first word must be a valid identifer. The second word must be a valid identifier OR composed only of digits.
								// Credit to http://stackoverflow.com/a/894567 for the Regex solution.

								if (definedSubstitutions.ContainsKey(words[1]))
								{
									throw new PreprocessingException(
										$"The substitution word {words[1]} may not be defined multiple times.");
								}

								definedSubstitutions.Add(words[1], words[2]);
							}
							else
							{
								throw new PreprocessingException(
									$"Invalid identifiers in substitution definition. Found: {words[1]} and {words[2]}.");
							}
						}
					}
					else if (trimmedLine.StartsWith("#undefine"))
					{
						// #undefine <identifier>: Undefines a defined constant.
						// TODO: Make #undefine support undefining substitutions

						string[] words = line.Split(' ');
						if (words.Length == 1 || words.Length > 2)
						{
							throw new PreprocessingException(
								$"Invalid number of words in undefintion directive. Found {words.Length} words, expected two.");
						}
						else
						{
							string constantToUndefine = words[1];
							if (!definedConstants.Contains(constantToUndefine))
							{
								throw new PreprocessingException(
									$"Could not undefine {constantToUndefine}, it was never defined to begin with.");
							}
							definedConstants.Remove(constantToUndefine);
						}
					}
					else if (trimmedLine.StartsWith("#ifdef"))
					{
						// #ifdef <identifier>: If <identifier> is #defined, we continue processing everything between this #ifdef and the next #else/#endif.
						// Otherwise, we ignore every line up to the next #else or #endif.
						string[] words = line.Split(' ');
						if (words.Length == 1 || words.Length > 2)
						{
							throw new PreprocessingException(
								$"Invalid number of words in conditional. Found {words.Length} words, expected two.");
						}
						else
						{
							conditionalValue = (definedConstants.Contains(words[1]))
								? ConditionalInclustionState.ConditionalTrue : ConditionalInclustionState.ConditionalFalse;
						}
					}
					else if (trimmedLine.StartsWith("#ifndef"))
					{
						// #ifndef <identifier>: If <identifier> is not #defined, we continue processing everything between this #ifndef and the next #else/#endif.
						// Otherwise, we ignore every line up to the next #else or #endif.
						string[] words = line.Split(' ');
						if (words.Length != 2)
						{
							throw new PreprocessingException(
								$"Invalid number of words in conditional. Found {words.Length} words, expected two.");
						}
						else
						{
							conditionalValue = (!definedConstants.Contains(words[1]))
								? ConditionalInclustionState.ConditionalTrue : ConditionalInclustionState.ConditionalFalse;
						}
					}
					// We've already taken care of the #else and #endif at the top.
					else if (trimmedLine.StartsWith("#include"))
					{
						// #include "file" or #include <file>: Loads and preprocesses a source file in this directory. Substitutes this line for a newline, the file, and another newline.
						string[] words = line.Split(' ');
						if (words.Length != 2)
						{
							throw new PreprocessingException(
								$"Invalid number of words in include statement. Found {words.Length} words, expected two.");
						}
						string fileName = words[1].Substring(1, words[1].Length - 2); // get all the text from after the first char and before the last one
						string includedFile = LoadIncludeFile(fileName);
						resultBuilder.Append(includedFile);
					}
				}
				else
				{
					// This line is a normal line. We'll include it if conditionalValue is true
					// (meaning the #ifdef/#ifndef we're in is true) or null (if we're not in an #ifdef/#ifndef at all).
					// If conditional value is false, we won't include it.

					if (conditionalValue == ConditionalInclustionState.ConditionalFalse)
					{
						continue;
					}

					string resultLine = line;
					foreach (KeyValuePair<string, string> substitution in definedSubstitutions)
					{
						if (resultLine.Contains(substitution.Key))
						{
							resultLine = resultLine.Replace(substitution.Key, substitution.Value);
						}
					}

					resultBuilder.Append(resultLine);
				}
			}

			return resultBuilder.ToString();
		}

		/// <summary>
		/// Loads and preprocesses the text of a Cix file named in an #include statement.
		/// </summary>
		/// <param name="fileName">The path of the file to load.</param>
		/// <returns>The text of the Cix file at that para, preprocessed.</returns>
		public string LoadIncludeFile(string fileName)
		{
			// For now, we'll just make #include "file" and #include <file> do the same thing
			// They'll look in the current dir for the file to include
			// Then they'll load and preprocess the file
			// And then return it.

			// TODO: ensure there are no cycles in the inclusion tree

			string includeFilePath = Path.Combine(basePath, fileName);
			if (!File.Exists(includeFilePath))
			{
				throw new PreprocessingException($"The include file at {includeFilePath} does not exist.");
			}

			includedFilePaths.Add(includeFilePath);

			string includeFile = File.ReadAllText(includeFilePath).RemoveComments();
			Preprocessor filePreprocessor = new Preprocessor(includeFile, includeFilePath);
			includeFile = filePreprocessor.Preprocess();

			return string.Concat(Environment.NewLine, includeFile, Environment.NewLine);
		}
	}

	public enum ConditionalInclustionState
	{
		NotInConditional,
		ConditionalTrue,
		ConditionalFalse
	}
}