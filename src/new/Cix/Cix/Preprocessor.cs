using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Cix.Errors;
using Cix.Extensions;
using Cix.Text;

namespace Cix
{
    public static class Preprocessor
    {
	    public static IEnumerable<Line> Preprocess(IEnumerable<Line> file)
	    {
		    string filePath = file.First().FilePath;
		    string basePath = Path.GetDirectoryName(filePath);
		    var definedConstants = new List<string>();
		    var definedSubstitutions = new Dictionary<string, string>();
		    var includedFilePaths = new List<(string filePath, int includedOnLineNumber)>();
		    var conditionalValue = ConditionalInclusionState.NotInConditional;

		    var outputFile = new List<Line>();

		    foreach (Line line in file.Where(l => !string.IsNullOrEmpty(l.Text)))
		    {
			    string trimmedLine = line.Text.Trim();
			    string[] words = trimmedLine.Split(' ');

				if (trimmedLine.StartsWith("#else"))
			    {
				    if (conditionalValue == ConditionalInclusionState.NotInConditional)
				    {
						ErrorContext.AddError(ErrorSource.Preprocessor, 13, "An #else was found without an #ifdef or an #ifndef.",
							line.FilePath, line.LineNumber, 1);
					    continue;
				    }
				    conditionalValue = ConditionalInclusionState.ConditionalTrue;
			    }
				else if (trimmedLine.StartsWith("#endif"))
			    {
				    if (conditionalValue == ConditionalInclusionState.NotInConditional)
				    {
					    ErrorContext.AddError(ErrorSource.Preprocessor, 14, "An #endif was found without an #ifdef or an #ifndef.",
						    line.FilePath, line.LineNumber, 1);
						continue;
					}
				    conditionalValue = ConditionalInclusionState.NotInConditional;
			    }

			    if (conditionalValue == ConditionalInclusionState.ConditionalFalse) { continue; }

			    if (trimmedLine.StartsWith("#"))
			    {
				    if (trimmedLine.StartsWith("#define"))
				    {
					    if (words.Length == 1 || words.Length > 3)
					    {
						    ErrorContext.AddError(ErrorSource.Preprocessor, 1,
							    $"\"{trimmedLine}\" isn't valid; must be \"#define SYMBOL\" or \"#define THIS THAT\"",
							    line.FilePath, line.LineNumber, 1);
						}
						else if (words.Length == 2)
					    {
						    string symbol = GetSymbolFromDefine(words, line.FilePath, line.LineNumber);
							if (symbol != null) { definedConstants.Add(symbol); }
					    }
						else if (words.Length == 3)
					    {
						    KeyValuePair<string, string>? substitution = GetSubstitutionFromDefine(words,
							    definedSubstitutions.Keys.Concat(definedConstants), line.FilePath, line.LineNumber);
						    if (substitution != null)
						    {
							    definedSubstitutions.Add(substitution.Value.Key, substitution.Value.Value);
						    }
					    }
				    }
			    }
				else if (trimmedLine.StartsWith("#undefine"))
			    {
				    if (words.Length == 1 || words.Length > 2)
				    {
						ErrorContext.AddError(ErrorSource.Preprocessor, 5,
							$"\"{trimmedLine}\" isn't valid; must be \"#undefine SYMBOL\"",
							line.FilePath, line.LineNumber, 1);
					    continue;
				    }
				    else
				    {
					    string symbolToUndefine = words[1];
					    if (definedConstants.Contains(symbolToUndefine))
					    {
						    definedConstants.Remove(symbolToUndefine);
					    }
						else if (definedSubstitutions.ContainsKey(symbolToUndefine))
					    {
						    definedSubstitutions.Remove(symbolToUndefine);
					    }
					    else
					    {
						    ErrorContext.AddError(ErrorSource.Preprocessor, 6,
							    $"Cannot undefine symbol or substitution {symbolToUndefine}, as it was not previously defined.",
							    line.FilePath, line.LineNumber, 1);
						    continue;
					    }
				    }
			    }
				else if (trimmedLine.StartsWith("#ifdef"))
			    {
				    if (words.Length == 1 || words.Length > 2)
				    {
					    ErrorContext.AddError(ErrorSource.Preprocessor, 7,
						    $"\"{trimmedLine}\" isn't valid; must be \"#ifdef SYMBOL\"",
						    line.FilePath, line.LineNumber, 1);
					    continue;
				    }
				    else
				    {
					    conditionalValue = (definedConstants.Contains(words[1]))
						    ? ConditionalInclusionState.ConditionalTrue
						    : ConditionalInclusionState.ConditionalFalse;
				    }
				}
			    else if (trimmedLine.StartsWith("#ifndef"))
			    {
				    if (words.Length == 1 || words.Length > 2)
				    {
					    ErrorContext.AddError(ErrorSource.Preprocessor, 8,
						    $"\"{trimmedLine}\" isn't valid; must be \"#ifndef SYMBOL\"",
						    line.FilePath, line.LineNumber, 1);
					    continue;
				    }
				    else
				    {
					    conditionalValue = (definedConstants.Contains(words[1]))
						    ? ConditionalInclusionState.ConditionalFalse
						    : ConditionalInclusionState.ConditionalTrue;
				    }
			    }
				else if (trimmedLine.StartsWith("#include"))
			    {
				    if (words.Length != 2)
				    {
					    ErrorContext.AddError(ErrorSource.Preprocessor, 9,
						    $"\"{trimmedLine}\" isn't valid; must be \"#include <file.cix>\" or \"#include \"file.cix\"\".",
						    line.FilePath, line.LineNumber, 1);
						continue;
					}
				    string fileName = words[1].Substring(1, words[1].Length - 2);
				    if (FileAlreadyIncluded(fileName, includedFilePaths, line.FilePath, line.LineNumber))
				    {
					    continue;
				    }


			    }
			}

		    return outputFile;
	    }

	    private static string GetSymbolFromDefine(IReadOnlyList<string> defineWords, string filePath, int lineNumber)
	    {
		    if (defineWords[1].IsIdentifier()) { return defineWords[1]; }
		    else
		    {
			    ErrorContext.AddError(ErrorSource.Preprocessor, 2,
				    $"\"{defineWords[1]}\" is not an identifier",
				    filePath, lineNumber, 1);
		    }
			return null;
		}

	    private static KeyValuePair<string, string>? GetSubstitutionFromDefine(
		    IReadOnlyList<string> defineWords, IEnumerable<string> definedSubstitutions, string filePath, int lineNumber)
	    {
		    if (defineWords[1].IsIdentifier() && (defineWords[2].IsIdentifierOrNumber()))
		    {
			    if (definedSubstitutions.Contains(defineWords[1]))
			    {
				    ErrorContext.AddError(ErrorSource.Preprocessor, 3,
					    $"Symbol {defineWords[1]} is already defined.",
					    filePath, lineNumber, 1);
			    }
			    return new KeyValuePair<string, string>(defineWords[1], defineWords[2]);
		    }
		    else
		    {
			    ErrorContext.AddError(ErrorSource.Preprocessor, 4,
				    $"\"{defineWords[3]}\" for symbol {defineWords[2]} is not a valid substitution; must be a single word or numeric literal.",
				    filePath, lineNumber, 1);
		    }
		    return null;
	    }

	    private static string LoadIncludeFile(string fileName, string basePath)
	    {
		    string filePath = Path.Combine(basePath, fileName);
		    if (!File.Exists(filePath))
		    {
				ErrorContext.AddError(ErrorSource.Preprocessor, 10,
					$"The include file \"{fileName}\" doesn't exist or has an invalid path.",
					);
		    }
	    }

	    private static bool FileAlreadyIncluded(string fileToBeIncludedPath,
		    IEnumerable<(string filePath, int includedOnLineNumber)> alreadyIncludedFilePaths,
		    string includingFilePath, int includeOnLineNumber)
	    {
		    foreach (var filePath in alreadyIncludedFilePaths)
		    {
			    if (fileToBeIncludedPath == filePath.filePath)
			    {
					ErrorContext.AddError(ErrorSource.Preprocessor, 15,
						$"File \"{fileToBeIncludedPath}\" was already included on line {filePath.includedOnLineNumber}.",
						includingFilePath, includeOnLineNumber, 1);
				    return false;
			    }
		    }
		    return true;
	    }
    }
}
