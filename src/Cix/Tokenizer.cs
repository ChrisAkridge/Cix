using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cix.Exceptions;

namespace Cix
{
	/// <summary>
	/// Given a list of words from a source file, this class assigns each a token type indicating what it is.
	/// </summary>
	public sealed class Tokenizer
	{
		private static readonly string[] UnaryPrefixOperators = { "+", "-", "!", "~", "++", "--", "*", "&" };
		private static readonly string[] UnaryPostfixOperators = { "++", "--" };

		// TODO: remove ternary operator
		private static readonly string[] BinaryTernaryOperators =
		{ ".", "->", "*", "/", "%", "+", "-", "<<", ">>", "<", "<=", ">", ">=",
		  "==", "!=", "&", "|", "^", "&&", "||", "=", "+=", "-=", "*=", "/=",
		  "%=", "<<=", ">>=", "<<=", "&=", "|=", "^=", "?", ":" };

		// TODO: replace char with byte, schar with sbyte
		private static readonly string[] ReservedKeywords =
		{ "break", "case", "char", "const", "continue", "default", "do",
		  "double", "else", "float", "for", "goto", "if", "int", "long",
		  "return", "schar", "short", "sizeof", "struct", "switch", "uint",
		  "ulong", "ushort", "void", "while" };

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
				{"^=", TokenType.OpBitwiseXORAssign},
				{"?", TokenType.OpTernaryAfterCondition}
			};

		private readonly List<Token> tokenList = new List<Token>();

		/// <summary>
		/// Associates words in a lexed Cix file with the kind of token they are.
		/// </summary>
		/// <param name="words">A list of strings containing each word of the Cix file.</param>
		/// <returns>A list of tokens made from the words and their token types.</returns>
		public List<Token> Tokenize(List<string> words)
		{
			int wordCount = words.Count;
			for (int i = 0; i < wordCount; i++)
			{
				string current = words[i];
				string last = (i > 0) ? words[i - 1] : "\0";
				string next = (i < wordCount - 1) ? words[i + 1] : "\0";

				if (current.IsIdentifier()) { AddToken(TokenType.Identifier, current); }
				else if (current == ";") { AddToken(TokenType.Semicolon, ";"); }
				else if (current == "{") { AddToken(TokenType.OpenScope, "{"); }
				else if (current == "}") { AddToken(TokenType.CloseScope, "}"); }
				else if (current == "(") { AddToken(TokenType.OpenParen, "("); }
				else if (current == ")") { AddToken(TokenType.CloseParen, ")"); }
				else if (current == "[") { AddToken(TokenType.OpenBracket, "["); }
				else if (current == "]") { AddToken(TokenType.CloseBracket, "]"); }
				else if (current == ",") { AddToken(TokenType.Comma, ","); }
				else if (ReservedKeywords.Contains(current.ToLower()))
				{
					ProcessReservedWord(current);
				}
				else if (current == "+")
				{
					ProcessPlusSign(last, next);
				}
				else if (current == "-")
				{
					ProcessMinusSign(last, next);
				}
				else if (current == "!")
				{
					ProcessExclamationMark(last, next);
				}
				else if (current == "~")
				{
					ProcessTilde(last, next);
				}
				else if (current == "++")
				{
					ProcessDoublePlus(last, next);
				}
				else if (current == "--")
				{
					ProcessDoubleMinus(last, next);
				}
				else if (current == "*")
				{
					ProcessAsterisk(last, next);
				}
				else if (current == "&")
				{
					ProcessAmpersand(last, next);
				}
				else if (current == ".")
				{
					ProcessDot(last, next);
				}
				else if (current == "->")
				{
					ProcessArrow(last, next);
				}
				else if (current.IsOneOfString("/", "%", "<<", ">>", "<", "<=", ">", ">=", "==", "!=", "|", "^", "&&", "||", "=", "+=", "-=", "*=", "/=", "%=", ">>=", "<<=", "&=", "|=", "^=", "?"))
				{
					ProcessOperator(current, last, next);
				}
				else if (current.Count(c => c == '*') == current.Length && current.Length > 1)
				{
					// Two or more asterisks, but only asterisks
					ProcessMultipleAsterisks(current, last, next);
				}
				else if (current.StartsWith("\"") && current.EndsWith("\""))
				{
					ProcessStringLiteral(current);
				}
				else
				{
					throw new TokenException(current, $"Unidentifiable word {current}");
                }
			}

			return tokenList;
		}

		private void ProcessReservedWord(string current)
		{
			string tokenEnumValue = string.Concat("Key", current);
			if (!Enum.TryParse(tokenEnumValue, true, out TokenType tokenType))
			{
				throw new TokenException(current, "Invalid keyword. (how did you get here anyway)"); /* UNREACHABLE CODE EXCEPTION */
			}
			AddToken(tokenType, current);
		}

		private void ProcessPlusSign(string last, string next)
		{
			// A plus sign can be either unary prefix identity or binary addition.
			// OpIdentity (+) requires: Preceding semicolon, closescope, openparen, comma, binary/ternary operator, or one of { - ! ~ }.
			//	Succeeding identifier, openparen, or one of { - ! ~ * & }.
			// OpAdd (+): Preceding identifier, closeparen, closebracket, one of { ++ -- }.
			//	Succeeding identifier, openparen, or one of { - ! ~ -- & * }.

			if (last == ";" || last == "}" || last == "(" || last == "," || IsBinaryTernaryOperator(last) || last.IsOneOfString("-", "!", "~"))
			{
				if (next.IsIdentifier() || next.IsOneOfString("(", "-", "!", "~", "*", "&"))
				{
					AddToken(TokenType.OpIdentity, "+");
				}
				else
				{
					throw new TokenException("+",
						$"Invalid unary identity operator. Preceded by {last}, succeeded by {next}.");
				}
			}
			else if (last.IsIdentifier() || last == ")" || last == "]" || last.IsOneOfString("++", "--"))
			{
				if (next.IsIdentifier() || next.IsOneOfString("(", "-", "!", "~", "--", "&", "*"))
				{
					AddToken(TokenType.OpAdd, "+");
				}
				else
				{
					throw new TokenException("+",
						$"Invalid addition operator. Preceded by {last}, succeeded by {next}.");
				}
			}
			else
			{
				throw new TokenException("+",
					$"Invalid plus sign token. Preceded by {last}, succeeded by {next}.");
			}
		}

		private void ProcessMinusSign(string last, string next)
		{
			// A minus sign is either a unary prefix inverse operator, or a binary subtraction operator.
			// OpInverse (-) requires: Preceding semicolon, closescope, openparen, comma, binary/ternary operator, or one of { + ! ~ }.
			//	Succeeding identifier, openparen, or one of { + ! ~ * & }.
			// OpSubtract (-): Preceding identifier, closeparen, closebracket, one of { ++ -- }.
			//	Succeeding identifier, openparen, or one of { + ! ~ ++ & * }.

			if (last.IsOneOfString(";", "}", "(", ",", "+", "!", "~") || IsBinaryTernaryOperator(last))
			{
				if (next.IsIdentifier() || next.IsOneOfString("(", "+", "!", "~", "*", "&"))
				{
					AddToken(TokenType.OpInverse, "-");
				}
				else
				{
					throw new TokenException("-",
						$"Invalid unary inverse operator. Preceded by {last}, succeeded by {next}.");
				}
			}
			else if (last.IsIdentifier() || last.IsOneOfString(")", "]", "++", "--"))
			{
				if (next.IsIdentifier() || next.IsOneOfString("(", "+", "!", "~", "++", "&", "*"))
				{
					AddToken(TokenType.OpSubtract, "-");
				}
				else
				{
					throw new TokenException("-",
						$"Invalid subtraction operator. Preceded by {last}, succeeded by {next}.");
				}
			}
			else
			{
				throw new TokenException("-",
					$"Invalid minus sign token. Preceded by {last}, succeeded by {next}.");
			}
		}

		private void ProcessExclamationMark(string last, string next)
		{
			// An exclamation mark is a unary prefix logical NOT.
			// OpLogicalNOT (!) requires: Preceding semicolon, closescope, openparen, comma, binary/ternary operator, or one of { + - ! ~ }.
			//	Succeeding identifier, openparen, or one of { + - ! ~ * & }.

			if (last.IsOneOfString(";", "}", "(", ",", "+", "-", "!", "~") || IsBinaryTernaryOperator(last))
			{
				if (next.IsIdentifier() || next.IsOneOfString("(", "+", "-", "!", "~", "*", "&"))
				{
					AddToken(TokenType.OpLogicalNOT, "!");
				}
				else
				{
					throw new TokenException("!",
						$"Invalid unary logical NOT operator. Preceded by {last}, succeeded by {next}.");
				}
			}
			else
			{
				throw new TokenException("!",
					$"Invalid exclamation mark token. Preceded by {last}, succeeded by {next}.");
			}
		}

		private void ProcessTilde(string last, string next)
		{
			// A tilde is a unary prefix bitwise NOT operator.
			// OpBitwiseNOT (~) requires: Preceding semicolon, closescope, openparen, comma, binary/ternary operator, or one of { + - ! ~ }.
			//	Succeeding identifier, openparen, or one of { + - ! ~ * & }.

			if (last.IsOneOfString(";", "}", "(", ",", "+", "-", "!", "~") || IsBinaryTernaryOperator(last))
			{
				if (next.IsIdentifier() || next.IsOneOfString("(", "+", "-", "!", "~", "*", "&"))
				{
					AddToken(TokenType.OpBitwiseAND, "~");
				}
				else
				{
					throw new TokenException("~",
						$"Invalid bitwise NOT operator. Preceded by {last}, succeeded by {next}.");
				}
			}
			else
			{
				throw new TokenException("~", $"Invalid tilde token. Preceded by {last}, succeeded by {next}.");
			}
		}

		private void ProcessDoublePlus(string last, string next)
		{
			// A double plus sign is either a preincrement or postincrement.
			// OpPreincrement (++) requires: Preceding semicolon, closescope, openparen, comma, or binary/ternary operator.
			//	Succeeding identifier.
			// OpPostincrement (++) requires: Preceding identifier or closebracket.
			//	Succeeding binary/ternary operator or semicolon.

			if (last.IsOneOfString(";", "}", "(", ",") || IsBinaryTernaryOperator(last))
			{
				if (next.IsIdentifier())
				{
					AddToken(TokenType.OpPreincrement, "++");
				}
				else
				{
					throw new TokenException("++",
						$"Invalid preincrement operator. Preceded by {last}, succeeded by {next}.");
				}
			}
			else if (last.IsIdentifier() || last == "}")
			{
				if (IsBinaryTernaryOperator(next) || next == ";")
				{
					AddToken(TokenType.OpPostincrement, "++");
				}
				else
				{
					throw new TokenException("++",
						$"Invalid postincrement operator. Preceded by {last}, succeeded by {next}.");
				}
			}
			else
			{
				throw new TokenException("++",
					$"Invalid double plus sign token. Preceded by {last}, succeeded by {next}.");
			}
		}

		private void ProcessDoubleMinus(string last, string next)
		{
			// A double minus sign is either a predecrement or postdecrement.
			// OpPredecrement (--) requires: Preceding semicolon, closescope, openparen, comma, or binary/ternary operator.
			//	Succeeding identifier.
			// OpPostdecrement (--) requires: Preceding identifier or closebracket.
			//	Succeeding binary/ternary operator or semicolon.

			if (last.IsOneOfString(";", "}", "(", ",") || IsBinaryTernaryOperator(last))
			{
				if (next.IsIdentifier())
				{
					AddToken(TokenType.OpPredecrement, "--");
				}
				else
				{
					throw new TokenException("--",
						$"Invalid predecrement operator. Preceded by {last}, succeeded by {next}.");
				}
			}
			else if (last.IsIdentifier() || last == "}")
			{
				if (IsBinaryTernaryOperator(next) || next == ";")
				{
					AddToken(TokenType.OpPostdecrement, "--");
				}
				else
				{
					throw new TokenException("--",
						$"Invalid postdecrement operator. Preceded by {last}, succeeded by {next}.");
				}
			}
			else
			{
				throw new TokenException("--",
					$"Invalid double minus sign token. Preceded by {last}, succeeded by {next}.");
			}
		}

		private void ProcessAsterisk(string last, string next)
		{
			// An asterisk is either a multiplication operator, pointer dereference, or pointer type declarator.
			// At this point, we cannot distinguish between the multiplication operator or pointer type declarator, so we will add it is Indeterminate.
			// OpPointerDerefence (*): Preceding semicolon, closescope, openparen, comma, binary/ternary operator, or one of { + - ! ~ & * }.
			//	Succeeding identifier, openparen or *.
			// OpMultiply (*): Preceding identifier, closeparen, closebracket, one of { ++ -- }.
			//	Succeeding identifier, one of { + - ! ~ ++ -- & * }.
			// Pointer data type: Preceding identifier.

			if (last.IsOneOfString(";", "}", "(", ",", "+", "-", "!", "~", "&", "*") || IsBinaryTernaryOperator(last))
			{
				if (next.IsIdentifier() || next.IsOneOfString("(", ")", "*"))
				{
					AddToken(TokenType.OpPointerDereference, "*");
				}
				else
				{
					throw new TokenException("*",
						$"Invalid pointer dereference operator. Preceded by {last}, succeeded by {next}.");
				}
			}
			else if (last.IsIdentifier(true))
			{
				if (last.IsOneOfString(")", "]", "++", "--") && (next.IsIdentifier() || next.IsOneOfString("+", "-", "!", "~", "++", "--", "&", "*")))
				{
					AddToken(TokenType.OpMultiply, "*");
				}
				else
				{
					AddToken(TokenType.Indeterminate, "*");
				}
			}
			else
			{
				throw new TokenException("*",
					$"Invalid asterisk token. Preceded by {last}, succeeded by {next}.");
			}
		}

		private void ProcessAmpersand(string last, string next)
		{
			// The ampersand indicates either a variable dereference or a bitwise AND operator.
			// OpVariableDerefence (&): Preceding semicolon, closescope, openparen, comma, binary/ternary operator, or one of { + - ! ~ & }.
			//	Succeeding identifier, openparen, or one of { & * }.
			// OpBitwiseAnd (&): Preceding identifier, closeparen, closebracket, one of { ++ -- }.
			//	Succeeding identifier, openparen, one of { + - ! ~ ++ -- * }.

			if (last.IsOneOfString(";", "}", "(", ",", "+", "-", "!", "~", "&") || IsBinaryTernaryOperator(last))
			{
				if (next.IsIdentifier() || next.IsOneOfString("(", "*", "&"))
				{
					AddToken(TokenType.OpVariableDereference, "&");
				}
				else
				{
					throw new TokenException("&",
						$"Invalid variable dereference operator. Preceded by {last}, succeeded by {next}.");
				}
			}
			else if (last.IsIdentifier() || last.IsOneOfString(")", "]", "++", "--"))
			{
				if (next.IsIdentifier() || next.IsOneOfString("(", "+", "-", "!", "~", "++", "--", "*"))
				{
					AddToken(TokenType.OpBitwiseAND, "&");
				}
				else
				{
					throw new TokenException("&",
						$"Invalid bitwise AND operator. Preceded by {last}, succeeded by {next}.");
				}
			}
			else
			{
				throw new TokenException("&",
					$"Invalid ampersand token. Preceded by {last}, succeeded by {next}.");
			}
		}

		private void ProcessDot(string last, string next)
		{
			// A dot by itself indicates a member access operator.
			// OpMemberAccess (.): Preceding identifier, closeparen, closebracket.
			//	Succeeding identifier.

			if (last.IsIdentifier() || last.IsOneOfString(")", "]"))
			{
				if (next.IsIdentifier())
				{
					AddToken(TokenType.OpMemberAccess, ".");
				}
				else
				{
					throw new TokenException(".",
						$"Invalid member access operator. Preceded by {last}, succeeded by {next}.");
				}
			}
			else
			{
				throw new TokenException(".", $"Invalid dot token. Preceded by {last}, succeeded by {next}.");
			}
		}

		private void ProcessArrow(string last, string next)
		{
			// An arrow is a pointer access operator.
			// OpPointerAccess (->): Preceding identifier, closeparen, closebracket.
			//	Succeeding identifier.

			if (last.IsIdentifier() || last.IsOneOfString(")", "]"))
			{
				if (next.IsIdentifier())
				{
					AddToken(TokenType.OpPointerMemberAccess, "->");
				}
				else
				{
					throw new TokenException("->",
						$"Invalid pointer member access operator. Preceded by {last}, succeeded by {next}.");
				}
			}
			else
			{
				throw new TokenException("->", $"Invalid arrow token. Preceded by {last}, succeeded by {next}.");
			}
		}

		private void ProcessOperator(string current, string last, string next)
		{
			// All Other Binary Operators and the Ternary Aftercondition (?):
			//	Preceding identifier, closeparen, closebracket, one of { ++ -- }.
			//	Succeeding identifier, openparen, or one of { + - ! ~ ++ -- & * }.

			if (last.IsIdentifierOrNumber() || last.IsOneOfString(")", "]", "++", "--"))
			{
				if (next.IsIdentifierOrNumber() || next.IsOneOfString("+", "-", "!", "~", "++", "--", "&", "*", "("))
				{
					AddToken(unambiguousBinaryOperatorTokens[current], current);
				}
				else
				{
					throw new TokenException(current,
						$"Invalid operator {current}. Preceded by {last}, succeeded by {next}.");
				}
			}
			else
			{
				throw new TokenException(current,
					$"Invalid token {current}. Preceded by {last}, succeeded by {next}.");
			}
		}

		private void ProcessMultipleAsterisks(string current, string last, string next)
		{
			// Two or more asterisks forming a single word denote a pointer type.
			// Preceded by: Identifier.
			// Succeeded by: Identifier, closeparen.

			if (last.IsIdentifier(allowReservedWords: true))
			{
				if (next.IsIdentifier() || next == ")")
				{
					AppendToken(current);
				}
				else
				{
					throw new TokenException(current,
						$"Invalid pointer type declaration. Preceded by {last}, succeeded by {next}.");
				}
			}
			else
			{
				throw new TokenException(current,
					$"Invalid sequence of asterisks. Preceded by {last}, succeeded by {next}.");
			}
		}

		private void ProcessStringLiteral(string current)
		{
			// A word beginning and ending in double quotes.
			AddToken(TokenType.StringLiteral, current);
		}

		private void AddToken(TokenType type, string word)
		{
			tokenList.Add(new Token(type, word));
		}

		private void AppendToken(string word)
		{
			Token oldToken = tokenList[tokenList.Count - 1];
			Token newToken = new Token(oldToken.Type, string.Concat(oldToken.Word, word));
			tokenList[tokenList.Count - 1] = newToken;
		}

		private static bool IsBinaryTernaryOperator(string word)
		{
			return BinaryTernaryOperators.Contains(word);
		}
	}

	/// <summary>
	/// Enumerates the kinds of tokens in a Cix file.
	/// </summary>
	public enum TokenType
	{
		Invalid,
		Identifier,
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
		OpTernaryAfterCondition,
		OpTernaryAfterTrueExpression,
		KeyBreak,
		KeyCase,
		KeyChar,
		KeyConst,
		KeyContinue,
		KeyDefault,
		KeyDo,
		KeyDouble,
		KeyElse,
		KeyFloat,
		KeyFor,
		KeyGoto,
		KeyIf,
		KeyInt,
		KeyLong,
		KeyReturn,
		KeySChar,
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
		StringLiteral
	}

	/// <summary>
	/// Represents a word and the type of token it is.
	/// </summary>
	public sealed class Token
	{
		/// <summary>
		/// Gets the type of token this token is.
		/// </summary>
		public TokenType Type { get; }

		/// <summary>
		/// Gets the word of this token.
		/// </summary>
		public string Word { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Token"/> class.
		/// </summary>
		/// <param name="type">The type of token this token is.</param>
		/// <param name="word">The word of this token.</param>
		public Token(TokenType type, string word)
		{
			Type = type;
			Word = word;
		}

		/// <summary>
		/// Returns a string representation of this token.
		/// </summary>
		/// <returns>A string representation of this token</returns>
		public override string ToString() => $"\"{Word}\" ({Type})";
	}
}
