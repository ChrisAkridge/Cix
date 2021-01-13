using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Cix.Errors;
using Cix.Text;

namespace Cix.Parser
{
	/// <summary>
	/// Performs preprocessing on a single Cix file, including token substitution and loading of
	/// included files.
	/// </summary>
    internal sealed class Preprocessor
	{
		private readonly IErrorListProvider errorList;

		private readonly List<Line> file;									// the contents of the file itself
		private readonly string filePath;									// the full path to the file
		private readonly string basePath;									// the path to the directory holding the file
		private readonly List<string> definedConstants;						// a list of all defined constants (i.e. #define __SOME_FILE__)
		private readonly Dictionary<string, string> definedSubstitutions;	// a list of all defined substitutions (i.e. #define THIS THAT)
		private readonly List<string> includedFilePaths;					// paths to all files included by #include

		/// <summary>
		/// Initializes a new instance of the <see cref="Preprocessor"/> class.
		/// </summary>
		/// <param name="errorList">An interface representing an error list that this class can add errors to.</param>
		/// <param name="file">The text of the Cix file to preprocess.</param>
		/// <param name="filePath">The path of the Cix file to preprocess, used to load #includes.</param>
		public Preprocessor(IErrorListProvider errorList, IEnumerable<Line> file, string filePath)
		{
			this.errorList = errorList;
			this.file = file.ToList();
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
		public IList<Line> Preprocess()
		{
			var preprocessedFile = new List<Line>();
			var conditionalValue = ConditionalInclustionState.NotInConditional;	// we evaluate the condition as soon as we find it; this field holds the result

			// Every preprocessor directive is guaranteed to be on one line, so we can only look at the lines instead of lexing it.

			foreach (Line line in file)
			{
				string trimmedLine = line.Text.Trim();
				if (trimmedLine.ToLowerInvariant().StartsWith("#else"))
				{
					conditionalValue = ConditionalInclustionState.ConditionalTrue;
				}
				else if (trimmedLine.ToLowerInvariant().StartsWith("#endif"))
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
							errorList.AddError(ErrorSource.Preprocessor, 1,
								$"{trimmedLine} isn't valid; must be \"#define SYMBOL\" or \"#define THIS THAT\".", line);
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
								errorList.AddError(ErrorSource.Preprocessor, 2,
									$"Defined symbol {words[1]} is not an identifier.", line);
							}
						}
						else if (words.Length == 3)
						{
							// Matches either (at string start, 0x then one or more chars in 0-9,
							// A-F, and a-f) or (at string start, one or more digits)
							const string numberOrHexNumberRegex = @"^0x[0-9A-Fa-f]+|^\d+";

							if (words[1].IsIdentifier() && (words[2].IsIdentifier() || Regex.IsMatch(words[2], numberOrHexNumberRegex)))
							{
								// The first word must be a valid identifer. The second word must be a valid identifier OR composed only of digits.
								// Credit to http://stackoverflow.com/a/894567 for the Regex solution.

								if (definedSubstitutions.ContainsKey(words[1]))
								{
									errorList.AddError(ErrorSource.Preprocessor, 3,
										$"Symbol {words[1]} is already defined.", line);
								}

								definedSubstitutions.Add(words[1], words[2]);
							}
							else
							{
								errorList.AddError(ErrorSource.Preprocessor, 4,
									$"Substitution {words[2]} for symbol {words[1]} isn't valid; must be an identifier or integer.", line);
							}
						}
					}
					else if (trimmedLine.StartsWith("#undefine"))
					{
						// #undefine <identifier>: Undefines a defined constant.
						// TODO: Make #undefine support undefining substitutions

						string[] words = trimmedLine.Split(' ');
						if (words.Length == 1 || words.Length > 2)
						{
							errorList.AddError(ErrorSource.Preprocessor, 5,
								$"{trimmedLine} isn't valid; must be \"#undefine SYMBOL\".", line);
						}
						else
						{
							string constantToUndefine = words[1];
							if (!definedConstants.Contains(constantToUndefine))
							{
								if (!definedSubstitutions.ContainsKey(constantToUndefine))
								{
									errorList.AddError(ErrorSource.Preprocessor, 6,
										$"Cannot undefine {words[1]} as it was not previously defined.", line);
								}
								else { definedSubstitutions.Remove(constantToUndefine); }
							}
							else { definedConstants.Remove(constantToUndefine); }
						}
					}
					else if (trimmedLine.StartsWith("#ifdef"))
					{
						// #ifdef <identifier>: If <identifier> is #defined, we continue processing
						// everything between this #ifdef and the next #else/#endif.
						// Otherwise, we ignore every line up to the next #else or #endif.
						string[] words = line.Text.Split(' ');
						if (words.Length == 1 || words.Length > 2)
						{
							errorList.AddError(ErrorSource.Preprocessor, 7,
								$"\"{line.Text}\" isn't valid; must be #ifdef SYMBOL", line);
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
						string[] words = line.Text.Split(' ');
						if (words.Length != 2)
						{
							errorList.AddError(ErrorSource.Preprocessor, 8,
								$"\"{line.Text}\" isn't valid; must be #ifndef SYMBOL", line);
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
						string[] words = line.Text.Split(' ');
						if (words.Length != 2)
						{
							errorList.AddError(ErrorSource.Preprocessor, 9,
								$"\"{line.Text}\" isn't valid; must be #include \"file\" or #include <file>", line);
							continue;
						}
						string fileName = words[1].Substring(1, words[1].Length - 2); // get all the text from after the first char and before the last one
						IEnumerable<Line> includedFile = LoadIncludeFile(line, fileName);
						preprocessedFile.AddRange(includedFile);
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

					string resultLine = line.Text;
					foreach (KeyValuePair<string, string> substitution in definedSubstitutions)
					{
						if (resultLine.Contains(substitution.Key))
						{
							resultLine = resultLine.Replace(substitution.Key, substitution.Value);
						}
					}

					preprocessedFile.Add(new Line(line.FilePath, line.LineNumber, resultLine));
				}
			}

			return preprocessedFile;
		}

		/// <summary>
		/// Loads and preprocesses the text of a Cix file named in an #include statement.
		/// </summary>
		/// <param name="includingLine">The line from the original source file that included this file.</param>
		/// <param name="fileName">The path of the file to load.</param>
		/// <returns>The text of the Cix file at that para, preprocessed.</returns>
		public IEnumerable<Line> LoadIncludeFile(Line includingLine, string fileName)
		{
			// For now, we'll just make #include "file" and #include <file> do the same thing
			// They'll look in the current dir for the file to include
			// Then they'll load and preprocess the file
			// And then return it.

			// TODO: ensure there are no cycles in the inclusion tree

			string includeFilePath = Path.Combine(basePath, fileName);
			if (!File.Exists(includeFilePath))
			{
				errorList.AddError(ErrorSource.Preprocessor, 10,
					$"The include file {fileName} doesn't exist or has an invalid path.", includingLine);
				return Enumerable.Empty<Line>();
			}

			includedFilePaths.Add(includeFilePath);
			
			var io = new IO(errorList);
			string includeFileText = io.LoadInputFile(includeFilePath);
			IList<Line> includeFile = io.SplitFileByLine(includeFilePath, includeFileText);

			var commentRemover = new CommentRemover(errorList);
			IList<Line> fileWithoutComments = commentRemover.RemoveComments(includeFile);

			Preprocessor filePreprocessor = new Preprocessor(errorList, fileWithoutComments, includeFilePath);
			return filePreprocessor.Preprocess();
		}
	}

	public enum ConditionalInclustionState
	{
		NotInConditional,
		ConditionalTrue,
		ConditionalFalse
	}
}