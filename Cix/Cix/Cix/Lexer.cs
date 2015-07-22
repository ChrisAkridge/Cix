using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cix.Exceptions;

namespace Cix
{
	/// <summary>
	/// Divides a source file into individual words by scanning each character.
	/// </summary>
	public sealed class Lexer
	{
		private string file;				// The actual text of the file.
		private StringBuilder builder;		// A builder holding the current word.
		private ParsingContext context;		// The context of the lexing; roughly, what the last scanned character was part of
		private List<string> wordList;		// A list of lexed words.
		private int lineNumber;				// The current line number where the scanning is. Used to make ParseExceptions.
		private int charNumber;				// The current character number where the scanning is. Used to make ParseExceptions.
		private bool withinDirective;		// Set when the lexer finds a # character in Root context and cleared when it then finds a newline.

		/// <summary>
		/// Gets the path to the file being lexed.
		/// </summary>
		public string FilePath { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Lexer"/> class.
		/// </summary>
		/// <param name="filePath">The path to the file to be lexed.</param>
		public Lexer(string file)
		{
			this.file = file;
		}

		/// <summary>
		/// Lexes the source file and returns its words.
		/// </summary>
		/// <returns>A list containing each word of the source file.</returns>
		public List<string> EnumerateWords()
		{
			if (string.IsNullOrEmpty(file))
			{
				return new List<string>();
			}

			builder = new StringBuilder();
			context = ParsingContext.Root;
			wordList = new List<string>();
			lineNumber = charNumber = 0;

			for (int i = 0; i < file.Length; i++)
			{
				charNumber++;
				char current = '\0';
				char last = '\0';
				char next = '\0';

				current = file[i];
				last = (i > 0) ? file[i - 1] : '\0';
				next = (i < file.Length - 1) ? file[i + 1] : '\0';

				if (char.IsWhiteSpace(current))
				{
					ProcessWhitespace(current, last, next);
				}
				else if (current.IsOneOfCharacter('{', '}', '[', ']', '(', ')'))
				{
					ProcessBraceBracketOrParentheses(current, last, next);
				}
				else if (char.IsLetter(current) || current == '_')
				{
					ProcessLetterOrUnderscore(current, last, next);
				}
				else if (char.IsDigit(current))
				{
					ProcessNumber(current, last, next);
				}
				else if (current == '"')
				{
					ProcessQuotationMark(last, next);
				}
				else if (current == '+')
				{
					ProcessPlusSign(last, next);
				}
				else if (current == '-')
				{
					ProcessMinusSign(last, next);
				}
				else if (current == '!')
				{
					ProcessExclamationMark(last, next);
				}
				else if (current == '~')
				{
					ProcessTilde(last, next);
				}
				else if (current == '*')
				{
					ProcessAsterisk(last, next);
				}
				else if (current == '/')
				{
					ProcessForwardSlash(last, next);
				}
				else if (current == '%')
				{
					ProcessPercentSign(last, next);
				}
				else if (current == '<')
				{
					ProcessLessThanSign(last, next);	
				}
				else if (current == '>')
				{
					ProcessGreaterThanSign(last, next);	
				}
				else if (current == '&')
				{
					ProcessAmpersand(last, next);
				}
				else if (current == '|')
				{
					ProcessVerticalLine(last, next);
				}
				else if (current == '^')
				{
					ProcessCaret(last, next);
				}
				else if (current == '.')
				{
					ProcessDot(last, next);
				}
				else if (current == '?')
				{
					ProcessQuestionMark(last, next);
				}
				else if (current == ':')
				{
					ProcessColon(last, next);
				}
				else if (current == '=')
				{
					ProcessEqualsSign(last, next);
				}
				else if (current == '\\')
				{
					ProcessBackslash(last, next);
				}
				else if (current == ';')
				{
					ProcessSemicolon(last, next);
				}
				else if (current == ',')
				{
					ProcessComma(last, next);
				}
				else if (current == '#')
				{
					ProcessNumberSign(last, next);
				}
				else
				{
					if (context == ParsingContext.StringLiteral)
					{
						builder.Append(current);
					}
					else
					{
						throw new ParseException(string.Format("Invalid character {0}.", current), file, lineNumber, charNumber);
					}
				}
			}

			return wordList;
		}

		private void ProcessWhitespace(char current, char last, char next)
		{
			bool isLineTerminator = current.IsOneOfCharacter('\r', '\n');

			if (isLineTerminator && last != '\r')
			{
				lineNumber++;
				charNumber = 0;

				if (withinDirective)
				{
					withinDirective = false;
					if (builder.Length > 0) { AddWordToList(); }
					builder.Append("\\r\\n"); // This newline helps the tokenizer to figure out where directives end
					AddWordToList();
				}
			}

			switch (context)
			{
				case ParsingContext.Root:
				case ParsingContext.Whitespace:
					break;
				case ParsingContext.Directive:
				case ParsingContext.Word:
				case ParsingContext.Operator:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					if (builder.Length > 0) { AddWordToList(); }
					context = ParsingContext.Whitespace;
					break;
				case ParsingContext.StringLiteral:
					if (isLineTerminator && last != '\r')
					{
						builder.Append("\r\n");
					}
					else
					{
						builder.Append(current);
					}
					break;
				default:
					break;
			}
		}

		private void ProcessBraceBracketOrParentheses(char current, char last, char next)
		{
			switch (context)
			{
				case ParsingContext.Root:
				case ParsingContext.Directive:
					throw new ParseException("Invalid bracket, brace, or parentheses", file, lineNumber, charNumber);
				case ParsingContext.Whitespace:
					builder.Append(current);
					AddWordToList();
					context = ParsingContext.Whitespace;
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					AddWordToList();
					context = ParsingContext.Whitespace;
					builder.Append(current);
					AddWordToList();
					break;
				case ParsingContext.Operator:
					AddWordToList();
					builder.Append(current);
					context = ParsingContext.Whitespace;
					break;
				case ParsingContext.StringLiteral:
					builder.Append(current);
					break;
				default:
					break;
			}
		}

		private void ProcessLetterOrUnderscore(char current, char last, char next)
		{
			switch (context)
			{
				case ParsingContext.Root:
				case ParsingContext.Whitespace:
					context = ParsingContext.Word;
					if (builder.Length > 0) { AddWordToList(); }
					builder.Append(current);
					break;
				case ParsingContext.Directive:
					if (current == '_')
					{
						throw new ParseException("Invalid underscore in preprocessor directive", file, lineNumber, charNumber);
					}
					builder.Append(current);
					break;
				case ParsingContext.Word:
					builder.Append(current);
					break;
				case ParsingContext.Operator:
					AddWordToList();
					context = ParsingContext.Word;
					builder.Append(current);
					break;
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
					if (char.ToLower(current).IsOneOfCharacter('u', 'l', 'f', 'd'))
					{
						context = ParsingContext.NumericLiteralSuffix;
						builder.Append(current);
					}
					else
					{
						throw new ParseException(string.Format("Invalid letter {0} in numeric literal.", current), file, lineNumber, charNumber);
					}
					break;
				case ParsingContext.NumericLiteralSuffix:
					if (char.ToLower(last) == 'u' && char.ToLower(current) == 'l')
					{
						builder.Append(current);
						AddWordToList();
						context = ParsingContext.Whitespace;
					}
					else
					{
						throw new ParseException(string.Format("Invalid letter {0} in numeric literal suffix.", current), file, lineNumber, charNumber);
					}
					break;
				case ParsingContext.StringLiteral:
					builder.Append(current);
					break;
				default:
					break;
			}
		}

		private void ProcessQuotationMark(char last, char next)
		{
			switch (context)
			{
				case ParsingContext.Root:
					throw new ParseException("Invalid quotation mark.", file, lineNumber, charNumber);
				case ParsingContext.Whitespace:
					context = ParsingContext.StringLiteral;
					builder.Append('"');
					break;
				case ParsingContext.Directive:
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					throw new ParseException("Invalid quotation mark in word, directive, or numeric literal.", file, lineNumber, charNumber);
				case ParsingContext.Operator:
					AddWordToList();
					builder.Append('"');
					context = ParsingContext.StringLiteral;
					break;
				case ParsingContext.StringLiteral:
					if (last == '\\')
					{
						builder.Append("\\\"");
					}
					else
					{
						builder.Append('"');
						AddWordToList();
						context = ParsingContext.Whitespace;
					}
					break;
				default:
					break;
			}
		}

		private void ProcessPlusSign(char last, char next)
		{
			switch (context)
			{
				case ParsingContext.Root:
				case ParsingContext.Directive:
					throw new ParseException("Invalid character + in root context or directive.", file, lineNumber, charNumber);
				case ParsingContext.Whitespace:
					context = ParsingContext.Operator;
					builder.Append('+');
					break;
				case ParsingContext.Word:
					AddWordToList();
					context = ParsingContext.Operator;
					builder.Append('+');
					break;
				case ParsingContext.Operator:
					if (last == '+')
					{
						builder.Append('+');
						AddWordToList();
						context = ParsingContext.Whitespace;
					}
					else
					{
						throw new ParseException("Invalid + in operator.", file, lineNumber, charNumber);
					}
					break;
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					AddWordToList();
					context = ParsingContext.Operator;
					builder.Append('+');
					break;
				case ParsingContext.StringLiteral:
					builder.Append('+');
					break;
				default:
					break;
			}
		}

		private void ProcessMinusSign(char last, char next)
		{
			switch (context)
			{
				case ParsingContext.Root:
				case ParsingContext.Directive:
					throw new ParseException("Invalid minus sign in root context or directive.", file, lineNumber, charNumber);
				case ParsingContext.Whitespace:
					builder.Append('-');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					AddWordToList();
					builder.Append('-');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.Operator:
					if (last == '-')
					{
						builder.Append('-');
					}
					else
					{
						throw new ParseException("Invalid minus sign in operator.", file, lineNumber, charNumber);
					}
					break;
				case ParsingContext.StringLiteral:
					builder.Append('-');
					break;
				default:
					break;
			}
		}

		private void ProcessExclamationMark(char last, char next)
		{
			switch (context)
			{
				case ParsingContext.Root:
				case ParsingContext.Directive:
				case ParsingContext.Operator:
					throw new ParseException("Invalid exclamation mark in root context, directive, or operator.", file, lineNumber, charNumber);
				case ParsingContext.Whitespace:
					builder.Append('!');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					if (next == '=')
					{
						AddWordToList();
						builder.Append('!');
						context = ParsingContext.Operator;
					}
					else
					{
						throw new ParseException("Invalid exclamation mark in word or numeric literal.", file, lineNumber, charNumber);
					}
					break;
				case ParsingContext.StringLiteral:
					builder.Append('!');
					break;
				default:
					break;
			}
		}

		private void ProcessTilde(char last, char next)
		{
			switch (context)
			{
				case ParsingContext.Root:
				case ParsingContext.Directive:
				case ParsingContext.Operator:
					throw new ParseException("Invalid tilde in root context, directive, or operator.", file, lineNumber, charNumber);
				case ParsingContext.Whitespace:
					builder.Append('~');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					if (next == '=')
					{
						AddWordToList();
						builder.Append('~');
						context = ParsingContext.Operator;
					}
					else
					{
						throw new ParseException("Invalid tilde in word or numeric literal.", file, lineNumber, charNumber);
					}
					break;
				case ParsingContext.StringLiteral:
					builder.Append('~');
					break;
				default:
					break;
			}
		}

		private void ProcessAsterisk(char last, char next)
		{
			switch (context)
			{
				case ParsingContext.Root:
				case ParsingContext.Directive:
					throw new ParseException("Invalid asterisk in root context, or directive.", file, lineNumber, charNumber);
				case ParsingContext.Whitespace:
					builder.Append('*');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.Operator:
					if (last == '*')
					{
						builder.Append('*');
					}
					else
					{
						throw new ParseException("Invalid operator.", file, lineNumber, charNumber);
					}
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					AddWordToList();
					builder.Append('*');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.StringLiteral:
					builder.Append('*');
					break;
				default:
					break;
			}
		}

		private void ProcessForwardSlash(char last, char next)
		{
			switch (context)
			{
				case ParsingContext.Root:
				case ParsingContext.Directive:
				case ParsingContext.Operator:
					throw new ParseException("Invalid forward slash in root context, directive, or operator.", file, lineNumber, charNumber);
				case ParsingContext.Whitespace:
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					AddWordToList();
					builder.Append('/');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.StringLiteral:
					builder.Append('/');
					break;
				default:
					break;
			}
		}

		private void ProcessPercentSign(char last, char next)
		{
			switch (context)
			{
				case ParsingContext.Root:
				case ParsingContext.Directive:
				case ParsingContext.Operator:
					throw new ParseException("Invalid percent sign in root context, directive, or operator.", file, lineNumber, charNumber);
				case ParsingContext.Whitespace:
					builder.Append('%');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					AddWordToList();
					builder.Append('%');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.StringLiteral:
					builder.Append('%');
					break;
				default:
					break;
			}
		}

		private void ProcessLessThanSign(char last, char next)
		{
			switch (context)
			{
				case ParsingContext.Root: // where you left off: fix the directive handler for < and > because they're totally valid here.
					throw new ParseException("Invalid less-than sign in root context.", file, lineNumber, charNumber);
				case ParsingContext.Whitespace:
					builder.Append('<');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.Operator:
					if (last == '<')
					{
						builder.Append('<');
					}
					else
					{
						throw new ParseException("Invalid operator.", file, lineNumber, charNumber);
					}
					break;
				case ParsingContext.Directive:
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					AddWordToList();
					builder.Append('<');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.StringLiteral:
					builder.Append('<');
					break;
				default:
					break;
			}
		}
		
		private void ProcessGreaterThanSign(char last, char next)
		{
			switch (context)
			{
				case ParsingContext.Root:
					throw new ParseException("Invalid greater-than sign in root context.", file, lineNumber, charNumber);
				case ParsingContext.Whitespace:
					builder.Append('>');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.Operator:
					if (last.IsOneOfCharacter('-', '>'))
					{
						builder.Append('>');
					}
					else
					{
						throw new ParseException("Invalid operator.", file, lineNumber, charNumber);
					}
					break;
				case ParsingContext.Directive:
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					AddWordToList();
					builder.Append('>');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.StringLiteral:
					builder.Append('>');
					break;
				default:
					break;
			}
		}

		private void ProcessAmpersand(char last, char next)
		{
			switch (context)
			{
				case ParsingContext.Root:
				case ParsingContext.Directive:
					throw new ParseException("Invalid ampersand in root context or operator.", file, lineNumber, charNumber);
				case ParsingContext.Whitespace:
					if (builder.Length > 0) { AddWordToList(); }
					builder.Append('&');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.Operator:
					if (last == '&')
					{
						builder.Append('&');
					}
					else
					{
						throw new ParseException("Invalid operator.", file, lineNumber, charNumber);
					}
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					AddWordToList();
					builder.Append('&');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.StringLiteral:
					builder.Append('&');
					break;
				default:
					break;
			}
		}

		private void ProcessVerticalLine(char last, char next)
		{
			switch (context)
			{
				case ParsingContext.Root:
				case ParsingContext.Directive:
					throw new ParseException("Invalid vertical line in root context or operator.", file, lineNumber, charNumber);
				case ParsingContext.Whitespace:
					builder.Append('|');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.Operator:
					if (last == '|')
					{
						builder.Append('|');
					}
					else
					{
						throw new ParseException("Invalid operator.", file, lineNumber, charNumber);
					}
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					AddWordToList();
					builder.Append('|');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.StringLiteral:
					builder.Append('|');
					break;
				default:
					break;
			}
		}
		
		private void ProcessCaret(char last, char next)
		{
			switch (context)
			{
				case ParsingContext.Root:
				case ParsingContext.Directive:
				case ParsingContext.Operator:
					throw new ParseException("Invalid caret in root context, directive, operator.", file, lineNumber, charNumber);
				case ParsingContext.Whitespace:
					builder.Append('^');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					AddWordToList();
					builder.Append('^');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.StringLiteral:
					builder.Append('^');
					break;
				default:
					break;
			}
		}

		private void ProcessDot(char last, char next)
		{
			switch (context)
			{
				case ParsingContext.Root:
				case ParsingContext.Whitespace:
				case ParsingContext.Directive:
				case ParsingContext.Operator:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					throw new ParseException("Invalid dot in root context, whitespace context, directive, operator, or numeric literal fraction/suffix.", file, lineNumber, charNumber);
				case ParsingContext.Word:
					AddWordToList();
					builder.Append('.');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.NumericLiteral:
					builder.Append('.');
					context = ParsingContext.NumericLiteralFraction;
					break;
				case ParsingContext.StringLiteral:
					builder.Append('.');
					break;
				default:
					break;
			}
		}

		private void ProcessQuestionMark(char last, char next)
		{
			switch (context)
			{
				case ParsingContext.Root:
				case ParsingContext.Directive:
				case ParsingContext.Operator:
					throw new ParseException("Invalid question mark in root context, directive, or operator.", file, lineNumber, charNumber);
				case ParsingContext.Whitespace:
					builder.Append('?');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					AddWordToList();
					builder.Append('?');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.StringLiteral:
					builder.Append('?');
					break;
				default:
					break;
			}
		}

		private void ProcessColon(char last, char next)
		{
			switch (context)
			{
				case ParsingContext.Root:
				case ParsingContext.Directive:
				case ParsingContext.Operator:
					throw new ParseException("Invalid colon in root context, directive, or operator.", file, lineNumber, charNumber);
				case ParsingContext.Whitespace:
					builder.Append(':');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					AddWordToList();
					builder.Append(':');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.StringLiteral:
					builder.Append(':');
					break;
				default:
					break;
			}
		}

		private void ProcessEqualsSign(char last, char next)
		{
			switch (context)
			{
				case ParsingContext.Root:
				case ParsingContext.Directive:
					throw new ParseException("Invalid equals sign in root context or directive.", file, lineNumber, charNumber);
				case ParsingContext.Whitespace:
					builder.Append('=');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.Operator:
					if (last.IsOneOfCharacter('<', '>', '+', '-', '*', '/', '%', '&', '|', '^', '!', '='))
					{
						builder.Append('=');
						context = ParsingContext.Whitespace;
					}
					else
					{
						throw new ParseException("Invalid operator.", file, lineNumber, charNumber);
					}
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					AddWordToList();
					builder.Append('=');
					context = ParsingContext.Operator;
					break;
				case ParsingContext.StringLiteral:
					builder.Append('=');
					break;
				default:
					break;
			}
		}

		private void ProcessBackslash(char last, char next)
		{
			switch (context)
			{
				case ParsingContext.Root:
				case ParsingContext.Whitespace:
				case ParsingContext.Directive:
				case ParsingContext.Word:
				case ParsingContext.Operator:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					throw new ParseException("Invalid backslash in most contexts.", file, lineNumber, charNumber);
				case ParsingContext.StringLiteral:
					builder.Append('\\');
					break;
				default:
					break;
			}
		}

		private void ProcessSemicolon(char last, char next)
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
					if (builder.Length > 0) { AddWordToList(); }
					builder.Append(';');
					AddWordToList();
					context = ParsingContext.Whitespace;
					break;
				case ParsingContext.Directive:
					throw new ParseException("Invalid semicolon in directive.", file, lineNumber, charNumber);
				case ParsingContext.StringLiteral:
					builder.Append(';');
					break;
				default:
					break;
			}
		}

		private void ProcessComma(char last, char next)
		{
			switch (context)
			{
				case ParsingContext.Root:
				case ParsingContext.Directive:
				case ParsingContext.Operator:
					throw new ParseException("Invalid comma in root context, directive, operator.", file, lineNumber, charNumber);
				case ParsingContext.Whitespace:
					builder.Append(',');
					AddWordToList();
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					AddWordToList();
					builder.Append(',');
					AddWordToList();
					context = ParsingContext.Whitespace;
					break;
				case ParsingContext.StringLiteral:
					builder.Append(',');
					break;
				default:
					break;
			}
		}

		private void ProcessNumberSign(char last, char next)
		{
			switch (context)
			{
				case ParsingContext.Root:
				case ParsingContext.Whitespace:
					builder.Append('#');
					context = ParsingContext.Directive;
					withinDirective = true;
					break;
				case ParsingContext.Directive:
				case ParsingContext.Word:
				case ParsingContext.Operator:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					throw new ParseException("Invalid number sign in directive, word, operator, or numeric literal.", file, lineNumber, charNumber);
				case ParsingContext.StringLiteral:
					builder.Append('#');
					break;
				default:
					break;
			}
		}

		private void ProcessNumber(char current, char last, char next)
		{
			switch (context)
			{
				case ParsingContext.Root:
				case ParsingContext.Directive:
				case ParsingContext.NumericLiteralSuffix:
					throw new ParseException("Invalid number in root context, directive, or numeric literal suffix.", file, lineNumber, charNumber);
				case ParsingContext.Whitespace:
				case ParsingContext.Operator:
					if (builder.Length > 0)
					{
						AddWordToList();
					}
					builder.Append(current);
					context = ParsingContext.NumericLiteral;
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.StringLiteral:
					builder.Append(current);
					break;
				default:
					break;
			}
		}

		private void AddWordToList()
		{
			wordList.Add(builder.ToString());
			builder.Clear();
		}

		/// <summary>
		/// A list of all the different things the word enumerator's loop might be in the middle of.
		/// </summary>
		private enum ParsingContext
		{
			Root,
			Whitespace,
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
