using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix
{
	public static class StringExtensions
	{
		private static readonly char[] validIdentifierCharacters = new char[]
		{ 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 
		  'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 
		  'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c',
		  'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
		  'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 
		  'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6',
		  '7', '8', '9', '_', '.' };

		internal static readonly string[] reservedKeywords = new string[] 
		{ "break", "case", "char", "const", "continue", "default", "do",
		  "double", "else", "float", "for", "goto", "if", "int", "long", 
		  "return", "schar", "short", "sizeof", "struct", "switch", "uint", 
		  "ulong", "ushort", "void", "while" };

		public static bool IsOneOfString(this string check, params string[] values)
		{
			return new HashSet<string>(values).Contains(check);
		}

		public static bool IsIdentifier(this string word, bool allowReservedWords = false)
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

		public static bool IsNumericLiteral(this string word)
		{
			if (string.IsNullOrEmpty(word) || word == "\r\n")
			{
				// Empty strings and newlines cannot be numeric literals.
				return false;
			}

			if (!char.IsDigit(word[0]))
			{
				// A numeric literal cannot start with a letter.
				return false;
			}

			foreach (char c in word.ToLowerInvariant())
			{
				// All characters in a numeric literal must be a digit, a period, or one of the suffixes
				if (!(c >= '0' && c <= '9') && c == '.' && c.IsOneOfCharacter('u', 'l', 'f', 'd'))
				{
					return false;
				}
			}

			return true;
		}

		public static string RemoveComments(this string input)
		{
			StringBuilder result = new StringBuilder();				// Stores the uncommented version of the file.
			CommentKind currentCommentKind = CommentKind.NoComment;	// Keeps track of what kind of comment we're in, if any

			for (int i = 0; i < input.Length; i++)
			{
				char current = input[i];
				char last = (i > 0) ? input[i - 1] : '\0';
				char next = (i < input.Length - 1) ? input[i + 1] : '\0';
 
				if (current == '/')
				{
					if (last == '/' || last == '*')
					{
						continue;
					}
					else if (next == '/')
					{
						currentCommentKind = CommentKind.SingleLine;
					}
					else if (next == '*')
					{
						currentCommentKind = CommentKind.MultipleLines;
					}
					else
					{
						result.Append(current);
					}
				}
				else if (current == '*')
				{
					if (last == '/')
					{
						continue;
					}
					else if (next == '/')
					{
						currentCommentKind = CommentKind.NoComment;
					}
					else
					{
						result.Append(current);
					}
				}
				else if (current == '\r' || current == '\n')
				{
					if (currentCommentKind == CommentKind.SingleLine)
					{
						currentCommentKind = CommentKind.NoComment;
					}
					result.Append(current);
				}
				else
				{
					if (currentCommentKind == CommentKind.NoComment)
					{
						result.Append(current);
					}
				}
			}
			
			return result.ToString();
		}

		public static Tuple<string, int> SeparateTypeNameAndPointerLevel(this string fullTypeName)
		{
			if (!fullTypeName.Any(c => c == '*'))
			{
				return new Tuple<string, int>(fullTypeName, 0);
			}

			int pointerLevel = fullTypeName.Count(c => c == '*');
			string typeName = fullTypeName.Substring(0, fullTypeName.Length - pointerLevel);

			return new Tuple<string, int>(typeName, pointerLevel);
		}

		private enum CommentKind
		{
			NoComment,
			SingleLine,
			MultipleLines
		}
	}
}
