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
			if (string.IsNullOrEmpty(this.file))
			{
				return new List<string>();
			}

			this.builder = new StringBuilder();
			this.context = ParsingContext.Root;
			this.wordList = new List<string>();
			this.lineNumber = this.charNumber = 0;

			for (int i = 0; i < this.file.Length; i++)
			{
				this.charNumber++;
				char current = '\0';
				char last = '\0';
				char next = '\0';

				try
				{
					current = this.file[i];
					last = (i > 0) ? this.file[i - 1] : '\0';
					next = (i < this.file.Length - 1) ? this.file[i + 1] : '\0';
				}
				catch (Exception ex)
				{
					
				}

				if (char.IsWhiteSpace(current))
				{
					this.ProcessWhitespace(current, last, next);
				}
				else if (current.IsOneOfCharacter('{', '}', '[', ']', '(', ')'))
				{
					this.ProcessBraceBracketOrParentheses(current, last, next);
				}
				else if (char.IsLetter(current) || current == '_')
				{
					this.ProcessLetterOrUnderscore(current, last, next);
				}
				else if (char.IsDigit(current))
				{
					this.ProcessNumber(current, last, next);
				}
				else if (current == '"')
				{
					this.ProcessQuotationMark(last, next);
				}
				else if (current == '+')
				{
					this.ProcessPlusSign(last, next);
				}
				else if (current == '-')
				{
					this.ProcessMinusSign(last, next);
				}
				else if (current == '!')
				{
					this.ProcessExclamationMark(last, next);
				}
				else if (current == '~')
				{
					this.ProcessTilde(last, next);
				}
				else if (current == '*')
				{
					this.ProcessAsterisk(last, next);
				}
				else if (current == '/')
				{
					this.ProcessForwardSlash(last, next);
				}
				else if (current == '%')
				{
					this.ProcessPercentSign(last, next);
				}
				else if (current == '<')
				{
					this.ProcessLessThanSign(last, next);	
				}
				else if (current == '>')
				{
					this.ProcessGreaterThanSign(last, next);	
				}
				else if (current == '&')
				{
					this.ProcessAmpersand(last, next);
				}
				else if (current == '|')
				{
					this.ProcessVerticalLine(last, next);
				}
				else if (current == '^')
				{
					this.ProcessCaret(last, next);
				}
				else if (current == '.')
				{
					this.ProcessDot(last, next);
				}
				else if (current == '?')
				{
					this.ProcessQuestionMark(last, next);
				}
				else if (current == ':')
				{
					this.ProcessColon(last, next);
				}
				else if (current == '=')
				{
					this.ProcessEqualsSign(last, next);
				}
				else if (current == '\\')
				{
					this.ProcessBackslash(last, next);
				}
				else if (current == ';')
				{
					this.ProcessSemicolon(last, next);
				}
				else if (current == ',')
				{
					this.ProcessComma(last, next);
				}
				else if (current == '#')
				{
					this.ProcessNumberSign(last, next);
				}
				else
				{
					if (this.context == ParsingContext.StringLiteral)
					{
						this.builder.Append(current);
					}
					else
					{
						throw new ParseException(string.Format("Invalid character {0}.", current), this.file, this.lineNumber, this.charNumber);
					}
				}
			}

			return this.wordList;
		}

		private void ProcessWhitespace(char current, char last, char next)
		{
			bool isLineTerminator = current.IsOneOfCharacter('\r', '\n');

			if (isLineTerminator && last != '\r')
			{
				this.lineNumber++;
				this.charNumber = 0;

				if (this.withinDirective)
				{
					this.withinDirective = false;
					if (this.builder.Length > 0) { this.AddWordToList(); }
					this.builder.Append("\\r\\n"); // This newline helps the tokenizer to figure out where directives end
					this.AddWordToList();
				}
			}

			switch (this.context)
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
					if (this.builder.Length > 0) { this.AddWordToList(); }
					this.context = ParsingContext.Whitespace;
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
			switch (this.context)
			{
				case ParsingContext.Root:
				case ParsingContext.Directive:
					throw new ParseException("Invalid bracket, brace, or parentheses", this.file, this.lineNumber, this.charNumber);
				case ParsingContext.Whitespace:
					builder.Append(current);
					this.AddWordToList();
					this.context = ParsingContext.Whitespace;
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					this.AddWordToList();
					this.context = ParsingContext.Whitespace;
					this.builder.Append(current);
					this.AddWordToList();
					break;
				case ParsingContext.Operator:
					this.AddWordToList();
					this.builder.Append(current);
					this.context = ParsingContext.Whitespace;
					break;
				case ParsingContext.StringLiteral:
					this.builder.Append(current);
					break;
				default:
					break;
			}
		}

		private void ProcessLetterOrUnderscore(char current, char last, char next)
		{
			switch (this.context)
			{
				case ParsingContext.Root:
				case ParsingContext.Whitespace:
					this.context = ParsingContext.Word;
					if (this.builder.Length > 0) { this.AddWordToList(); }
					this.builder.Append(current);
					break;
				case ParsingContext.Directive:
					if (current == '_')
					{
						throw new ParseException("Invalid underscore in preprocessor directive", this.file, this.lineNumber, this.charNumber);
					}
					this.builder.Append(current);
					break;
				case ParsingContext.Word:
					this.builder.Append(current);
					break;
				case ParsingContext.Operator:
					this.AddWordToList();
					this.context = ParsingContext.Word;
					this.builder.Append(current);
					break;
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
					if (char.ToLower(current).IsOneOfCharacter('u', 'l', 'f', 'd'))
					{
						this.context = ParsingContext.NumericLiteralSuffix;
						this.builder.Append(current);
					}
					else
					{
						throw new ParseException(string.Format("Invalid letter {0} in numeric literal.", current), this.file, this.lineNumber, this.charNumber);
					}
					break;
				case ParsingContext.NumericLiteralSuffix:
					if (char.ToLower(last) == 'u' && char.ToLower(current) == 'l')
					{
						this.builder.Append(current);
						this.AddWordToList();
						this.context = ParsingContext.Whitespace;
					}
					else
					{
						throw new ParseException(string.Format("Invalid letter {0} in numeric literal suffix.", current), this.file, this.lineNumber, this.charNumber);
					}
					break;
				case ParsingContext.StringLiteral:
					this.builder.Append(current);
					break;
				default:
					break;
			}
		}

		private void ProcessQuotationMark(char last, char next)
		{
			switch (this.context)
			{
				case ParsingContext.Root:
					throw new ParseException("Invalid quotation mark.", this.file, this.lineNumber, this.charNumber);
				case ParsingContext.Whitespace:
					this.context = ParsingContext.StringLiteral;
					this.builder.Append('"');
					break;
				case ParsingContext.Directive:
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					throw new ParseException("Invalid quotation mark in word, directive, or numeric literal.", this.file, this.lineNumber, this.charNumber);
				case ParsingContext.Operator:
					this.AddWordToList();
					this.builder.Append('"');
					this.context = ParsingContext.StringLiteral;
					break;
				case ParsingContext.StringLiteral:
					if (last == '\\')
					{
						builder.Append("\\\"");
					}
					else
					{
						this.builder.Append('"');
						this.AddWordToList();
						this.context = ParsingContext.Whitespace;
					}
					break;
				default:
					break;
			}
		}

		private void ProcessPlusSign(char last, char next)
		{
			switch (this.context)
			{
				case ParsingContext.Root:
				case ParsingContext.Directive:
					throw new ParseException("Invalid character + in root context or directive.", this.file, this.lineNumber, this.charNumber);
				case ParsingContext.Whitespace:
					this.context = ParsingContext.Operator;
					this.builder.Append('+');
					break;
				case ParsingContext.Word:
					this.AddWordToList();
					this.context = ParsingContext.Operator;
					this.builder.Append('+');
					break;
				case ParsingContext.Operator:
					if (last == '+')
					{
						this.builder.Append('+');
						this.AddWordToList();
						this.context = ParsingContext.Whitespace;
					}
					else
					{
						throw new ParseException("Invalid + in operator.", this.file, this.lineNumber, this.charNumber);
					}
					break;
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					this.AddWordToList();
					this.context = ParsingContext.Operator;
					this.builder.Append('+');
					break;
				case ParsingContext.StringLiteral:
					this.builder.Append('+');
					break;
				default:
					break;
			}
		}

		private void ProcessMinusSign(char last, char next)
		{
			switch (this.context)
			{
				case ParsingContext.Root:
				case ParsingContext.Directive:
					throw new ParseException("Invalid minus sign in root context or directive.", this.file, this.lineNumber, this.charNumber);
				case ParsingContext.Whitespace:
					this.builder.Append('-');
					this.context = ParsingContext.Operator;
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					this.AddWordToList();
					this.builder.Append('-');
					this.context = ParsingContext.Operator;
					break;
				case ParsingContext.Operator:
					if (last == '-')
					{
						this.builder.Append('-');
					}
					else
					{
						throw new ParseException("Invalid minus sign in operator.", this.file, this.lineNumber, this.charNumber);
					}
					break;
				case ParsingContext.StringLiteral:
					this.builder.Append('-');
					break;
				default:
					break;
			}
		}

		private void ProcessExclamationMark(char last, char next)
		{
			switch (this.context)
			{
				case ParsingContext.Root:
				case ParsingContext.Directive:
				case ParsingContext.Operator:
					throw new ParseException("Invalid exclamation mark in root context, directive, or operator.", this.file, this.lineNumber, this.charNumber);
				case ParsingContext.Whitespace:
					this.builder.Append('!');
					this.context = ParsingContext.Operator;
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					if (next == '=')
					{
						this.AddWordToList();
						this.builder.Append('!');
						this.context = ParsingContext.Operator;
					}
					else
					{
						throw new ParseException("Invalid exclamation mark in word or numeric literal.", this.file, this.lineNumber, this.charNumber);
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
			switch (this.context)
			{
				case ParsingContext.Root:
				case ParsingContext.Directive:
				case ParsingContext.Operator:
					throw new ParseException("Invalid tilde in root context, directive, or operator.", this.file, this.lineNumber, this.charNumber);
				case ParsingContext.Whitespace:
					this.builder.Append('~');
					this.context = ParsingContext.Operator;
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					if (next == '=')
					{
						this.AddWordToList();
						this.builder.Append('~');
						this.context = ParsingContext.Operator;
					}
					else
					{
						throw new ParseException("Invalid tilde in word or numeric literal.", this.file, this.lineNumber, this.charNumber);
					}
					break;
				case ParsingContext.StringLiteral:
					this.builder.Append('~');
					break;
				default:
					break;
			}
		}

		private void ProcessAsterisk(char last, char next)
		{
			switch (this.context)
			{
				case ParsingContext.Root:
				case ParsingContext.Directive:
					throw new ParseException("Invalid asterisk in root context, or directive.", this.file, this.lineNumber, this.charNumber);
				case ParsingContext.Whitespace:
					this.builder.Append('*');
					this.context = ParsingContext.Operator;
					break;
				case ParsingContext.Operator:
					if (last == '*')
					{
						this.builder.Append('*');
					}
					else
					{
						throw new ParseException("Invalid operator.", this.file, this.lineNumber, this.charNumber);
					}
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					this.AddWordToList();
					this.builder.Append('*');
					this.context = ParsingContext.Operator;
					break;
				case ParsingContext.StringLiteral:
					this.builder.Append('*');
					break;
				default:
					break;
			}
		}

		private void ProcessForwardSlash(char last, char next)
		{
			switch (this.context)
			{
				case ParsingContext.Root:
				case ParsingContext.Directive:
				case ParsingContext.Operator:
					throw new ParseException("Invalid forward slash in root context, directive, or operator.", this.file, this.lineNumber, this.charNumber);
				case ParsingContext.Whitespace:
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					this.AddWordToList();
					this.builder.Append('/');
					this.context = ParsingContext.Operator;
					break;
				case ParsingContext.StringLiteral:
					this.builder.Append('/');
					break;
				default:
					break;
			}
		}

		private void ProcessPercentSign(char last, char next)
		{
			switch (this.context)
			{
				case ParsingContext.Root:
				case ParsingContext.Directive:
				case ParsingContext.Operator:
					throw new ParseException("Invalid percent sign in root context, directive, or operator.", this.file, this.lineNumber, this.charNumber);
				case ParsingContext.Whitespace:
					this.builder.Append('%');
					this.context = ParsingContext.Operator;
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					this.AddWordToList();
					this.builder.Append('%');
					this.context = ParsingContext.Operator;
					break;
				case ParsingContext.StringLiteral:
					this.builder.Append('%');
					break;
				default:
					break;
			}
		}

		private void ProcessLessThanSign(char last, char next)
		{
			switch (this.context)
			{
				case ParsingContext.Root: // where you left off: fix the directive handler for < and > because they're totally valid here.
					throw new ParseException("Invalid less-than sign in root context.", this.file, this.lineNumber, this.charNumber);
				case ParsingContext.Whitespace:
					this.builder.Append('<');
					this.context = ParsingContext.Operator;
					break;
				case ParsingContext.Operator:
					if (last == '<')
					{
						builder.Append('<');
					}
					else
					{
						throw new ParseException("Invalid operator.", this.file, this.lineNumber, this.charNumber);
					}
					break;
				case ParsingContext.Directive:
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					this.AddWordToList();
					this.builder.Append('<');
					this.context = ParsingContext.Operator;
					break;
				case ParsingContext.StringLiteral:
					this.builder.Append('<');
					break;
				default:
					break;
			}
		}
		
		private void ProcessGreaterThanSign(char last, char next)
		{
			switch (this.context)
			{
				case ParsingContext.Root:
					throw new ParseException("Invalid greater-than sign in root context.", this.file, this.lineNumber, this.charNumber);
				case ParsingContext.Whitespace:
					this.builder.Append('>');
					this.context = ParsingContext.Operator;
					break;
				case ParsingContext.Operator:
					if (last.IsOneOfCharacter('-', '>'))
					{
						this.builder.Append('>');
					}
					else
					{
						throw new ParseException("Invalid operator.", this.file, this.lineNumber, this.charNumber);
					}
					break;
				case ParsingContext.Directive:
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					this.AddWordToList();
					this.builder.Append('>');
					this.context = ParsingContext.Operator;
					break;
				case ParsingContext.StringLiteral:
					this.builder.Append('>');
					break;
				default:
					break;
			}
		}

		private void ProcessAmpersand(char last, char next)
		{
			switch (this.context)
			{
				case ParsingContext.Root:
				case ParsingContext.Directive:
					throw new ParseException("Invalid ampersand in root context or operator.", this.file, this.lineNumber, this.charNumber);
				case ParsingContext.Whitespace:
					if (this.builder.Length > 0) { this.AddWordToList(); }
					this.builder.Append('&');
					this.context = ParsingContext.Operator;
					break;
				case ParsingContext.Operator:
					if (last == '&')
					{
						this.builder.Append('&');
					}
					else
					{
						throw new ParseException("Invalid operator.", this.file, this.lineNumber, this.charNumber);
					}
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					this.AddWordToList();
					this.builder.Append('&');
					this.context = ParsingContext.Operator;
					break;
				case ParsingContext.StringLiteral:
					this.builder.Append('&');
					break;
				default:
					break;
			}
		}

		private void ProcessVerticalLine(char last, char next)
		{
			switch (this.context)
			{
				case ParsingContext.Root:
				case ParsingContext.Directive:
					throw new ParseException("Invalid vertical line in root context or operator.", this.file, this.lineNumber, this.charNumber);
				case ParsingContext.Whitespace:
					this.builder.Append('|');
					this.context = ParsingContext.Operator;
					break;
				case ParsingContext.Operator:
					if (last == '|')
					{
						this.builder.Append('|');
					}
					else
					{
						throw new ParseException("Invalid operator.", this.file, this.lineNumber, this.charNumber);
					}
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					this.AddWordToList();
					this.builder.Append('|');
					this.context = ParsingContext.Operator;
					break;
				case ParsingContext.StringLiteral:
					this.builder.Append('|');
					break;
				default:
					break;
			}
		}
		
		private void ProcessCaret(char last, char next)
		{
			switch (this.context)
			{
				case ParsingContext.Root:
				case ParsingContext.Directive:
				case ParsingContext.Operator:
					throw new ParseException("Invalid caret in root context, directive, operator.", this.file, this.lineNumber, this.charNumber);
				case ParsingContext.Whitespace:
					this.builder.Append('^');
					this.context = ParsingContext.Operator;
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					this.AddWordToList();
					this.builder.Append('^');
					this.context = ParsingContext.Operator;
					break;
				case ParsingContext.StringLiteral:
					this.builder.Append('^');
					break;
				default:
					break;
			}
		}

		private void ProcessDot(char last, char next)
		{
			switch (this.context)
			{
				case ParsingContext.Root:
				case ParsingContext.Whitespace:
				case ParsingContext.Directive:
				case ParsingContext.Operator:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					throw new ParseException("Invalid dot in root context, whitespace context, directive, operator, or numeric literal fraction/suffix.", this.file, this.lineNumber, this.charNumber);
				case ParsingContext.Word:
					this.AddWordToList();
					this.builder.Append('.');
					this.context = ParsingContext.Operator;
					break;
				case ParsingContext.NumericLiteral:
					this.builder.Append('.');
					this.context = ParsingContext.NumericLiteralFraction;
					break;
				case ParsingContext.StringLiteral:
					this.builder.Append('.');
					break;
				default:
					break;
			}
		}

		private void ProcessQuestionMark(char last, char next)
		{
			switch (this.context)
			{
				case ParsingContext.Root:
				case ParsingContext.Directive:
				case ParsingContext.Operator:
					throw new ParseException("Invalid question mark in root context, directive, or operator.", this.file, this.lineNumber, this.charNumber);
				case ParsingContext.Whitespace:
					this.builder.Append('?');
					this.context = ParsingContext.Operator;
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					this.AddWordToList();
					this.builder.Append('?');
					this.context = ParsingContext.Operator;
					break;
				case ParsingContext.StringLiteral:
					this.builder.Append('?');
					break;
				default:
					break;
			}
		}

		private void ProcessColon(char last, char next)
		{
			switch (this.context)
			{
				case ParsingContext.Root:
				case ParsingContext.Directive:
				case ParsingContext.Operator:
					throw new ParseException("Invalid colon in root context, directive, or operator.", this.file, this.lineNumber, this.charNumber);
				case ParsingContext.Whitespace:
					this.builder.Append(':');
					this.context = ParsingContext.Operator;
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					this.AddWordToList();
					this.builder.Append(':');
					this.context = ParsingContext.Operator;
					break;
				case ParsingContext.StringLiteral:
					this.builder.Append(':');
					break;
				default:
					break;
			}
		}

		private void ProcessEqualsSign(char last, char next)
		{
			switch (this.context)
			{
				case ParsingContext.Root:
				case ParsingContext.Directive:
					throw new ParseException("Invalid equals sign in root context or directive.", this.file, this.lineNumber, this.charNumber);
				case ParsingContext.Whitespace:
					this.builder.Append('=');
					this.context = ParsingContext.Operator;
					break;
				case ParsingContext.Operator:
					if (last.IsOneOfCharacter('<', '>', '+', '-', '*', '/', '%', '&', '|', '^', '!', '='))
					{
						builder.Append('=');
						this.context = ParsingContext.Whitespace;
					}
					else
					{
						throw new ParseException("Invalid operator.", this.file, this.lineNumber, this.charNumber);
					}
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					this.AddWordToList();
					this.builder.Append('=');
					this.context = ParsingContext.Operator;
					break;
				case ParsingContext.StringLiteral:
					this.builder.Append('=');
					break;
				default:
					break;
			}
		}

		private void ProcessBackslash(char last, char next)
		{
			switch (this.context)
			{
				case ParsingContext.Root:
				case ParsingContext.Whitespace:
				case ParsingContext.Directive:
				case ParsingContext.Word:
				case ParsingContext.Operator:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					throw new ParseException("Invalid backslash in most contexts.", this.file, this.lineNumber, this.charNumber);
				case ParsingContext.StringLiteral:
					this.builder.Append('\\');
					break;
				default:
					break;
			}
		}

		private void ProcessSemicolon(char last, char next)
		{
			switch (this.context)
			{
				case ParsingContext.Root:
				case ParsingContext.Whitespace:
				case ParsingContext.Word:
				case ParsingContext.Operator:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					if (this.builder.Length > 0) { this.AddWordToList(); }
					this.builder.Append(';');
					this.AddWordToList();
					this.context = ParsingContext.Whitespace;
					break;
				case ParsingContext.Directive:
					throw new ParseException("Invalid semicolon in directive.", this.file, this.lineNumber, this.charNumber);
				case ParsingContext.StringLiteral:
					this.builder.Append(';');
					break;
				default:
					break;
			}
		}

		private void ProcessComma(char last, char next)
		{
			switch (this.context)
			{
				case ParsingContext.Root:
				case ParsingContext.Directive:
				case ParsingContext.Operator:
					throw new ParseException("Invalid comma in root context, directive, operator.", this.file, this.lineNumber, this.charNumber);
				case ParsingContext.Whitespace:
					this.builder.Append(',');
					this.AddWordToList();
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					this.AddWordToList();
					this.builder.Append(',');
					this.AddWordToList();
					this.context = ParsingContext.Whitespace;
					break;
				case ParsingContext.StringLiteral:
					this.builder.Append(',');
					break;
				default:
					break;
			}
		}

		private void ProcessNumberSign(char last, char next)
		{
			switch (this.context)
			{
				case ParsingContext.Root:
				case ParsingContext.Whitespace:
					this.builder.Append('#');
					this.context = ParsingContext.Directive;
					this.withinDirective = true;
					break;
				case ParsingContext.Directive:
				case ParsingContext.Word:
				case ParsingContext.Operator:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.NumericLiteralSuffix:
					throw new ParseException("Invalid number sign in directive, word, operator, or numeric literal.", this.file, this.lineNumber, this.charNumber);
				case ParsingContext.StringLiteral:
					this.builder.Append('#');
					break;
				default:
					break;
			}
		}

		private void ProcessNumber(char current, char last, char next)
		{
			switch (this.context)
			{
				case ParsingContext.Root:
				case ParsingContext.Directive:
				case ParsingContext.NumericLiteralSuffix:
					throw new ParseException("Invalid number in root context, directive, or numeric literal suffix.", this.file, this.lineNumber, this.charNumber);
				case ParsingContext.Whitespace:
				case ParsingContext.Operator:
					if (this.builder.Length > 0)
					{
						this.AddWordToList();
					}
					this.builder.Append(current);
					this.context = ParsingContext.NumericLiteral;
					break;
				case ParsingContext.Word:
				case ParsingContext.NumericLiteral:
				case ParsingContext.NumericLiteralFraction:
				case ParsingContext.StringLiteral:
					this.builder.Append(current);
					break;
				default:
					break;
			}
		}

		private void AddWordToList()
		{
			this.wordList.Add(this.builder.ToString());
			this.builder.Clear();
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
