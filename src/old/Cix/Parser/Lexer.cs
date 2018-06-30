using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cix.Errors;
using Cix.Text;

namespace Cix.Parser
{
	/// <summary>
	/// Divides a preproccesed source file with comments removed into individual words by scanning
	/// each character.
	/// </summary>
	internal sealed class Lexer
	{
		private readonly IErrorListProvider errorList;
		private LineCharEnumerator charEnumerator;
		private readonly StringBuilder currentWord = new StringBuilder();
		private ParsingContext context;
		private readonly List<LexedWord> wordList = new List<LexedWord>();

		private int wordIndexOnLine = -1;
		private int lastLineNumber;

		/// <summary>
		/// Initializes a new instance of the <see cref="Lexer"/> class.
		/// </summary>
		/// <param name="errorList">An interface representing an error list that this class can add errors to.</param>
		public Lexer(IErrorListProvider errorList) => this.errorList = errorList;

		/// <summary>
		/// Lexes the source file and returns its words.
		/// </summary>
		/// <returns>A list containing each word of the source file.</returns>
		public IList<LexedWord> EnumerateWords(IList<Line> file)
		{
			charEnumerator = new LineCharEnumerator(file);
			context = ParsingContext.Root;

			foreach (LineChar lineChar in charEnumerator)
			{
				char last = charEnumerator.Previous.Text;
				char next = charEnumerator.Next.Text;
				char current = lineChar.Text;

				if (char.IsWhiteSpace(current))
				{
					ProcessWhitespace(current, last);
				}
				else if (current.IsOneOfCharacter('{', '}', '[', ']', '(', ')'))
				{
					ProcessBraceBracketOrParentheses(current);
				}
				else if (char.IsLetter(current) || lineChar.Text == '_')
				{
					ProcessLetterOrUnderscore(current, last);
				}
				else if (char.IsDigit(current))
				{
					ProcessNumber(current);
				}
				else
				{
					switch (current)
					{
						case '"':
							ProcessQuotationMark(last);
							break;
						case '+':
							ProcessPlusSign(last);
							break;
						case '-':
							ProcessMinusSign(last);
							break;
						case '!':
							ProcessExclamationMark(next);
							break;
						case '~':
							ProcessTilde(next);
							break;
						case '*':
							ProcessAsterisk(last);
							break;
						case '/':
							ProcessForwardSlash();
							break;
						case '%':
							ProcessPercentSign();
							break;
						case '<':
							ProcessLessThanSign(last);
							break;
						case '>':
							ProcessGreaterThanSign(last);
							break;
						case '&':
							ProcessAmpersand(last);
							break;
						case '|':
							ProcessVerticalLine(last);
							break;
						case '^':
							ProcessCaret();
							break;
						case '.':
							ProcessDot();
							break;
						case ':':
							ProcessColon();
							break;
						case '=':
							ProcessEqualsSign(last);
							break;
						case '\\':
							ProcessBackslash();
							break;
						case ';':
							ProcessSemicolon();
							break;
						case ',':
							ProcessComma();
							break;
						default:
							if (context == ParsingContext.StringLiteral)
							{
								currentWord.Append(lineChar);
							}
							else
							{
								errorList.AddError(ErrorSource.Lexer, 1, $"Invalid character {current}.",
									charEnumerator.CurrentLine);
							}
							break;
					}
				}
			}

			return wordList;
		}

		private void ProcessWhitespace(char current, char last)
		{
			bool isLineTerminator = (current == '\r') || (current == '\n');

			switch (context)
			{
				case ParsingContext.Root:
				case ParsingContext.Whitespace:
					break;
				case ParsingContext.Word:
				case ParsingContext.Operator:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
				case ParsingContext.HexadecimalNumericLiteral:
					if (currentWord.Length > 0) { AddWordToList(); }
					context = ParsingContext.Whitespace;
					break;
				case ParsingContext.StringLiteral:
					if (isLineTerminator && last != '\r')
					{
						currentWord.Append("\r\n");
					}
					else
					{
						currentWord.Append(current);
					}
					break;
				default: throw new ArgumentOutOfRangeException();
			}
		}

		private void ProcessBraceBracketOrParentheses(char current)
		{
			switch (context)
			{
				case ParsingContext.Root:
				case ParsingContext.Whitespace:
					currentWord.Append(current);
					AddWordToList();
					context = ParsingContext.Whitespace;
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
				case ParsingContext.HexadecimalNumericLiteral:
					AddWordToList();
					context = ParsingContext.Whitespace;
					currentWord.Append(current);
					AddWordToList();
					break;
				case ParsingContext.Operator:
					AddWordToList();
					currentWord.Append(current);
					context = ParsingContext.Whitespace;
					break;
				case ParsingContext.StringLiteral:
					currentWord.Append(current);
					break;
				default: throw new ArgumentOutOfRangeException();
			}
		}

		private void ProcessLetterOrUnderscore(char current, char last)
		{
			switch (context)
			{
				case ParsingContext.Root:
				case ParsingContext.Whitespace:
					context = ParsingContext.Word;
					if (currentWord.Length > 0) { AddWordToList(); }
					currentWord.Append(current);
					break;
				case ParsingContext.Word:
					currentWord.Append(current);
					break;
				case ParsingContext.Operator:
					AddWordToList();
					context = ParsingContext.Word;
					currentWord.Append(current);
					break;
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
					if (char.ToLower(current).IsOneOfCharacter('u', 'l', 'f', 'd'))
					{
						context = ParsingContext.NumericLiteralSuffix;
						currentWord.Append(current);
					}
					else if (char.ToLower(current) == 'x' && char.IsNumber(last))
					{
						// We're in a hexadecimal literal.
						context = ParsingContext.HexadecimalNumericLiteral;
						currentWord.Append(current);
					}
					else
					{
						errorList.AddError(ErrorSource.Lexer, 1, $"Invalid character {current}.", charEnumerator.CurrentLine);
					}
					break;
				case ParsingContext.NumericLiteralSuffix:
					if (char.ToLower(last) == 'u' && char.ToLower(current) == 'l')
					{
						currentWord.Append(current);
						AddWordToList();
						context = ParsingContext.Whitespace;
					}
					else
					{
						errorList.AddError(ErrorSource.Lexer, 1, $"Invalid character {current}.", charEnumerator.CurrentLine);
					}
					break;
				case ParsingContext.HexadecimalNumericLiteral:
					if (char.ToLower(current).IsOneOfCharacter('a', 'b', 'c', 'd', 'e', 'f'))
					{
						currentWord.Append(current);
					}
					else
					{
						errorList.AddError(ErrorSource.Lexer, 1, $"Invalid character {current}.", charEnumerator.CurrentLine);
					}
					break;
				case ParsingContext.StringLiteral:
					currentWord.Append(current);
					break;
				default: throw new ArgumentOutOfRangeException();
			}
		}

		private void ProcessQuotationMark(char last)
		{
			switch (context)
			{
				case ParsingContext.Root:
					errorList.AddError(ErrorSource.Lexer, 1, "Invalid character \".", charEnumerator.CurrentLine);
					break;
				case ParsingContext.Whitespace:
					context = ParsingContext.StringLiteral;
					currentWord.Append('"');
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
				case ParsingContext.HexadecimalNumericLiteral:
					errorList.AddError(ErrorSource.Lexer, 1, "Invalid character \".", charEnumerator.CurrentLine);
					break;
				case ParsingContext.Operator:
					AddWordToList();
					currentWord.Append('"');
					context = ParsingContext.StringLiteral;
					break;
				case ParsingContext.StringLiteral:
					if (last == '\\')
					{
						currentWord.Append("\\\"");
					}
					else
					{
						currentWord.Append('"');
						AddWordToList();
						context = ParsingContext.Whitespace;
					}
					break;
				default: throw new ArgumentOutOfRangeException();
			}
		}

		private void ProcessPlusSign(char last)
		{
			switch (context)
			{
				case ParsingContext.Root:
				case ParsingContext.Whitespace:
					context = ParsingContext.Operator;
					currentWord.Append('+');
					break;
				case ParsingContext.Word:
					AddWordToList();
					context = ParsingContext.Operator;
					currentWord.Append('+');
					break;
				case ParsingContext.Operator:
					if (last == '+')
					{
						currentWord.Append('+');
						AddWordToList();
						context = ParsingContext.Whitespace;
					}
					else
					{
						errorList.AddError(ErrorSource.Lexer, 1, "Invalid character +.", charEnumerator.CurrentLine);
					}
					break;
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
				case ParsingContext.HexadecimalNumericLiteral:
					AddWordToList();
					context = ParsingContext.Operator;
					currentWord.Append('+');
					break;
				case ParsingContext.StringLiteral:
					currentWord.Append('+');
					break;
				default: throw new ArgumentOutOfRangeException();
			}
		}

		private void ProcessMinusSign(char last)
		{
			switch (context)
			{
				case ParsingContext.Root:
				case ParsingContext.Whitespace:
					currentWord.Append('-');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
				case ParsingContext.HexadecimalNumericLiteral:
					AddWordToList();
					currentWord.Append('-');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.Operator:
					if (last == '-')
					{
						currentWord.Append('-');
					}
					else
					{
						errorList.AddError(ErrorSource.Lexer, 1, "Invalid character -.", charEnumerator.CurrentLine);
					}
					break;
				case ParsingContext.StringLiteral:
					currentWord.Append('-');
					break;
				default: throw new ArgumentOutOfRangeException();
			}
		}

		private void ProcessExclamationMark(char next)
		{
			switch (context)
			{
				case ParsingContext.Root:
				case ParsingContext.Operator:
					errorList.AddError(ErrorSource.Lexer, 1, "Invalid character !.", charEnumerator.CurrentLine);
					break;
				case ParsingContext.Whitespace:
					currentWord.Append('!');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
				case ParsingContext.HexadecimalNumericLiteral:
					if (next == '=')
					{
						AddWordToList();
						currentWord.Append('!');
						context = ParsingContext.Operator;
					}
					else
					{
						errorList.AddError(ErrorSource.Lexer, 1, "Invalid character !.", charEnumerator.CurrentLine);
					}
					break;
				case ParsingContext.StringLiteral:
					currentWord.Append('!');
					break;
				default: throw new ArgumentOutOfRangeException();
			}
		}

		private void ProcessTilde(char next)
		{
			switch (context)
			{
				case ParsingContext.Root:
				case ParsingContext.Operator:
					errorList.AddError(ErrorSource.Lexer, 1, "Invalid character ~.", charEnumerator.CurrentLine);
					break;
				case ParsingContext.Whitespace:
					currentWord.Append('~');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
				case ParsingContext.HexadecimalNumericLiteral:
					if (next == '=')
					{
						AddWordToList();
						currentWord.Append('~');
						context = ParsingContext.Operator;
					}
					else
					{
						errorList.AddError(ErrorSource.Lexer, 1, "Invalid character ~.", charEnumerator.CurrentLine);
					}
					break;
				case ParsingContext.StringLiteral:
					currentWord.Append('~');
					break;
				default: throw new ArgumentOutOfRangeException();
			}
		}

		private void ProcessAsterisk(char last)
		{
			switch (context)
			{
				case ParsingContext.Root:
				case ParsingContext.Whitespace:
					currentWord.Append('*');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.Operator:
					if (last == '*')
					{
						AddWordToList();
						currentWord.Append('*');
					}
					else
					{
						errorList.AddError(ErrorSource.Lexer, 1, "Invalid character *.", charEnumerator.CurrentLine);
					}
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
				case ParsingContext.HexadecimalNumericLiteral:
					AddWordToList();
					currentWord.Append('*');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.StringLiteral:
					currentWord.Append('*');
					break;
				default: throw new ArgumentOutOfRangeException();
			}
		}

		private void ProcessForwardSlash()
		{
			switch (context)
			{
				case ParsingContext.Root:
				case ParsingContext.Operator:
					errorList.AddError(ErrorSource.Lexer, 1, "Invalid character /.", charEnumerator.CurrentLine);
					break;
				case ParsingContext.Whitespace:
					currentWord.Append('/');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
				case ParsingContext.HexadecimalNumericLiteral:
					AddWordToList();
					currentWord.Append('/');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.StringLiteral:
					currentWord.Append('/');
					break;
				default: throw new ArgumentOutOfRangeException();
			}
		}

		private void ProcessPercentSign()
		{
			switch (context)
			{
				case ParsingContext.Root:
				case ParsingContext.Operator:
					errorList.AddError(ErrorSource.Lexer, 1, "Invalid character %.", charEnumerator.CurrentLine);
					break;
				case ParsingContext.Whitespace:
					currentWord.Append('%');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
				case ParsingContext.HexadecimalNumericLiteral:
					AddWordToList();
					currentWord.Append('%');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.StringLiteral:
					currentWord.Append('%');
					break;
				default: throw new ArgumentOutOfRangeException();
			}
		}

		private void ProcessLessThanSign(char last)
		{
			switch (context)
			{
				case ParsingContext.Root:
					errorList.AddError(ErrorSource.Lexer, 1, "Invalid character <.", charEnumerator.CurrentLine);
					break;
				case ParsingContext.Whitespace:
					currentWord.Append('<');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.Operator:
					if (last == '<')
					{
						currentWord.Append('<');
					}
					else
					{
						errorList.AddError(ErrorSource.Lexer, 1, "Invalid character <.", charEnumerator.CurrentLine);
					}
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
				case ParsingContext.HexadecimalNumericLiteral:
					AddWordToList();
					currentWord.Append('<');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.StringLiteral:
					currentWord.Append('<');
					break;
				default: throw new ArgumentOutOfRangeException();
			}
		}

		private void ProcessGreaterThanSign(char last)
		{
			switch (context)
			{
				case ParsingContext.Root:
					errorList.AddError(ErrorSource.Lexer, 1, "Invalid character >.", charEnumerator.CurrentLine);
					break;
				case ParsingContext.Whitespace:
					currentWord.Append('>');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.Operator:
					if (last.IsOneOfCharacter('-', '>'))
					{
						currentWord.Append('>');
					}
					else
					{
						errorList.AddError(ErrorSource.Lexer, 1, "Invalid character >.", charEnumerator.CurrentLine);
					}
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
				case ParsingContext.HexadecimalNumericLiteral:
					AddWordToList();
					currentWord.Append('>');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.StringLiteral:
					currentWord.Append('>');
					break;
				default: throw new ArgumentOutOfRangeException();
			}
		}

		private void ProcessAmpersand(char last)
		{
			switch (context)
			{
				case ParsingContext.Root:
					errorList.AddError(ErrorSource.Lexer, 1, "Invalid character &.", charEnumerator.CurrentLine);
					break;
				case ParsingContext.Whitespace:
					if (currentWord.Length > 0) { AddWordToList(); }
					currentWord.Append('&');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.Operator:
					if (last == '&')
					{
						currentWord.Append('&');
					}
					else
					{
						errorList.AddError(ErrorSource.Lexer, 1, "Invalid character &.", charEnumerator.CurrentLine);
					}
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
				case ParsingContext.HexadecimalNumericLiteral:
					AddWordToList();
					currentWord.Append('&');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.StringLiteral:
					currentWord.Append('&');
					break;
				default: throw new ArgumentOutOfRangeException();
			}
		}

		private void ProcessVerticalLine(char last)
		{
			switch (context)
			{
				case ParsingContext.Root:
					errorList.AddError(ErrorSource.Lexer, 1, "Invalid character |.", charEnumerator.CurrentLine);
					break;
				case ParsingContext.Whitespace:
					currentWord.Append('|');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.Operator:
					if (last == '|')
					{
						currentWord.Append('|');
					}
					else
					{
						errorList.AddError(ErrorSource.Lexer, 1, "Invalid character |.", charEnumerator.CurrentLine);
					}
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
				case ParsingContext.HexadecimalNumericLiteral:
					AddWordToList();
					currentWord.Append('|');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.StringLiteral:
					currentWord.Append('|');
					break;
				default: throw new ArgumentOutOfRangeException();
			}
		}

		private void ProcessCaret()
		{
			switch (context)
			{
				case ParsingContext.Root:
				case ParsingContext.Operator:
					errorList.AddError(ErrorSource.Lexer, 1, "Invalid character ^.", charEnumerator.CurrentLine);
					break;
				case ParsingContext.Whitespace:
					currentWord.Append('^');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
				case ParsingContext.HexadecimalNumericLiteral:
					AddWordToList();
					currentWord.Append('^');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.StringLiteral:
					currentWord.Append('^');
					break;
				default: throw new ArgumentOutOfRangeException();
			}
		}

		private void ProcessDot()
		{
			switch (context)
			{
				case ParsingContext.Root:
				case ParsingContext.Whitespace:
				case ParsingContext.Operator:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
				case ParsingContext.HexadecimalNumericLiteral:
					errorList.AddError(ErrorSource.Lexer, 1, "Invalid character ..", charEnumerator.CurrentLine);
					break;
				case ParsingContext.Word:
					AddWordToList();
					currentWord.Append('.');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.NumericLiteral:
					currentWord.Append('.');
					context = ParsingContext.NumericLiteralFraction;
					break;
				case ParsingContext.StringLiteral:
					currentWord.Append('.');
					break;
				default: throw new ArgumentOutOfRangeException();
			}
		}

		private void ProcessColon()
		{
			switch (context)
			{
				case ParsingContext.Root:
				case ParsingContext.Operator:
					errorList.AddError(ErrorSource.Lexer, 1, "Invalid character :.", charEnumerator.CurrentLine);
					break;
				case ParsingContext.Whitespace:
					currentWord.Append(':');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
				case ParsingContext.HexadecimalNumericLiteral:
					AddWordToList();
					currentWord.Append(':');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.StringLiteral:
					currentWord.Append(':');
					break;
				default: throw new ArgumentOutOfRangeException();
			}
		}

		private void ProcessEqualsSign(char last)
		{
			switch (context)
			{
				case ParsingContext.Root:
					errorList.AddError(ErrorSource.Lexer, 1, "Invalid character =.", charEnumerator.CurrentLine);
					break;
				case ParsingContext.Whitespace:
					currentWord.Append('=');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.Operator:
					if (last.IsOneOfCharacter('<', '>', '+', '-', '*', '/', '%', '&', '|', '^', '!', '='))
					{
						currentWord.Append('=');
						context = ParsingContext.Whitespace;
					}
					else
					{
						errorList.AddError(ErrorSource.Lexer, 1, "Invalid character =.", charEnumerator.CurrentLine);
					}
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
				case ParsingContext.HexadecimalNumericLiteral:
					AddWordToList();
					currentWord.Append('=');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.StringLiteral:
					currentWord.Append('=');
					break;
				default: throw new ArgumentOutOfRangeException();
			}
		}

		private void ProcessBackslash()
		{
			switch (context)
			{
				case ParsingContext.Root:
				case ParsingContext.Whitespace:
				case ParsingContext.Word:
				case ParsingContext.Operator:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
				case ParsingContext.HexadecimalNumericLiteral:
					errorList.AddError(ErrorSource.Lexer, 1, "Invalid character \\.", charEnumerator.CurrentLine);
					break;
				case ParsingContext.StringLiteral:
					currentWord.Append('\\');
					break;
				default: throw new ArgumentOutOfRangeException();
			}
		}

		private void ProcessSemicolon()
		{
			switch (context)
			{
				case ParsingContext.Root:
				case ParsingContext.Whitespace:
				case ParsingContext.Word:
				case ParsingContext.Operator:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
				case ParsingContext.HexadecimalNumericLiteral:
					if (currentWord.Length > 0) { AddWordToList(); }
					currentWord.Append(';');
					AddWordToList();
					context = ParsingContext.Whitespace;
					break;
				case ParsingContext.StringLiteral:
					currentWord.Append(';');
					break;
				default: throw new ArgumentOutOfRangeException();
			}
		}

		private void ProcessComma()
		{
			switch (context)
			{
				case ParsingContext.Root:
				case ParsingContext.Operator:
					errorList.AddError(ErrorSource.Lexer, 1, "Invalid character ,.", charEnumerator.CurrentLine);
					break;
				case ParsingContext.Whitespace:
					currentWord.Append(',');
					AddWordToList();
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
				case ParsingContext.HexadecimalNumericLiteral:
					AddWordToList();
					currentWord.Append(',');
					AddWordToList();
					context = ParsingContext.Whitespace;
					break;
				case ParsingContext.StringLiteral:
					currentWord.Append(',');
					break;
				default: throw new ArgumentOutOfRangeException();
			}
		}

		private void ProcessNumber(char current)
		{
			switch (context)
			{
				case ParsingContext.Root:
				case ParsingContext.NumericLiteralSuffix:
					errorList.AddError(ErrorSource.Lexer, 1, $"Invalid character {current}.", charEnumerator.CurrentLine);
					break;
				case ParsingContext.Whitespace:
				case ParsingContext.Operator:
					if (currentWord.Length > 0)
					{
						AddWordToList();
					}
					currentWord.Append(current);
					context = ParsingContext.NumericLiteral;
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.HexadecimalNumericLiteral:
				case ParsingContext.StringLiteral:
					currentWord.Append(current);
					break;
				default: throw new ArgumentOutOfRangeException();
			}
		}

		private void AddWordToList()
		{
			Line currentLine = charEnumerator.CurrentLine;
			if (currentLine.LineNumber != lastLineNumber)
			{
				lastLineNumber = currentLine.LineNumber;
				wordIndexOnLine = -1;
			}

			wordList.Add(new LexedWord(currentLine.FilePath, currentLine.LineNumber, ++wordIndexOnLine, currentWord.ToString()));
			currentWord.Clear();
		}

		/// <summary>
		/// A list of all the different things the word enumerator's loop might be in the middle of.
		/// </summary>
		private enum ParsingContext
		{
			Root,
			Whitespace,
			Word,
			Operator,
			NumericLiteral,
			NumericLiteralFraction,
			NumericLiteralSuffix,
			HexadecimalNumericLiteral,
			StringLiteral,
		}
	}
}
