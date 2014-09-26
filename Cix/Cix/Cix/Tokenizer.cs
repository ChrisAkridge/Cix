using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix
{
	/// <summary>
	/// Given a list of words from a source file, this class assigns each a token type indicating what it is.
	/// </summary>
	public sealed class Tokenizer
	{
		private static readonly char[] validIdentifierCharacters = new char[]
		{ 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 
		  'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 
		  'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c',
		  'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
		  'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 
		  'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6',
		  '7', '8', '9', '_', '.' };

		private static readonly string[] unaryPrefixOperators = new string[] { "+", "-", "!", "~", "++", "--", "*", "&" };
		private static readonly string[] unaryPostfixOperators = new string[] { "++", "--" };
		private static readonly string[] binaryTernaryOperators = new string[]
		{ ".", "->", "*", "/", "%", "+", "-", "<<", ">>", "<", "<=", ">", ">=", 
		  "==", "!=", "&", "|", "^", "&&", "||", "=", "+=", "-=", "*=", "/=",
		  "%=", "<<=", ">>=", "<<=", "&=", "|=", "^=", "?", ":" };

		private static readonly string[] reservedKeywords = new string[] 
		{ "break", "case", "char", "const", "continue", "default", "do",
		  "double", "else", "float", "for", "goto", "if", "int", "long", 
		  "return", "schar", "short", "sizeof", "struct", "switch", "uint", 
		  "ulong", "ushort", "void", "while" };

		private List<Token> tokenList = new List<Token>();

		public List<Token> Tokenize(List<string> words)
		{
			int wordCount = words.Count;
			for (int i = 0; i < wordCount; i++)
			{
				string current = words[i];
				string last = (i > 0) ? words[i - 1] : "\0";
				string next = (i < wordCount - 1) ? words[i + 1] : "\0";

				if (IsIdentifier(current)) { this.AddToken(TokenType.Identifier, current); }
				else if (current == ";") { this.AddToken(TokenType.Semicolon, ";"); }
				else if (current == "{") { this.AddToken(TokenType.OpenScope, "{"); }
				else if (current == "}") { this.AddToken(TokenType.CloseScope, "}"); }
				else if (current == "(") { this.AddToken(TokenType.OpenParen, "("); }
				else if (current == ")") { this.AddToken(TokenType.CloseParen, ")"); }
				else if (current == "[") { this.AddToken(TokenType.OpenBracket, "["); }
				else if (current == "]") { this.AddToken(TokenType.CloseBracket, "]"); }
				else if (reservedKeywords.Contains(current.ToLower()))
				{
					this.ProcessReservedWord(current);
				}
				else if (current == "+")
				{
					this.ProcessPlusSign(last, next);
				}
				else if (current == "-")
				{
					this.ProcessMinusSign(last, next);
				}
				else if (current == "!")
				{
					this.ProcessExclamationMark(last, next);
				}
				else if (current == "~")
				{
					this.ProcessTilde(last, next);
				}
				else if (current == "++")
				{
					this.ProcessDoublePlus(last, next);
				}
				else if (current == "--")
				{
					this.ProcessDoubleMinus(last, next);
				}
				else if (current == "*")
				{
					this.ProcessAsterisk(last, next);
				}
				else if (current == "&")
				{
					this.ProcessAmpersand(last, next);
				}
				else if (current == ".")
				{

				}
				else if (current == "->")
				{

				}
				else if (current.IsOneOfString("/", "%", "<<", ">>", "<", "<=", ">=", "==", "!=", "|", "^", "&&", "||", "=", "+=", "-=", "*=", "/=", "%=", ">>=", "<<=", "&=", "|=", "^=", "?"))
				{ 
				
				}
				else if (current.Count<char>(c => c == '*') == current.Length)
				{

				}
				else
				{
					this.AddToken(TokenType.Invalid, current);
				}
			}

			return this.tokenList;
		}

		private void ProcessReservedWord(string current)
		{
			string tokenEnumValue = string.Concat("Key", current);
			TokenType tokenType;
			if (!Enum.TryParse(tokenEnumValue, true, out tokenType))
			{
				throw new TokenException(current, "Invalid keyword. (how did you get here anyway)");
			}
			this.AddToken(tokenType, current);
		}

		private void ProcessPlusSign(string last, string next)
		{
			// A plus sign can be either unary prefix identity or binary addition.
			// OpIdentity (+) requires: Preceding semicolon, closescope, openparen, comma, binary/ternary operator, or one of { - ! ~ }. 
			//	Succeeding identifier, or one of { - ! ~ * & }.
			// OpAdd (+): Preceding identifier, closeparen, closebracket, one of { ++ -- }. 
			//	Succeeding identifier, one of { - ! ~ -- & * }.

			if (last == ";" || last == "}" || last == "(" || last == "," || IsBinaryTernaryOperator(last) || last.IsOneOfString("-", "!", "~"))
			{
				if (IsIdentifier(next) || next.IsOneOfString("-", "!", "~", "*", "&"))
				{
					this.AddToken(TokenType.OpIdentity, "+");
				}
				else
				{
					throw new TokenException("+", string.Format("Invalid unary identity operator. Preceded by {0}, succeeded by {1}.", last, next));
				}
			}
			else if (IsIdentifier(last) || last == ")" || last == "]" || last.IsOneOfString("++", "--"))
			{
				if (IsIdentifier(next) || next.IsOneOfString("-", "!", "~", "--", "&", "*"))
				{
					this.AddToken(TokenType.OpAdd, "+");
				}
				else
				{
					throw new TokenException("+", string.Format("Invalid addition operator. Preceded by {0}, succeeded by {1}.", last, next));
				}
			}
			else
			{
				throw new TokenException("+", string.Format("Invalid plus sign token. Preceded by {0}, succeeded by {1}.", last, next));
			}
		}

		private void ProcessMinusSign(string last, string next)
		{
			// A minus sign is either a unary prefix inverse operator, or a binary subtraction operator.
			// OpInverse (-) requires: Preceding semicolon, closescope, openparen, comma, binary/ternary operator, or one of { + ! ~ }.
			//	Succeeding identifier, or one of { + ! ~ * & }.
			// OpSubtract (-): Preceding identifier, closeparen, closebracket, one of { ++ -- }.
			//	Succeeding identifier, one of { + ! ~ ++ & * }.

			if (last.IsOneOfString(";", "}", "(", ",", "+", "!", "~") || IsBinaryTernaryOperator(last))
			{
				if (IsIdentifier(next) || next.IsOneOfString("+", "!", "~", "*", "&"))
				{
					this.AddToken(TokenType.OpInverse, "-");
				}
				else
				{
					throw new TokenException("-", string.Format("Invalid unary inverse operator. Preceded by {0}, succeeded by {1}.", last, next));
				}
			}
			else if (IsIdentifier(last) || last.IsOneOfString(")", "]", "++", "--"))
			{
				if (IsIdentifier(next) || next.IsOneOfString("(", "+", "!", "~", "++", "&", "*"))
				{
					this.AddToken(TokenType.OpSubtract, "-");
				}
				else
				{
					throw new TokenException("-", string.Format("Invalid subtraction operator. Preceded by {0}, succeeded by {1}.", last, next));
				}
			}
			else
			{
				throw new TokenException("-", string.Format("Invalid minus sign token. Preceded by {0}, succeeded by {1}.", last, next));
			}
		}

		private void ProcessExclamationMark(string last, string next)
		{
			// An exclamation mark is a unary prefix logical NOT.
			// OpLogicalNOT (!) requires: Preceding semicolon, closescope, openparen, comma, binary/ternary operator, or one of { + - ! ~ }.
			//	Succeeding identifier, or one of { + - ! ~ * & }.

			if (last.IsOneOfString(";", "}", "(", ",", "+", "-", "!", "~") || IsBinaryTernaryOperator(last))
			{
				if (IsIdentifier(next) || next.IsOneOfString("+", "-", "!", "~", "*", "&"))
				{
					this.AddToken(TokenType.OpLogicalNOT, "!");
				}
				else
				{
					throw new TokenException("!", string.Format("Invalid unary logical NOT operator. Preceded by {0}, succeeded by {1}.", last, next));
				}
			}
			else
			{
				throw new TokenException("!", string.Format("Invalid exclamation mark token. Preceded by {0}, succeeded by {1}.", last, next));
			}
		}

		private void ProcessTilde(string last, string next)
		{
			// A tilde is a unary prefix bitwise NOT operator.
			// OpBitwiseNOT (~) requires: Preceding semicolon, closescope, openparen, comma, binary/ternary operator, or one of { + - ! ~ }.
			//	Succeeding identifier, or one of { + - ! ~ * & }.

			if (last.IsOneOfString(";", "}", "(", ",", "+", "-", "!", "~") || IsBinaryTernaryOperator(last))
			{
				if (IsIdentifier(next) || next.IsOneOfString("+", "-", "!", "~", "*", "&"))
				{
					this.AddToken(TokenType.OpBitwiseAND, "~");
				}
				else
				{
					throw new TokenException("~", string.Format("Invalid bitwise NOT operator. Preceded by {0}, succeeded by {1}.", last, next));
				}
			}
			else
			{
				throw new TokenException("~", string.Format("Invalid tilde token. Preceded by {0}, succeeded by {1}.", last, next));
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
				if (IsIdentifier(next))
				{
					this.AddToken(TokenType.OpPreincrement, "++");
				}
				else
				{
					throw new TokenException("++", string.Format("Invalid preincrement operator. Preceded by {0}, succeeded by {1}.", last, next));
				}
			}
			else if (IsIdentifier(last) || last == "}")
			{
				if (IsBinaryTernaryOperator(next) || next == ";")
				{
					this.AddToken(TokenType.OpPostincrement, "++");
				}
				else
				{
					throw new TokenException("++", string.Format("Invalid postincrement operator. Preceded by {0}, succeeded by {1}.", last, next));
				}
			}
			else
			{
				throw new TokenException("++", string.Format("Invalid double plus sign token. Preceded by {0}, succeeded by {1}.", last, next));
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
				if (IsIdentifier(next))
				{
					this.AddToken(TokenType.OpPredecrement, "--");
				}
				else
				{
					throw new TokenException("--", string.Format("Invalid predecrement operator. Preceded by {0}, succeeded by {1}.", last, next));
				}
			}
			else if (IsIdentifier(last) || last == "}")
			{
				if (IsBinaryTernaryOperator(next) || next == ";")
				{
					this.AddToken(TokenType.OpPostdecrement, "--");
				}
				else
				{
					throw new TokenException("--", string.Format("Invalid postdecrement operator. Preceded by {0}, succeeded by {1}.", last, next));
				}
			}
			else
			{
				throw new TokenException("--", string.Format("Invalid double minus sign token. Preceded by {0}, succeeded by {1}.", last, next));
			}
		}

		private void ProcessAsterisk(string last, string next)
		{
			// An asterisk is either a multiplication operator, pointer dereference, or pointer type declarator.
			// At this point, we cannot distinguish between the multiplication operator or pointer type declarator, so we will add it is Indeterminate.
			// OpPointerDerefence (*): Preceding semicolon, closescope, openparen, comma, binary/ternary operator, or one of { + - ! ~ & }.
			//	Succeeding identifier or openparen.
			// OpMultiply (*): Preceding identifier, closeparen, closebracket, one of { ++ -- }.
			//	Succeeding identifier, one of { + - ! ~ ++ -- & * }.
			// Pointer data type: Preceding identifier.

			if (last.IsOneOfString(";", "}", "(", ",", "+", "-", "!", "~", "&") || IsBinaryTernaryOperator(last))
			{
				if (IsIdentifier(next) || next.IsOneOfString("(", ")"))
				{
					this.AddToken(TokenType.OpPointerDereference, "*");
				}
				else
				{
					throw new TokenException("*", string.Format("Invalid pointer dereference operator. Preceded by {0}, succeeded by {1}.", last, next));
				}
			}
			else if (IsIdentifier(last, true) || last.IsOneOfString(")", "]", "++", "--"))
			{
				this.AddToken(TokenType.IndeterminateAsterisk, "*");
			}
			else
			{
				throw new TokenException("*", string.Format("Invalid asterisk token. Preceded by {0}, succeeded by {1}.", last, next));
			}
		}

		private void ProcessAmpersand(string last, string next)
		{
			// The ampersand indicates either a variable dereference or a bitwise AND operator.
			// OpVariableDerefence (&): Preceding semicolon, closescope, openparen, comma, binary/ternary operator, or one of { + - ! ~ & }. 
			//	Succeeding identifier or one of { & * }.
			// OpBitwiseAnd (&): Preceding identifier, closeparen, closebracket, one of { ++ -- }. 
			//	Succeeding identifier, one of { + - ! ~ ++ -- * }.

			if (last.IsOneOfString(";", "}", "(", ",", "+", "-", "!", "~", "&") || IsBinaryTernaryOperator(last))
			{
				if (IsIdentifier(next) || next.IsOneOfString("*", "&"))
				{
					this.AddToken(TokenType.OpVariableDereference, "&");
				}
				else
				{
					throw new TokenException("&", string.Format("Invalid variable dereference operator. Preceded by {0}, succeeded by {1}.", last, next));
				}
			}
			else if (IsIdentifier(last) || last.IsOneOfString(")", "]", "++", "--"))
			{
				if (IsIdentifier(next) || next.IsOneOfString("+", "-", "!", "~", "++", "--", "*"))
				{
					this.AddToken(TokenType.OpBitwiseAND, "&");
				}
				else
				{
					throw new TokenException("&", string.Format("Invalid bitwise AND operator. Preceded by {0}, succeeded by {1}.", last, next));
				}
			}
			else
			{
				throw new TokenException("&", string.Format("Invalid ampersand token. Preceded by {0}, succeeded by {1}.", last, next));
			}
		}

		private void ProcessDot(string last, string next)
		{

		}

		private void ProcessArrow(string last, string next)
		{

		}

		private void ProcessOperator(string last, string next)
		{

		}

		private void AddToken(TokenType type, string word)
		{
			this.tokenList.Add(new Token(type, word));
		}

		private void AppendToken(TokenType type, string word)
		{
			tokenList[tokenList.Count - 1] = new Token(type, word);
		}

		private static bool IsIdentifier(string word, bool allowReservedWords = false)
		{
			if (string.IsNullOrEmpty(word) || word == "\\r\\n")
			{
				return false;
			}

			if (word.Length == 1 && word[0] == '.')
			{
				// A dot by itself is an OperatorMemberAccess. If it's not by itself, it's part of a numeric literal.
				return false;
			}

			foreach (char c in word)
			{
				if (!c.IsOneOfCharacter(validIdentifierCharacters))
				{
					return false;
				}
			}

			if (!allowReservedWords)
			{
				string lower = word.ToLower();
				foreach (string reservedWord in reservedKeywords)
				{
					if (word == reservedWord)
					{
						return false;
					}
				}
			}
			return true;
		}

		private static bool IsBinaryTernaryOperator(string word)
		{
			return binaryTernaryOperators.Contains(word);
		}
	}

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
		OpBitwiseNOTAssign,
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
		IndeterminateAsterisk
	}

	public sealed class Token
	{
		public TokenType Type { get; private set; }
		public string Word { get; private set; }

		public Token(TokenType type, string word)
		{
			this.Type = type;
			this.Word = word;
		}
	}
}
