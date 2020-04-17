using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Cix.Errors;

namespace Cix.Parser
{
	/// <summary>
	/// Given a list of words from a source file, this class assigns each a token type indicating what it is.
	/// </summary>
	internal sealed class Tokenizer
	{
		private static readonly string[] BinaryOperators =
		{ ".", "->", "*", "/", "%", "+", "-", "<<", ">>", "<", "<=", ">", ">=",
		  "==", "!=", "&", "|", "^", "&&", "||", "=", "+=", "-=", "*=", "/=",
		  "%=", "<<=", ">>=", "<<=", "&=", "|=", "^=" };

		private static readonly string[] ReservedKeywords =
		{
			"break", "byte", "case", "const", "continue", "default", "do", "double", "else", "float", "for",
			"global", "goto", "if", "int", "long", "return", "sbyte", "short", "sizeof", "struct", "switch",
			"uint", "ulong", "ushort", "void", "while"
		};

		private readonly Dictionary<string, TokenType> unambiguousBinaryOperatorTokens = new Dictionary<string,TokenType>()
		{
				{"/", TokenType.OpDivide},
				{"%", TokenType.OpModulusDivide},
  				{"<<", TokenType.OpShiftLeft},
				{">>", TokenType.OpShiftRight},
				{"<", TokenType.OpLessThan},
				{"<=", TokenType.OpLessThanOrEqualTo},
				{">", TokenType.OpGreaterThan},
				{">=", TokenType.OpGreaterThanOrEqualTo},
				{"==", TokenType.OpEqualTo},
				{"!=", TokenType.OpNotEqualTo},
				{"|", TokenType.OpBitwiseOR},
				{"^", TokenType.OpBitwiseXOR},
				{"&&", TokenType.OpLogicalAND},
				{"||", TokenType.OpLogicalOR},
				{"=", TokenType.OpAssign},
				{"+=", TokenType.OpAddAssign},
				{"-=", TokenType.OpSubtractAssign},
				{"*=", TokenType.OpMultiplyAssign},
				{"/=", TokenType.OpDivideAssign},
				{"%=", TokenType.OpModulusDivideAssign},
				{">>=", TokenType.OpShiftRightAssign},
				{"<<=", TokenType.OpShiftLeftAssign},
				{"&=", TokenType.OpBitwiseANDAssign},
				{"|=", TokenType.OpBitwiseORAssign},
				{"^=", TokenType.OpBitwiseXORAssign}
		};

		private readonly IErrorListProvider errorList;
		private readonly List<Token> tokenList = new List<Token>();

		public Tokenizer(IErrorListProvider errorList) => this.errorList = errorList;

		/// <summary>
		/// Associates words in a lexed Cix file with the kind of token they are.
		/// </summary>
		/// <param name="words">A list of strings containing each word of the Cix file.</param>
		/// <returns>A list of tokens made from the words and their token types.</returns>
		public IList<Token> Tokenize(IList<LexedWord> words)
		{
			int wordCount = words.Count;
			for (int i = 0; i < wordCount; i++)
			{
				LexedWord current = words[i];
				LexedWord last = (i > 0) ? words[i - 1] : null;
				LexedWord next = (i < wordCount - 1) ? words[i + 1] : null;

				if (current.Text.IsIdentifier()) { AddToken(TokenType.Identifier, current); }
				else if (current.Text.IsBasicNumericLiteral()) { AddToken(TokenType.BasicNumericLiteral, current); }
				else if (current.Text.IsBasicHexadecimalLiteral())
				{
					AddToken(TokenType.BasicHexadecimalLiteral, current);
				}
				else if (current.Text.IsSuffixedNumericLiteral()) { AddToken(TokenType.SuffixedNumericLiteral, current); }
				else if (current.Text.IsSuffixedHexadecimalLiteral()) { AddToken(TokenType.SuffixedHexadecimalLiteral, current); }
				else if (current.Text.IsFloatingLiteralWithDecimal())
				{
					AddToken(TokenType.FloatingLiteralWithDecimal, current);
				}
				else if (current.Text.IsSuffixedFloatingLiteral())
				{
					AddToken(TokenType.SuffixedFloatingLiteral, current);
				}
				else if (current.Text.IsFloatingLiteralWithExponent()) { AddToken(TokenType.FloatingLiteralWithExponent, current); }
				else
				{
					switch (current.Text)
					{
						case ";": AddToken(TokenType.Semicolon, current); break;
						case "{": AddToken(TokenType.OpenScope, current); break;
						case "}": AddToken(TokenType.CloseScope, current); break;
						case "(": AddToken(TokenType.OpenParen, current); break;
						case ")": AddToken(TokenType.CloseParen, current); break;
						case "[": AddToken(TokenType.OpenBracket, current); break;
						case "]": AddToken(TokenType.CloseBracket, current); break;
						case ",": AddToken(TokenType.Comma, current); break;
						case "+": ProcessPlusSign(current, last, next); break;
						case "-": ProcessMinusSign(current, last, next); break;
						case "!": ProcessExclamationMark(current, last, next); break;
						case "~": ProcessTilde(current, last, next); break;
						case "++": ProcessDoublePlus(current, last, next); break;
						case "--": ProcessDoubleMinus(current, last, next); break;
						case "*": ProcessAsterisk(current, last, next); break;
						case "&": ProcessAmpersand(current, last, next); break;
						case ".": ProcessDot(current, last, next); break;
						case "->": ProcessArrow(current, last, next); break;
						default:
							if (current.Text.IsOneOfString("/", "%", "<<", ">>", "<", "<=", ">", ">=", "==", "!=", "|", "^", "&&", "||", "=", "+=", "-=", "*=", "/=", "%=", ">>=", "<<=", "&=", "|=", "^=", "?"))
							{
								ProcessOperator(current, last, next);
							}
							else if (ReservedKeywords.Contains(current.Text.ToLower()))
							{
								ProcessReservedWord(current);
							}
							else if (current.Text.Count(c => c == '*') == current.Text.Length && current.Text.Length > 1)
							{
								// Two or more asterisks, but only asterisks
								ProcessMultipleAsterisks(current, last, next);
							}
							else if (current.Text.StartsWith("\"") && current.Text.EndsWith("\""))
							{
								ProcessStringLiteral(current);
							}
							else
							{
								errorList.AddError(ErrorSource.Tokenizer, 1, $"The word {current.Text} cannot be parsed.",
									current.FilePath, current.LineNumber);
							}
							break;
					}
				}
			}

			return tokenList;
		}

		private void ProcessReservedWord(LexedWord current)
		{
			string tokenEnumValue = string.Concat("Key", current.Text);
			if (!Enum.TryParse(tokenEnumValue, true, out TokenType tokenType))
			{
				throw new InvalidOperationException(
					"This code path should be unreachable; seeing this means there's a bug.");
			}
			AddToken(tokenType, current);
		}

		private void ProcessPlusSign(LexedWord current, LexedWord last, LexedWord next)
		{
			// A plus sign can be either unary prefix identity or binary addition.
			// OpIdentity (+) requires: Preceding semicolon, closescope, openparen, comma, binary/ternary operator, or one of { - ! ~ }.
			//	Succeeding identifier, openparen, or one of { - ! ~ * & }.
			// OpAdd (+): Preceding identifier, closeparen, closebracket, one of { ++ -- }.
			//	Succeeding identifier, openparen, or one of { - ! ~ -- & * }.

			string lastText = last.Text;
			string nextText = next.Text;

			if (lastText == ";" || lastText == "}" || lastText == "(" || lastText == "," ||
			    IsBinaryTernaryOperator(lastText) || lastText.IsOneOfString("-", "!", "~"))
			{
				if (nextText.IsIdentifierOrLiteral() || nextText.IsOneOfString("(", "-", "!", "~", "*", "&"))
				{
					AddToken(TokenType.OpIdentity, current);
				}
				else
				{
					errorList.AddError(ErrorSource.Tokenizer, 2,
						$"The operator {current.Text} cannot appear in this location.", current.FilePath,
						current.LineNumber);
				}
			}
			else if (lastText.IsIdentifierOrLiteral() || lastText == ")" || lastText == "]" || lastText.IsOneOfString("++", "--"))
			{
				if (nextText.IsIdentifierOrLiteral() || nextText.IsOneOfString("(", "-", "!", "~", "--", "&", "*"))
				{
					AddToken(TokenType.OpAdd, current);
				}
				else
				{
					errorList.AddError(ErrorSource.Tokenizer, 2,
						$"The operator {current.Text} cannot appear in this location.", current.FilePath,
						current.LineNumber);
				}
			}
			else
			{
				errorList.AddError(ErrorSource.Tokenizer, 2,
					$"The operator {current.Text} cannot appear in this location.", current.FilePath,
					current.LineNumber);
			}
		}

		private void ProcessMinusSign(LexedWord current, LexedWord last, LexedWord next)
		{
			// A minus sign is either a unary prefix inverse operator, or a binary subtraction operator.
			// OpInverse (-) requires: Preceding semicolon, closescope, openparen, comma, binary/ternary operator, or one of { + ! ~ }.
			//	Succeeding identifier, openparen, or one of { + ! ~ * & }.
			// OpSubtract (-): Preceding identifier, closeparen, closebracket, one of { ++ -- }.
			//	Succeeding identifier, openparen, or one of { + ! ~ ++ & * }.

			string lastText = last.Text;
			string nextText = next.Text;

			if (lastText.IsOneOfString(";", "}", "(", ",", "+", "!", "~") || IsBinaryTernaryOperator(lastText))
			{
				if (nextText.IsIdentifierOrLiteral() || nextText.IsOneOfString("(", "+", "!", "~", "*", "&"))
				{
					AddToken(TokenType.OpInverse, current);
				}
				else
				{
					errorList.AddError(ErrorSource.Tokenizer, 2,
						$"The operator {current.Text} cannot appear in this location.", current.FilePath,
						current.LineNumber);
				}
			}
			else if (lastText.IsIdentifierOrLiteral() || lastText.IsOneOfString(")", "]", "++", "--"))
			{
				if (nextText.IsIdentifierOrLiteral() || nextText.IsOneOfString("(", "+", "!", "~", "++", "&", "*"))
				{
					AddToken(TokenType.OpSubtract, current);
				}
				else
				{
					errorList.AddError(ErrorSource.Tokenizer, 2,
						$"The operator {current.Text} cannot appear in this location.", current.FilePath,
						current.LineNumber);
				}
			}
			else
			{
				errorList.AddError(ErrorSource.Tokenizer, 2,
					$"The operator {current.Text} cannot appear in this location.", current.FilePath,
					current.LineNumber);
			}
		}

		private void ProcessExclamationMark(LexedWord current, LexedWord last, LexedWord next)
		{
			// An exclamation mark is a unary prefix logical NOT.
			// OpLogicalNOT (!) requires: Preceding semicolon, closescope, openparen, comma, binary/ternary operator, or one of { + - ! ~ }.
			//	Succeeding identifier, openparen, or one of { + - ! ~ * & }.

			string lastText = last.Text;
			string nextText = next.Text;

			if (lastText.IsOneOfString(";", "}", "(", ",", "+", "-", "!", "~") || IsBinaryTernaryOperator(lastText))
			{
				if (nextText.IsIdentifierOrLiteral() || nextText.IsOneOfString("(", "+", "-", "!", "~", "*", "&"))
				{
					AddToken(TokenType.OpLogicalNOT, current);
				}
				else
				{
					errorList.AddError(ErrorSource.Tokenizer, 2,
						$"The operator {current.Text} cannot appear in this location.", current.FilePath,
						current.LineNumber);
				}
			}
			else
			{
				errorList.AddError(ErrorSource.Tokenizer, 2,
					$"The operator {current.Text} cannot appear in this location.", current.FilePath,
					current.LineNumber);
			}
		}

		private void ProcessTilde(LexedWord current, LexedWord last, LexedWord next)
		{
			// A tilde is a unary prefix bitwise NOT operator.
			// OpBitwiseNOT (~) requires: Preceding semicolon, closescope, openparen, comma, binary/ternary operator, or one of { + - ! ~ }.
			//	Succeeding identifier, openparen, or one of { + - ! ~ * & }.

			string lastText = last.Text;
			string nextText = next.Text;

			if (lastText.IsOneOfString(";", "}", "(", ",", "+", "-", "!", "~") || IsBinaryTernaryOperator(lastText))
			{
				if (nextText.IsIdentifierOrLiteral() || nextText.IsOneOfString("(", "+", "-", "!", "~", "*", "&"))
				{
					AddToken(TokenType.OpBitwiseAND, current);
				}
				else
				{
					errorList.AddError(ErrorSource.Tokenizer, 2,
						$"The operator {current.Text} cannot appear in this location.", current.FilePath,
						current.LineNumber);
				}
			}
			else
			{
				errorList.AddError(ErrorSource.Tokenizer, 2,
					$"The operator {current.Text} cannot appear in this location.", current.FilePath,
					current.LineNumber);
			}
		}

		private void ProcessDoublePlus(LexedWord current, LexedWord last, LexedWord next)
		{
			// A double plus sign is either a preincrement or postincrement.
			// OpPreincrement (++) requires: Preceding semicolon, closescope, openparen, comma, or binary/ternary operator.
			//	Succeeding identifier.
			// OpPostincrement (++) requires: Preceding identifier or closebracket.
			//	Succeeding binary/ternary operator or semicolon.

			string lastText = last.Text;
			string nextText = next.Text;

			if (lastText.IsOneOfString(";", "}", "(", ",") || IsBinaryTernaryOperator(lastText))
			{
				if (nextText.IsIdentifier())
				{
					AddToken(TokenType.OpPreincrement, current);
				}
				else
				{
					errorList.AddError(ErrorSource.Tokenizer, 2,
						$"The operator {current.Text} cannot appear in this location.", current.FilePath,
						current.LineNumber);
				}
			}
			else if (lastText.IsIdentifier() || lastText == "}")
			{
				if (IsBinaryTernaryOperator(nextText) || nextText == ";")
				{
					AddToken(TokenType.OpPostincrement, current);
				}
				else
				{
					errorList.AddError(ErrorSource.Tokenizer, 2,
						$"The operator {current.Text} cannot appear in this location.", current.FilePath,
						current.LineNumber);
				}
			}
			else
			{
				errorList.AddError(ErrorSource.Tokenizer, 2,
					$"The operator {current.Text} cannot appear in this location.", current.FilePath,
					current.LineNumber);
			}
		}

		private void ProcessDoubleMinus(LexedWord current, LexedWord last, LexedWord next)
		{
			// A double minus sign is either a predecrement or postdecrement.
			// OpPredecrement (--) requires: Preceding semicolon, closescope, openparen, comma, or binary/ternary operator.
			//	Succeeding identifier.
			// OpPostdecrement (--) requires: Preceding identifier or closebracket.
			//	Succeeding binary/ternary operator or semicolon.

			string lastText = last.Text;
			string nextText = next.Text;

			if (lastText.IsOneOfString(";", "}", "(", ",") || IsBinaryTernaryOperator(lastText))
			{
				if (nextText.IsIdentifier())
				{
					AddToken(TokenType.OpPredecrement, current);
				}
				else
				{
					errorList.AddError(ErrorSource.Tokenizer, 2,
						$"The operator {current.Text} cannot appear in this location.", current.FilePath,
						current.LineNumber);
				}
			}
			else if (lastText.IsIdentifier() || lastText == "}")
			{
				if (IsBinaryTernaryOperator(nextText) || nextText == ";")
				{
					AddToken(TokenType.OpPostdecrement, current);
				}
				else
				{
					errorList.AddError(ErrorSource.Tokenizer, 2,
						$"The operator {current.Text} cannot appear in this location.", current.FilePath,
						current.LineNumber);
				}
			}
			else
			{
				errorList.AddError(ErrorSource.Tokenizer, 2,
					$"The operator {current.Text} cannot appear in this location.", current.FilePath,
					current.LineNumber);
			}
		}

		private void ProcessAsterisk(LexedWord current, LexedWord last, LexedWord next)
		{
			// An asterisk is either a multiplication operator, pointer dereference, or pointer type declarator.
			// At this point, we cannot distinguish between the multiplication operator or pointer type declarator, so we will add it is Indeterminate.
			// OpPointerDerefence (*): Preceding semicolon, closescope, openparen, comma, binary/ternary operator, or one of { + - ! ~ & * }.
			//	Succeeding identifier, openparen or *.
			// OpMultiply (*): Preceding identifier, closeparen, closebracket, one of { ++ -- }.
			//	Succeeding identifier, one of { + - ! ~ ++ -- & * }.
			// Pointer data type: Preceding identifier.

			string lastText = last.Text;
			string nextText = next.Text;

			if (lastText.IsOneOfString(";", "}", "(", ",", "+", "-", "!", "~", "&", "*") || IsBinaryTernaryOperator(lastText))
			{
				if (nextText.IsIdentifier() || nextText.IsOneOfString("(", ")", "*"))
				{
					AddToken(TokenType.OpPointerDereference, current);
				}
				else
				{
					errorList.AddError(ErrorSource.Tokenizer, 2,
						$"The operator {current.Text} cannot appear in this location.", current.FilePath,
						current.LineNumber);
				}
			}
			else if (lastText.IsIdentifierOrLiteral(true))
			{
				if (lastText.IsOneOfString(")", "]", "++", "--") && (nextText.IsIdentifier() || nextText.IsOneOfString("+", "-", "!", "~", "++", "--", "&", "*")))
				{
					AddToken(TokenType.OpMultiply, current);
				}
				else
				{
					AddToken(TokenType.Indeterminate, current);
				}
			}
			else
			{
				errorList.AddError(ErrorSource.Tokenizer, 2,
					$"The operator {current.Text} cannot appear in this location.", current.FilePath,
					current.LineNumber);
			}
		}

		private void ProcessAmpersand(LexedWord current, LexedWord last, LexedWord next)
		{
			// The ampersand indicates either a variable dereference or a bitwise AND operator.
			// OpVariableDerefence (&): Preceding semicolon, closescope, openparen, comma, binary/ternary operator, or one of { + - ! ~ & }.
			//	Succeeding identifier, openparen, or one of { & * }.
			// OpBitwiseAnd (&): Preceding identifier, closeparen, closebracket, one of { ++ -- }.
			//	Succeeding identifier, openparen, one of { + - ! ~ ++ -- * }.

			string lastText = last.Text;
			string nextText = next.Text;

			if (lastText.IsOneOfString(";", "}", "(", ",", "+", "-", "!", "~", "&") || IsBinaryTernaryOperator(lastText))
			{
				if (nextText.IsIdentifierOrLiteral() || nextText.IsOneOfString("(", "*", "&"))
				{
					AddToken(TokenType.OpVariableDereference, current);
				}
				else
				{
					errorList.AddError(ErrorSource.Tokenizer, 2,
						$"The operator {current.Text} cannot appear in this location.", current.FilePath,
						current.LineNumber);
				}
			}
			else if (lastText.IsIdentifierOrLiteral() || lastText.IsOneOfString(")", "]", "++", "--"))
			{
				if (nextText.IsIdentifierOrLiteral() || nextText.IsOneOfString("(", "+", "-", "!", "~", "++", "--", "*"))
				{
					AddToken(TokenType.OpBitwiseAND, current);
				}
				else
				{
					errorList.AddError(ErrorSource.Tokenizer, 2,
						$"The operator {current.Text} cannot appear in this location.", current.FilePath,
						current.LineNumber);
				}
			}
			else
			{
				errorList.AddError(ErrorSource.Tokenizer, 2,
					$"The operator {current.Text} cannot appear in this location.", current.FilePath,
					current.LineNumber);
			}
		}

		private void ProcessDot(LexedWord current, LexedWord last, LexedWord next)
		{
			// A dot by itself indicates a member access operator.
			// OpMemberAccess (.): Preceding identifier, closeparen, closebracket.
			//	Succeeding identifier.

			string lastText = last.Text;
			string nextText = next.Text;

			if (lastText.IsIdentifier() || lastText.IsOneOfString(")", "]"))
			{
				if (nextText.IsIdentifier())
				{
					AddToken(TokenType.OpMemberAccess, current);
				}
				else
				{
					errorList.AddError(ErrorSource.Tokenizer, 2,
						$"The operator {current.Text} cannot appear in this location.", current.FilePath,
						current.LineNumber);
				}
			}
			else
			{
				errorList.AddError(ErrorSource.Tokenizer, 2,
					$"The operator {current.Text} cannot appear in this location.", current.FilePath,
					current.LineNumber);
			}
		}

		private void ProcessArrow(LexedWord current, LexedWord last, LexedWord next)
		{
			// An arrow is a pointer access operator.
			// OpPointerAccess (->): Preceding identifier, closeparen, closebracket.
			//	Succeeding identifier.

			string lastText = last.Text;
			string nextText = next.Text;

			if (lastText.IsIdentifier() || lastText.IsOneOfString(")", "]"))
			{
				if (nextText.IsIdentifier())
				{
					AddToken(TokenType.OpPointerMemberAccess, current);
				}
				else
				{
					errorList.AddError(ErrorSource.Tokenizer, 2,
						$"The operator {current.Text} cannot appear in this location.", current.FilePath,
						current.LineNumber);
				}
			}
			else
			{
				errorList.AddError(ErrorSource.Tokenizer, 2,
					$"The operator {current.Text} cannot appear in this location.", current.FilePath,
					current.LineNumber);
			}
		}

		private void ProcessOperator(LexedWord current, LexedWord last, LexedWord next)
		{
			// All Other Binary Operators and the Ternary Aftercondition (?):
			//	Preceding identifier, closeparen, closebracket, one of { ++ -- }.
			//	Succeeding identifier, openparen, or one of { + - ! ~ ++ -- & * }.

			string lastText = last.Text;
			string nextText = next.Text;

			if (lastText.IsIdentifierOrLiteral() || lastText.IsOneOfString(")", "]", "++", "--"))
			{
				if (nextText.IsIdentifierOrLiteral() || nextText.IsOneOfString("+", "-", "!", "~", "++", "--", "&", "*", "("))
				{
					AddToken(unambiguousBinaryOperatorTokens[current.Text], current);
				}
				else
				{
					errorList.AddError(ErrorSource.Tokenizer, 2,
						$"The operator {current.Text} cannot appear in this location.", current.FilePath,
						current.LineNumber);
				}
			}
			else
			{
				errorList.AddError(ErrorSource.Tokenizer, 3,
					$"The word {current.Text} cannot appear in the place of an operator.", current.FilePath,
					current.LineNumber);
			}
		}

		private void ProcessMultipleAsterisks(LexedWord current, LexedWord last, LexedWord next)
		{
			// Two or more asterisks forming a single word denote a pointer type.
			// Preceded by: Identifier.
			// Succeeded by: Identifier, closeparen.

			string lastText = last.Text;
			string nextText = next.Text;

			if (lastText.IsIdentifier(allowReservedWords: true))
			{
				if (nextText.IsIdentifier() || nextText == ")")
				{
					AppendToken(current);
				}
				else
				{
					errorList.AddError(ErrorSource.Tokenizer, 3,
						$"The type {current.Text} must be followed by a name or a close parenthesis.", current.FilePath,
						current.LineNumber);
				}
			}
			else
			{
				errorList.AddError(ErrorSource.Tokenizer, 3,
					$"The type {current.Text} must be followed by a name or a close parenthesis.", current.FilePath,
					current.LineNumber);
			}
		}

		private void ProcessStringLiteral(LexedWord current)
		{
			// 1. Remove the starting and ending quotation marks.
			// 2. Process every escape character
			
			// Code from IronAssembler, used with permission.
			StringBuilder result = new StringBuilder(current.Text.Length - 2);
			string literal = current.Text;
			literal = literal.Substring(1, literal.Length - 2);

			for (int i = 0; i < literal.Length; i++)
			{
				char currentChar = literal[i];

				if (currentChar == '\'' || currentChar == '\"')
				{
					// These characters need to be escaped, so we'll throw an error.
					errorList.AddError(ErrorSource.Tokenizer, 6,
						"A string literal cannot have unescaped single or double quotes.", 
						current.FilePath, current.LineNumber);
				}
				else if (currentChar == '\\')
				{
					if (i == literal.Length - 1)
					{
						errorList.AddError(ErrorSource.Tokenizer, 7,
							"An escaping backslash cannot be at the end of a string literal.",
							current.FilePath, current.LineNumber);
						return;
					}

					char next = literal[i + 1];

					if (next == 'u' || next == 'U')
					{
						int codePointLength = (next == 'u') ? 4 : 8;
						if (i + 1 + codePointLength > literal.Length - 1)
						{
							errorList.AddError(ErrorSource.Tokenizer, 8,
								"An string literal ends in a Unicode escape sequence, but there aren't enough hexadecimal digits to determine the codepoint.",
								current.FilePath, current.LineNumber);
							return;
						}

						string codePointString = literal.Substring(i + 2, codePointLength);
						if (!int.TryParse(codePointString, NumberStyles.HexNumber,
							CultureInfo.CurrentCulture, out int codePoint))
						{
							errorList.AddError(ErrorSource.Tokenizer, 9,
								$"The sequence {codePointString} is not a valid Unicode code point.",
								current.FilePath, current.LineNumber);
							return;
						}

						result.Append(char.ConvertFromUtf32(codePoint));
						i += 1 + codePointLength;
					}

					switch (next)
					{
						case '\'':
							result.Append('\'');
							break;
						case '\"':
							result.Append('\"');
							break;
						case '\\':
							result.Append('\\');
							break;
						case '0':
							result.Append('\0');
							break;
						case 'a':
							result.Append('\a');
							break;
						case 'b':
							result.Append('\b');
							break;
						case 'f':
							result.Append('\f');
							break;
						case 'n':
							result.Append('\n');
							break;
						case 'r':
							result.Append('\r');
							break;
						case 't':
							result.Append('\t');
							break;
						case 'v':
							result.Append('\v');
							break;
						default:
							errorList.AddError(ErrorSource.Tokenizer, 10,
								$"The escape sequence \\{next} is not valid.",
								current.FilePath, current.LineNumber);
							return;
					}

					i++;
				}
				else { result.Append(currentChar); }

				AddToken(TokenType.StringLiteral,
					new LexedWord(current.FilePath, current.LineNumber, current.WordNumber, result.ToString()));
			}
		}

		private void AddToken(TokenType type, LexedWord word)
		{
			tokenList.Add(new Token(type, word));
		}

		private void AppendToken(LexedWord word)
		{
			Token oldToken = tokenList[tokenList.Count - 1];
			var newToken = new Token(oldToken.Type,
				new LexedWord(oldToken.FilePath, oldToken.LineNumber, oldToken.WordNumber,
					oldToken.Text + word.Text));
			tokenList[tokenList.Count - 1] = newToken;
		}

		private static bool IsBinaryTernaryOperator(string word) => BinaryOperators.Contains(word);
	}

	/// <summary>
	/// Enumerates the kinds of tokens in a Cix file.
	/// </summary>
	public enum TokenType
	{
		Invalid,
		Identifier,
		BasicNumericLiteral,
		BasicHexadecimalLiteral,
		SuffixedNumericLiteral,
		SuffixedHexadecimalLiteral,
		FloatingLiteralWithDecimal,
		SuffixedFloatingLiteral,
		FloatingLiteralWithExponent,
		Directive,
		DirectiveEnd,
		Semicolon,
		Comma,
		OpenScope,
		CloseScope,
		OpenParen,
		CloseParen,
		OpenBracket,
		CloseBracket,
		OpIdentity,
		OpInverse,
		OpLogicalNOT,
		OpBitwiseNOT,
		OpPreincrement,
		OpPostincrement,
		OpPredecrement,
		OpPostdecrement,
		OpPointerDereference,
		OpVariableDereference,
		OpMemberAccess,
		OpPointerMemberAccess,
		OpMultiply,
		OpDivide,
		OpModulusDivide,
		OpAdd,
		OpSubtract,
		OpShiftLeft,
		OpShiftRight,
		OpLessThan,
		OpLessThanOrEqualTo,
		OpGreaterThan,
		OpGreaterThanOrEqualTo,
		OpEqualTo,
		OpNotEqualTo,
		OpBitwiseAND,
		OpBitwiseOR,
		OpBitwiseXOR,
		OpLogicalAND,
		OpLogicalOR,
		OpAssign,
		OpAddAssign,
		OpSubtractAssign,
		OpMultiplyAssign,
		OpDivideAssign,
		OpModulusDivideAssign,
		OpShiftRightAssign,
		OpShiftLeftAssign,
		OpBitwiseANDAssign,
		OpBitwiseORAssign,
		OpBitwiseXORAssign,
		KeyBreak,
		KeyByte,
		KeyCase,
		KeyConst,
		KeyContinue,
		KeyDefault,
		KeyDo,
		KeyDouble,
		KeyElse,
		KeyFloat,
		KeyFor,
		KeyGlobal,
		KeyGoto,
		KeyIf,
		KeyInt,
		KeyLong,
		KeyReturn,
		KeySByte,
		KeyShort,
		KeySizeof,
		KeyStruct,
		KeySwitch,
		KeyUInt,
		KeyULong,
		KeyUShort,
		KeyVoid,
		KeyWhile,

		/// <summary>
		/// An asterisk whose purpose cannot be determined at this stage of compilation.
		/// </summary>
		/// <remarks>
		/// An asterisk between two identifiers is either multiplication (x * y) or
		/// the declaration of a pointer to a value (int* y). It's the latter only if the identifier
		/// to the left of the asterisk is a type name.
		/// </remarks>
		Indeterminate,
		StringLiteral,
		IntrinsicHWCall,
		IntrinsicFuncPtr
	}

	/// <summary>
	/// Represents a word and the type of token it is.
	/// </summary>
	public sealed class Token
	{
		public string FilePath { get; }
		public int LineNumber { get; }
		public int WordNumber { get; }

		/// <summary>
		/// Gets the type of token this token is.
		/// </summary>
		public TokenType Type { get; }

		/// <summary>
		/// Gets the word of this token.
		/// </summary>
		public string Text { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Token"/> class.
		/// </summary>
		/// <param name="type">The type of token this token is.</param>
		/// <param name="word">The word of this token.</param>
		public Token(TokenType type, LexedWord word)
		{
			Type = type;
			FilePath = word.FilePath;
			LineNumber = word.LineNumber;
			WordNumber = word.WordNumber;
			Text = word.Text;
		}

		/// <summary>
		/// Returns a string representation of this token.
		/// </summary>
		/// <returns>A string representation of this token</returns>
		public override string ToString() => $"{LineNumber}:{WordNumber} \"{Text}\" ({Type})";
	}
}
