using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cix.Exceptions;

namespace Cix
{
	/// <summary>
	/// Given a list of words from a source file, this class assigns each a token type indicating what it is.
	/// </summary>
	public sealed class Tokenizer
	{
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

		private Dictionary<string, TokenType> unambiguousBinaryOperatorTokens = new Dictionary<string,TokenType>()
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

		private List<Token> tokenList = new List<Token>();

		public List<Token> Tokenize(List<string> words)
		{
			int wordCount = words.Count;
			for (int i = 0; i < wordCount; i++)
			{
				string current = words[i];
				string last = (i > 0) ? words[i - 1] : "\0";
				string next = (i < wordCount - 1) ? words[i + 1] : "\0";

				if (current.IsIdentifier()) { this.AddToken(TokenType.Identifier, current); }
				else if (current == ";") { this.AddToken(TokenType.Semicolon, ";"); }
				else if (current == "{") { this.AddToken(TokenType.OpenScope, "{"); }
				else if (current == "}") { this.AddToken(TokenType.CloseScope, "}"); }
				else if (current == "(") { this.AddToken(TokenType.OpenParen, "("); }
				else if (current == ")") { this.AddToken(TokenType.CloseParen, ")"); }
				else if (current == "[") { this.AddToken(TokenType.OpenBracket, "["); }
				else if (current == "]") { this.AddToken(TokenType.CloseBracket, "]"); }
				else if (current == ",") { this.AddToken(TokenType.Comma, ","); }
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
					this.ProcessDot(last, next);
				}
				else if (current == "->")
				{
					this.ProcessArrow(last, next);
				}
				else if (current.IsOneOfString("/", "%", "<<", ">>", "<", "<=", ">", ">=", "==", "!=", "|", "^", "&&", "||", "=", "+=", "-=", "*=", "/=", "%=", ">>=", "<<=", "&=", "|=", "^=", "?"))
				{
					this.ProcessOperator(current, last, next);
				}
				else if (current.Count(c => c == '*') == current.Length && current.Length > 1)
				{
					// Two or more asterisks, but only asterisks
					this.ProcessMultipleAsterisks(current, last, next);
				}
				else
				{
					throw new TokenException(current, string.Format("Unidentifiable word {0}.", current));
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
				if (next.IsIdentifier() || next.IsOneOfString("-", "!", "~", "*", "&"))
				{
					this.AddToken(TokenType.OpIdentity, "+");
				}
				else
				{
					throw new TokenException("+", string.Format("Invalid unary identity operator. Preceded by {0}, succeeded by {1}.", last, next));
				}
			}
			else if (last.IsIdentifier() || last == ")" || last == "]" || last.IsOneOfString("++", "--"))
			{
				if (next.IsIdentifier() || next.IsOneOfString("-", "!", "~", "--", "&", "*"))
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
				if (next.IsIdentifier() || next.IsOneOfString("+", "!", "~", "*", "&"))
				{
					this.AddToken(TokenType.OpInverse, "-");
				}
				else
				{
					throw new TokenException("-", string.Format("Invalid unary inverse operator. Preceded by {0}, succeeded by {1}.", last, next));
				}
			}
			else if (last.IsIdentifier() || last.IsOneOfString(")", "]", "++", "--"))
			{
				if (next.IsIdentifier() || next.IsOneOfString("(", "+", "!", "~", "++", "&", "*"))
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
				if (next.IsIdentifier() || next.IsOneOfString("+", "-", "!", "~", "*", "&"))
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
				if (next.IsIdentifier() || next.IsOneOfString("+", "-", "!", "~", "*", "&"))
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
				if (next.IsIdentifier())
				{
					this.AddToken(TokenType.OpPreincrement, "++");
				}
				else
				{
					throw new TokenException("++", string.Format("Invalid preincrement operator. Preceded by {0}, succeeded by {1}.", last, next));
				}
			}
			else if (last.IsIdentifier() || last == "}")
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
				if (next.IsIdentifier())
				{
					this.AddToken(TokenType.OpPredecrement, "--");
				}
				else
				{
					throw new TokenException("--", string.Format("Invalid predecrement operator. Preceded by {0}, succeeded by {1}.", last, next));
				}
			}
			else if (last.IsIdentifier() || last == "}")
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
				if (next.IsIdentifier() || next.IsOneOfString("(", ")"))
				{
					this.AddToken(TokenType.OpPointerDereference, "*");
				}
				else
				{
					throw new TokenException("*", string.Format("Invalid pointer dereference operator. Preceded by {0}, succeeded by {1}.", last, next));
				}
			}
			else if (last.IsIdentifier(true) || last.IsOneOfString(")", "]", "++", "--"))
			{
				this.AddToken(TokenType.Indeterminate, "*");
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
				if (next.IsIdentifier() || next.IsOneOfString("*", "&"))
				{
					this.AddToken(TokenType.OpVariableDereference, "&");
				}
				else
				{
					throw new TokenException("&", string.Format("Invalid variable dereference operator. Preceded by {0}, succeeded by {1}.", last, next));
				}
			}
			else if (last.IsIdentifier() || last.IsOneOfString(")", "]", "++", "--"))
			{
				if (next.IsIdentifier() || next.IsOneOfString("+", "-", "!", "~", "++", "--", "*"))
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
			// A dot by itself indicates a member access operator.
			// OpMemberAccess (.): Preceding identifier, closeparen, closebracket. 
			//	Succeeding identifier.

			if (last.IsIdentifier() || last.IsOneOfString(")", "]"))
			{
				if (next.IsIdentifier())
				{
					this.AddToken(TokenType.OpMemberAccess, ".");
				}
				else
				{
					throw new TokenException(".", string.Format("Invalid member access operator. Preceded by {0}, succeeded by {1}.", last, next));
				}
			}
			else
			{
				throw new TokenException(".", string.Format("Invalid dot token. Preceded by {0}, succeeded by {1}.", last, next));
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
					this.AddToken(TokenType.OpPointerMemberAccess, "->");
				}
				else
				{
					throw new TokenException("->", string.Format("Invalid pointer member access operator. Preceded by {0}, succeeded by {1}.", last, next));
				}
			}
			else
			{
				throw new TokenException("->", string.Format("Invalid arrow token. Preceded by {0}, succeeded by {1}.", last, next));
			}
		}

		private void ProcessOperator(string current, string last, string next)
		{
			// All Other Binary Operators and the Ternary Aftercondition (?): 
			//	Preceding identifier, closeparen, closebracket, one of { ++ -- }. 
			//	Succeeding identifier, one of { + - ! ~ ++ -- & * }.

			if (last.IsIdentifier() || last.IsOneOfString(")", "]", "++", "--"))
			{
				if (next.IsIdentifier() || next.IsOneOfString("+", "-", "!", "~", "++", "--", "&", "*"))
				{
					this.AddToken(this.unambiguousBinaryOperatorTokens[current], current);
				}
				else
				{
					throw new TokenException(current, string.Format("Invalid operator {0}. Preceded by {1}, succeeded by {2}.", current, last, next));
				}
			}
			else
			{
				throw new TokenException(current, string.Format("Invalid token {0}. Preceded by {1}, succeeded by {2}.", current, last, next));
			}
		}

		private void ProcessMultipleAsterisks(string current, string last, string next)
		{
			// Two or more asterisks forming a single word denote a pointer type.
			// Preceded by: Identifier.
			// Succeeded by: Identifier, closeparen.

			if (last.IsIdentifier())
			{
				if (next.IsIdentifier() || next == ")")
				{
					this.AppendToken(current);
				}
				else
				{
					throw new TokenException(current, string.Format("Invalid pointer type declaration. Preceded by {0}, succeeded by {1}.", last, next));
				}
			}
			else
			{
				throw new TokenException(current, string.Format("Invalid sequence of asterisks. Preceded by {0}, succeeded by {1}.", last, next));
			}
		}

		private void AddToken(TokenType type, string word)
		{
			this.tokenList.Add(new Token(type, word));
		}

		private void AppendToken(string word)
		{
			Token oldToken = this.tokenList[this.tokenList.Count - 1];
			Token newToken = new Token(oldToken.Type, string.Concat(oldToken.Word, word));
			this.tokenList[this.tokenList.Count - 1] = newToken;
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
		Indeterminate
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
