using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cix.Extensions
{
    public static class StringExtensions
    {
	    private static readonly char[] ValidIdentifierCharacters =
	    { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
		  'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S',
		  'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c',
		  'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
		  'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w',
		  'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6',
		  '7', '8', '9', '_', '.' };

	    private static readonly char[] InvalidFirstIdentifierCharacters =
		{ '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

	    internal static readonly string[] ReservedKeywords =
	    { "break", "case", "char", "const", "continue", "default", "do",
		  "double", "else", "float", "for", "goto", "if", "int", "long",
		  "return", "schar", "short", "sizeof", "struct", "switch", "uint",
		  "ulong", "ushort", "void", "while" };

		public static bool IsOneOfString(this string check, params string[] values)
			=> new HashSet<string>(values).Contains(check);

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

			// if (invalidFirstIdentifierCharacters.Contains(word[0])) { return false; }

			if (word.Any(c => !c.IsOneOfCharacter(ValidIdentifierCharacters))) { return false; }

			return allowReservedWords || ReservedKeywords.All(r => word.ToLower() != r);
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

			for (var i = 0; i < word.ToLowerInvariant().Length; i++)
			{
				char c = word.ToLowerInvariant()[i];
				// All characters in a numeric literal must be a digit, a period, or one of the suffixes
				if (!(c >= '0' && c <= '9') && c == '.' && c.IsOneOfCharacter('u', 'l', 'f', 'd'))
				{
					return false;
				}
			}

			return true;
		}

		public static bool IsIdentifierOrNumber(this string word) => IsIdentifier(word) || IsNumericLiteral(word);

		public static Tuple<string, int> SeparateTypeNameAndPointerLevel(this string fullTypeName)
		{
			if (fullTypeName.All(c => c != '*'))
			{
				return new Tuple<string, int>(fullTypeName, 0);
			}

			int pointerLevel = fullTypeName.Count(c => c == '*');
			string typeName = fullTypeName.Substring(0, fullTypeName.Length - pointerLevel);

			return new Tuple<string, int>(typeName, pointerLevel);
		}

		public static string TrimAsterisks(this string typeName)
		{
			if (string.IsNullOrEmpty(typeName))
			{
				throw new ArgumentException("The provided type name was null or empty.");
			}

			int firstAsteriskIndex = typeName.IndexOf('*');
			return (firstAsteriskIndex == -1) ? typeName : typeName.Substring(0, firstAsteriskIndex);
		}
	}
}
