using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix
{
	/// <summary>
	/// Iterates through a source file by returning each word or operator in the code.
	/// </summary>
	public sealed class SourceFileIterator
	{
		private string file;

		public string FilePath { get; private set; }

		public SourceFileIterator(string filePath)
		{
			if (!File.Exists(filePath))
			{
				throw new FileNotFoundException(string.Format("The file at {0} does not exist.", filePath));
			}

			this.file = File.ReadAllText(filePath);
		}

		public IEnumerable<string> EnumerateWords()
		{
			if (string.IsNullOrEmpty(this.file))
			{
				yield break;
			}

			CurrentEnumerationScope scope = CurrentEnumerationScope.Root;
			StringBuilder currentWord = new StringBuilder();

			for (int i = 0; i < this.file.Length; i++)
			{
				// Separation rules:
				// Yield a string on whitespace following a word/operator
				// Do not yield a string on whitespace following a whitespaces
				// Yield the last word/operator on a brace/bracket/paren not preceded by a whitespace, then yield the curly brace
				// Yield just the brace/bracket/paren if preceded by whitespaces
				// Always yield ;

				// Comments: for single line comments, skip to past the next newline and keep going
				// For multi-line comments, skip to past the */

				// Operator rules:
				// Yield immediately any of ~ . ? :
				// For +, check for any of (+ =) and yield +, ++, or +=
				// For -, check for any of (- =) and yield -, --, or -=
				// For !, check for = and yield ! or !=
				// For *, check for = and yield * or *=
				// For /, check for = and yield / or /=
				// For ^, check for = and yield ^ or ^=
				// For %, check for = and yield % or %=
				// For <, check for any of (= < <=) and yield <, <=, <<, <<=
				// For >, check for any of (= > >=) and yield >, >=, >>, >>=
				// For =, check for = and yield = or ==

				char current = this.file[i];

				if (char.IsLetterOrDigit(current) || current == '_')
				{
					#region Word Processor
					// A normal word. Always starts with a non-numeric character, though.
					switch (scope)
					{
						case CurrentEnumerationScope.Root:
						case CurrentEnumerationScope.Whitespace:
							if (char.IsDigit(current))
							{
								// This is a numeric literal, not an identifier.
								scope = CurrentEnumerationScope.NumericLiteral;
							}
							else
							{
								// This is a word.
								scope = CurrentEnumerationScope.Word;
							}
							currentWord.Append(current);
							break;
						case CurrentEnumerationScope.Comment:
							break;
						case CurrentEnumerationScope.Directive:
							if (char.IsDigit(current))
							{
								throw new ParseException("Invalid directive.", this.file, i);
							}
							currentWord.Append(current);
							break;
						case CurrentEnumerationScope.Word:
							currentWord.Append(current);
							break;
						case CurrentEnumerationScope.Operator:
							throw new ParseException(string.Format("Invalid character {0} in operator.", current), this.file, i);
						case CurrentEnumerationScope.NumericLiteral:
							if (char.IsDigit(current))
							{
								currentWord.Append(current);
							}
							else if (current == '.')
							{
								scope = CurrentEnumerationScope.NumericLiteralFraction;
								currentWord.Append(current);
							}
							else if ( char.ToLower(current) == 'l' || char.ToLower(current) == 'u'
								|| char.ToLower(current) == 'f' || char.ToLower(current) == 'd')
							{
								scope = CurrentEnumerationScope.NumericLiteralSuffix;
								currentWord.Append(current);
							}
							else
							{
								throw new ParseException(string.Format("Invalid character {0} in numeric literal.", current), this.file, i);
							}
							break;
						case CurrentEnumerationScope.NumericLiteralFraction:
							if (char.IsDigit(current))
							{
								currentWord.Append(current);
							}
							else if (char.ToLower(current) == 'l' || char.ToLower(current) == 'u'
								|| char.ToLower(current) == 'f' || char.ToLower(current) == 'd')
							{
								scope = CurrentEnumerationScope.NumericLiteralSuffix;
								currentWord.Append(current);
							}
							else if (current == '.')
							{
								throw new ParseException("Fractional portion of numeric literal has already begun.", this.file, i);
							}
							else
							{
								throw new ParseException(string.Format("Invalid character {0} in numeric literal.", current), this.file, i);
							}
							break;
						case CurrentEnumerationScope.NumericLiteralSuffix:
							if (char.ToLower(current) == 'l')
							{
								currentWord.Append(current);
								// At this point, we can be certain that the numeric literal is complete.
								yield return currentWord.ToString();
								currentWord = new StringBuilder();
								scope = CurrentEnumerationScope.Root;
							}
							else
							{
								throw new ParseException(string.Format("Invalid character {0} in numeric literal suffix.", current), this.file, i);
							}
							break;
						case CurrentEnumerationScope.StringLiteral:
							currentWord.Append(current);
							break;
						default:
							break;
					}
					#endregion
				}
				else if (char.IsWhiteSpace(current))
				{
					if (scope != CurrentEnumerationScope.Whitespace && scope != CurrentEnumerationScope.Root)
					{
						yield return currentWord.ToString();
						currentWord = new StringBuilder();
					}
					scope = CurrentEnumerationScope.Whitespace;
				}
				else if (current == '{' || current == '}' || current == '[' || current == ']' || current == '(' || current == ')')
				{
					if (scope != CurrentEnumerationScope.Whitespace)
					{
						yield return currentWord.ToString();
						currentWord = new StringBuilder();
						yield return current.ToString();
					}
					else
					{
						yield return current.ToString();
					}
				}
				else if (current == '#')
				{
					#region Number Sign Processor
					// Either a preprocessor directive or the # in a string literal.
					if (scope == CurrentEnumerationScope.Root)
					{
						// It's a preprocessor directive.
						scope = CurrentEnumerationScope.Directive;
						currentWord.Append('#');
					}
					else if (scope == CurrentEnumerationScope.StringLiteral)
					{
						// It's in a string literal.
						currentWord.Append('#');
					}
					else if (scope == CurrentEnumerationScope.Comment) { }
					else
					{
						throw new ParseException(string.Format("Found a number sign in an invalid context of {0}.", scope), this.file, i);
					}
					#endregion
				}
			}
		}

		private static bool IsOneOfCharacter(char check, params char[] values)
		{
			HashSet<char> hash = new HashSet<char>(values);
			return hash.Contains(check);
		}

		/// <summary>
		/// A list of all the different things the word enumerator's loop might be in the middle of.
		/// </summary>
		private enum CurrentEnumerationScope
		{
			Root,
			Whitespace,
			Comment,
			Directive,
			Word,
			Operator,
			NumericLiteral,
			NumericLiteralFraction,
			NumericLiteralSuffix,
			StringLiteral
		}
	}
}
