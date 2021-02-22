using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Celarix.Cix.Compiler.Exceptions;
using Celarix.Cix.Compiler.Extensions;
using Celarix.Cix.Compiler.IO.Models;
using NLog;

namespace Celarix.Cix.Compiler.Preparse
{
	/// <summary>
	/// Performs preprocessing on a single Cix file, including token substitution and loading of
	/// included files.
	/// </summary>
	internal sealed class Preprocessor
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        
		private readonly List<Line> file;                                   // the contents of the file itself
		private readonly string basePath;                                   // the path to the directory holding the file
		private readonly List<string> definedConstants;                     // a list of all defined constants (i.e. #define __SOME_FILE__)
		private readonly Dictionary<string, string> definedSubstitutions;   // a list of all defined substitutions (i.e. #define THIS THAT)
		private readonly List<string> includedFilePaths;                    // paths to all files included by #include

        /// <summary>
        /// Initializes a new instance of the <see cref="Preprocessor"/> class.
        /// </summary>
        /// <param name="file">The text of the Cix file to preprocess.</param>
        /// <param name="declaredSymbols">Symbols declared by the invoker, typically from command-line options.</param>
        public Preprocessor(IList<Line> file, IList<string> declaredSymbols)
		{
			this.file = file?.ToList();

            if (this.file == null)
            {
                throw new ArgumentNullException(nameof(file), $"Cannot preprocess a null file.");
            }
            
			basePath = (this.file.Any())
                ? Path.GetDirectoryName(this.file.First().SourceFilePath)
                : null;
            definedConstants = declaredSymbols.ToList();
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
            logger.Trace($"Starting preprocessing on \"{file.First().SourceFileName}\"...");
            
			var preprocessedFile = new List<Line>();
			var conditionalValue = ConditionalInclustionState.NotInConditional; // we evaluate the condition as soon as we find it; this field holds the result

			// Every preprocessor directive is guaranteed to be on one line, so we can only look at the lines instead of lexing it.

			foreach (var line in file)
			{
				string trimmedLine = line.Text.Trim();
				if (trimmedLine.ToLowerInvariant().StartsWith("#else", StringComparison.Ordinal))
				{
					conditionalValue = ConditionalInclustionState.ConditionalTrue;
				}
				else if (trimmedLine.ToLowerInvariant().StartsWith("#endif", StringComparison.Ordinal))
				{
					conditionalValue = ConditionalInclustionState.NotInConditional;
				}

				if (conditionalValue == ConditionalInclustionState.ConditionalFalse)
				{
					continue;
				}

				if (trimmedLine.StartsWith("#", StringComparison.Ordinal))
				{
					// This line is a preprocessor directive.

					if (trimmedLine.StartsWith("#define", StringComparison.Ordinal))
					{
						// #define <identifier>: Defines a constant named <identifier>.
						// If <identifier> is used in an #ifdef, it will evaluate to true. 
						// If <identifier> is used in an #ifndef, it will evaluate to false.

						// #define <substitution> <substitute>: Defines two values, the first of which is an identifier, the second of which is an identifier or a number.
						// All instances of <substitution> will be replaced with <substitute> AFTER the preprocessor has processed all lines.

						var words = trimmedLine.Split(' ');
						if (words.Length < 3)
						{
							throw new ErrorFoundException(ErrorSource.Preprocessor, 1,
                                $"{trimmedLine} isn't valid; must be \"#define SYMBOL\" or \"#define THIS THAT\".",
                                line, 0);
						}
						else
                        {
                            switch (words.Length)
                            {
                                case 2 when words[1].IsIdentifier():
                                    // This is a valid preprocessor constant defintion. Add it to this list of constants.
                                    logger.Trace($"Declared symbol \"{words[1]}\"");
                                    definedConstants.Add(words[1]);

                                    break;
                                case 2:
                                    throw new ErrorFoundException(ErrorSource.Preprocessor, 2,
                                        $"Defined symbol {words[1]} is not an identifier.", line, 0);
                                case 3:
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
                                            throw new ErrorFoundException(ErrorSource.Preprocessor, 3,
                                                $"Symbol {words[1]} is already defined.", line, 0);
                                        }

                                        logger.Trace($"Declared substitution \"{words[1]}\" for \"{words[2]}\"");
                                        definedSubstitutions.Add(words[1], words[2]);
                                    }
                                    else
                                    {
                                        throw new ErrorFoundException(ErrorSource.Preprocessor, 4,
                                            $"Substitution {words[2]} for symbol {words[1]} isn't valid; must be an identifier or integer.",
                                            line, 0);
                                    }

                                    break;
                                }
                            }
                        }
                    }
					else if (trimmedLine.StartsWith("#undefine", StringComparison.Ordinal))
					{
						// #undefine <identifier>: Undefines a defined constant.

						var words = trimmedLine.Split(' ');
						if (words.Length == 1 || words.Length > 2)
						{
							throw new ErrorFoundException(ErrorSource.Preprocessor, 5,
                                $"{trimmedLine} isn't valid; must be \"#undefine SYMBOL\".", line, 0);
						}
						else
						{
							string constantToUndefine = words[1];
							if (!definedConstants.Contains(constantToUndefine))
							{
								if (!definedSubstitutions.ContainsKey(constantToUndefine))
								{
									throw new ErrorFoundException(ErrorSource.Preprocessor, 6,
                                        $"Cannot undefine {words[1]} as it was not previously defined.", line, 0);
								}
                                else
                                {
                                    logger.Trace($"Undefined substitution \"{constantToUndefine}\"");
                                    definedSubstitutions.Remove(constantToUndefine);
                                }
							}
                            else
                            {
                                logger.Trace($"Undefined symbol \"{constantToUndefine}\"");
                                definedConstants.Remove(constantToUndefine);
                            }
						}
					}
					else if (trimmedLine.StartsWith("#ifdef", StringComparison.Ordinal))
                    {
                        // #ifdef <identifier>: If <identifier> is #defined, we continue processing
						// everything between this #ifdef and the next #else/#endif.
						// Otherwise, we ignore every line up to the next #else or #endif.
						var words = line.Text.Split(' ');

                        conditionalValue = words.Length == 1 || words.Length > 2
                            ? throw new ErrorFoundException(ErrorSource.Preprocessor, 7,
                                $"\"{line.Text}\" isn't valid; must be #ifdef SYMBOL", line, 0)
                            : (definedConstants.Contains(words[1]))
                                ? ConditionalInclustionState.ConditionalTrue
                                : ConditionalInclustionState.ConditionalFalse;
                    }
					else if (trimmedLine.StartsWith("#ifndef", StringComparison.Ordinal))
					{
						// #ifndef <identifier>: If <identifier> is not #defined, we continue processing everything between this #ifndef and the next #else/#endif.
						// Otherwise, we ignore every line up to the next #else or #endif.
						var words = line.Text.Split(' ');
						conditionalValue = words.Length != 2
                            ? throw new ErrorFoundException(ErrorSource.Preprocessor, 8,
                                $"\"{line.Text}\" isn't valid; must be #ifndef SYMBOL", line, 0)
                            : (!definedConstants.Contains(words[1]))
                                ? ConditionalInclustionState.ConditionalTrue
                                : ConditionalInclustionState.ConditionalFalse;
                    }
					// We've already taken care of the #else and #endif at the top.
					else if (trimmedLine.StartsWith("#include", StringComparison.Ordinal))
					{
						// #include "file" or #include <file>: Loads and preprocesses a source file in this directory. Substitutes this line for a newline, the file, and another newline.
						var words = line.Text.Split(' ');
						if (words.Length != 2)
						{
							throw new ErrorFoundException(ErrorSource.Preprocessor, 9,
                                $"\"{line.Text}\" isn't valid; must be #include \"file\" or #include <file>", line, 0);
						}
						string fileName = words[1].Substring(1, words[1].Length - 2); // get all the text from after the first char and before the last one
						var includedFile = LoadIncludeFile(line, fileName, definedConstants);
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
					foreach (var (symbol, newValue) in definedSubstitutions.Where(substitution => resultLine.Contains(substitution.Key)))
                    {
                        resultLine = resultLine.Replace(symbol, newValue);
                    }

                    preprocessedFile.Add(new Line
                    {
                        Text = line.Text,
                        SourceFilePath = line.SourceFilePath,
                        FileLineNumber = line.FileLineNumber,
                        FileStartCharacterIndex = line.FileStartCharacterIndex
                    });
				}
			}

            logger.Trace($"Completed preprocessing on \"{file.First().SourceFilePath}\"");
			return preprocessedFile;
		}

        public static void SetOverallLineAndCharacterIndices(IList<Line> lines)
        {
            logger.Trace("Setting overall line and starting character indices...");
            
            int overallLineIndex = 0;
            int overallStartCharacterIndex = 0;

            foreach (var line in lines)
            {
                line.OverallLineNumber = overallLineIndex;
                line.OverallStartCharacterIndex = overallStartCharacterIndex;

                overallLineIndex += 1;
                overallStartCharacterIndex += line.Text.Length;
            }

            logger.Trace("Set overall line and starting character indices.");
        }

		/// <summary>
		/// Loads and preprocesses the text of a Cix file named in an #include statement.
		/// </summary>
		/// <param name="includingLine">The line from the original source file that included this file.</param>
		/// <param name="fileName">The path of the file to load.</param>
		/// <param name="declaredSymbols">Symbols declared by the invoker, typically from command-line options.</param>
		/// <returns>The text of the Cix file at that para, preprocessed.</returns>
		private IList<Line> LoadIncludeFile(Line includingLine, string fileName, IList<string> declaredSymbols)
		{
			// For now, we'll just make #include "file" and #include <file> do the same thing
			// They'll look in the current dir for the file to include
			// Then they'll load and preprocess the file
			// And then return it.

			string includeFilePath = Path.Combine(basePath, fileName);
			if (!File.Exists(includeFilePath))
			{
				throw new ErrorFoundException(ErrorSource.Preprocessor, 10,
                    $"The include file {fileName} doesn't exist or has an invalid path.", includingLine, 0);
			}

			if (includedFilePaths.Contains(includeFilePath))
			{
				throw new ErrorFoundException(ErrorSource.Preprocessor, 15,
                    $"The file {includeFilePath} was already included.",
                    includingLine, 0);
			}

            logger.Trace($"Including \"{includeFilePath}\"...");
			includedFilePaths.Add(includeFilePath);

            var includeFile = IO.IO.SplitFileIntoLines(includeFilePath);

            var filePreprocessor = new Preprocessor(includeFile, declaredSymbols);
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