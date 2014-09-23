using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix
{
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
		private static readonly string[] binaryOperators = new string[]
		{ ".", "->", "*", "/", "%", "+", "-", "<<", ">>", "<", "<=", ">", ">=", 
		  "==", "!=", "&", "|", "^", "&&", "||", "=", "+=", "-=", "*=", "/=",
		  "%=", "<<=", ">>=", "<<=", "&=", "|=", "^=" };

		private static readonly string[] reservedKeywords = new string[] 
		{ "break", "case", "char", "const", "continue", "default", "do",
		  "double", "else", "float", "for", "goto", "if", "int", "long", 
		  "return", "short", "sizeof", "struct", "switch", "void", "while" };

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
			switch (current)
			{
				case "break":
				case "case":
				case "char":
				case "const":
				case "continue":
				case "default":
				case "do":
				case "double":
				case "else":
				case "float":
				case "for":
				case "goto":
				case "if":
				case "int":
				case "long":
				case "return":
				case "short":
				case "sizeof":
				case "struct":
				case "switch":
				case "void":
				case "while":
				default:
					break;
			}
		}

		private void AddToken(TokenType type, string word)
		{
			this.tokenList.Add(new Token(type, word));
		}

		private static bool IsIdentifier(string word)
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

			string lower = word.ToLower();
			foreach (string reservedWord in reservedKeywords)
			{
				if (word == reservedWord)
				{
					return false;
				}
			}
			return true;
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
		KeyElse,
		KeyFloat,
		KeyFor,
		KeyIf,
		KeyInt,
		KeyLong,
		KeyReturn,
		KeyShort,
		KeySizeof,
		KeyStruct,
		KeySwitch,
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
