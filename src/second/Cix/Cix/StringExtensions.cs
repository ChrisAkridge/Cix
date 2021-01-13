using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Cix
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
		{
			"break", "case", "char", "const", "continue", "default", "do", "double", "else", "float", "for", "global",
			"goto", "if", "int", "long", "return", "schar", "short", "sizeof", "struct", "switch", "uint",
			"ulong", "ushort", "void", "while"
		};

		public static bool IsOneOfString(this string check, params string[] values)
			=> new HashSet<string>(values).Contains(check);

		public static bool IsIdentifier(this string word, bool allowReservedWords = false)
		{
			// TODO: support identifiers starting with @
			if (string.IsNullOrEmpty(word) || word == "\\r\\n")
			{
				return false;
			}

			if (word.Length == 1 && word[0] == '.')
			{
				// A dot by itself is an OperatorMemberAccess. If it's not by itself, it's part of a numeric literal.
				return false;
			}

			if (InvalidFirstIdentifierCharacters.Contains(word[0])) { return false; }

			foreach (char c in word)
			{
				if (!c.IsOneOfCharacter(ValidIdentifierCharacters))
				{
					return false;
				}
			}

			return allowReservedWords || ReservedKeywords.All(r => word.ToLower() != r);
		}

		/// <summary>
		/// Determines if a given string is a basic numeric literal (an optional minus sign, then
		/// one or more digits).
		/// </summary>
		/// <param name="word">The word to check.</param>
		/// <returns>True if <paramref name="word"/> is a basic numeric literal, false if it's not.</returns>
		public static bool IsBasicNumericLiteral(this string word)
		{
			// Turns out we can just rely on TryParse. We don't care here exactly what type of number
			// we have (it can be any of int, long, uint, ulong, float, or double), so we can just
			// range check based on the widest integral type we have: long (then ulong if it fails).

			// Although basic literals can be turned into floats and doubles, you can't make a float
			// or double out of the range of +/-9.2 quintillion as a basic numeric literal. You need
			// any of the floating literals to do that.

			// ReSharper disable once ArrangeMethodOrOperatorBody
			return long.TryParse(word, out _) || ulong.TryParse(word, out _);
		}

		/// <summary>
		/// Determines if a given string is a basic hexadecimal literal (0x, then one or more digits or hex digits).
		/// </summary>
		/// <param name="word">The word to check.</param>
		/// <returns>True if <paramref name="word"/> is a basic hexadecimal literal, false if it's not.</returns>
		public static bool IsBasicHexadecimalLiteral(this string word)
		{
			// We expect an 0x here, but the only method that can actually parse it with the 0x is
			// Convert.ToInt64, which will throw on failure. So we'll strip it out to pass it to
			// long.TryParse instead.

			if (!word.ToLowerInvariant().StartsWith("0x")) { return false; }

			string hexDigitsOnly = word.Substring(2);
			return long.TryParse(hexDigitsOnly, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture,
				       out _)
			       || ulong.TryParse(hexDigitsOnly, NumberStyles.AllowHexSpecifier,
				       CultureInfo.InvariantCulture, out _);
		}

		/// <summary>
		/// Determines if a given string is a suffixed numeric literal (one or more digits, then one of
		/// u, ul, or l).
		/// </summary>
		/// <param name="word">The word to check.</param>
		/// <returns>True if <paramref name="word"/> is a suffixed numeric literal, false if it's not.</returns>
		public static bool IsSuffixedNumericLiteral(this string word)
		{
			string lowercase = word.ToLowerInvariant();
			if (lowercase.EndsWith("u") || lowercase.EndsWith("l"))
			{
				lowercase = lowercase.Substring(0, lowercase.Length - 1);
			}
			else if (lowercase.EndsWith("ul")) { lowercase = lowercase.Substring(0, lowercase.Length); }
			else { return false; }

			return long.TryParse(lowercase, out _) || ulong.TryParse(lowercase, out _);
		}

		/// <summary>
		/// Determines if a given string is a suffixed hexadecimal literal (0x, then one or more digits or hex digits,
		/// then either u, ul, or l).
		/// </summary>
		/// <param name="word">The word to check.</param>
		/// <returns>True if <paramref name="word"/> is a suffixed hexadecimal literal, false if it's not.</returns>
		public static bool IsSuffixedHexadecimalLiteral(this string word)
		{
			string lowercase = word.ToLowerInvariant();
			if (lowercase.EndsWith("u") || lowercase.EndsWith("l"))
			{
				lowercase = lowercase.Substring(0, lowercase.Length - 1);
			}
			else if (lowercase.EndsWith("ul")) { lowercase = lowercase.Substring(0, lowercase.Length); }
			else { return false; }

			return lowercase.IsBasicHexadecimalLiteral();
		}

		/// <summary>
		/// Determines if a given string is a floating literal with a decimal point (one or more digits,
		/// a decimal point, then one or more digits).
		/// </summary>
		/// <param name="word">The word to check.</param>
		/// <returns>True if <paramref name="word"/> is a basic hexadecimal literal, false if it's not.</returns>
		public static bool IsFloatingLiteralWithDecimal(this string word) => double.TryParse(word, out _)
		    && !word.ToLowerInvariant().Contains("e");

		/// <summary>
		/// Determines if a given string is a suffixed floating literal (either a basic numeric literal,
		/// or a floating literal with a decimal point, followed by either f or d).
		/// </summary>
		/// <param name="word">The word to check.</param>
		/// <returns>True if <paramref name="word"/> is a suffixed floating literal, false if it's not.</returns>
		public static bool IsSuffixedFloatingLiteral(this string word)
		{
			string lowercase = word.ToLowerInvariant();
			if (!lowercase.EndsWith("f") && !lowercase.EndsWith("d")) { return false; }

			string literalWithoutSuffix = lowercase.Substring(0, lowercase.Length - 1);

			return IsBasicNumericLiteral(literalWithoutSuffix) ||
			       IsFloatingLiteralWithDecimal(literalWithoutSuffix);
		}

		public static bool IsFloatingLiteralWithExponent(this string word)
			=> double.TryParse(word, out _);

		public static bool IsIdentifierOrLiteral(this string word, bool allowReservedWords = false)
			=> word.IsIdentifier(allowReservedWords) || word.IsBasicNumericLiteral() || word.IsBasicHexadecimalLiteral() ||
		       word.IsSuffixedNumericLiteral() || word.IsSuffixedHexadecimalLiteral() || word.IsFloatingLiteralWithDecimal() ||
		       word.IsSuffixedFloatingLiteral() || word.IsFloatingLiteralWithExponent();

		[Obsolete]
		public static string RemoveComments(this string input)
		{
			StringBuilder result = new StringBuilder();				// Stores the uncommented version of the file.
			CommentKind currentCommentKind = CommentKind.NoComment;	// Keeps track of what kind of comment we're in, if any

			for (int i = 0; i < input.Length; i++)
			{
				char current = input[i];
				char last = (i > 0) ? input[i - 1] : '\0';
				char next = (i < input.Length - 1) ? input[i + 1] : '\0';
 
				switch (current)
				{
					case '/' when last == '/' || last == '*':
						continue;
					case '/' when next == '/':
						currentCommentKind = CommentKind.SingleLine;
						break;
					case '/' when next == '*':
						currentCommentKind = CommentKind.MultipleLines;
						break;
					case '/':
						result.Append(current);
						break;
					case '*' when last == '/':
						continue;
					case '*' when next == '/':
						currentCommentKind = CommentKind.NoComment;
						break;
					case '*' when currentCommentKind == CommentKind.NoComment:
						result.Append(current);
						break;
					case '\r':
					case '\n':
						if (currentCommentKind == CommentKind.SingleLine)
						{
							currentCommentKind = CommentKind.NoComment;
						}
						result.Append(current);
						break;
					default:
						if (currentCommentKind == CommentKind.NoComment)
						{
							result.Append(current);
						}
						break;
				}
			}

			return result.ToString();
		}

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

		public static string Substring(this string s, int startIndex, int endIndex)
		{
			int length = endIndex - startIndex;
			return s.Substring(startIndex, length);
		}

		public static void AppendLineWithIndent(this StringBuilder builder, string line, int indent)
		{
			builder.Append(new string(' ', indent));
			builder.AppendLine(line);
		}

		private enum CommentKind
		{
			NoComment,
			SingleLine,
			MultipleLines
		}
	}
}
