using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cix.Errors;
using Cix.Parser;

namespace Cix.AST.Generator
{
    [Obsolete]
	internal static class LiteralParser
	{
		public static ExpressionConstant Parse(Token token, IErrorListProvider errorList)
		{
			switch (token.Type)
			{
				case TokenType.BasicNumericLiteral: return ParseBasicNumericLiteral(token);
				case TokenType.BasicHexadecimalLiteral: return ParseBasicHexadecimalLiteral(token);
				case TokenType.SuffixedNumericLiteral: return ParseSuffixedNumericLiteral(token, errorList);
				case TokenType.SuffixedHexadecimalLiteral: return ParseSuffixedHexadecimalLiteral(token, errorList);
				case TokenType.FloatingLiteralWithDecimal: return ParseFloatingLiteral(token);
				case TokenType.SuffixedFloatingLiteral: return ParseSuffixedFloatingLiteral(token, errorList);
				case TokenType.FloatingLiteralWithExponent: return ParseFloatingLiteralWithExponent(token);
				case TokenType.StringLiteral: return new ExpressionConstant(token.Text);
				default:
					throw new ArgumentOutOfRangeException(nameof(token.Type),
						$"The token type {token.Type} is not a literal token type.");
			}
		}

		private static ExpressionConstant ParseBasicNumericLiteral(Token token)
		{
			if (int.TryParse(token.Text, out int intResult)) { return new ExpressionConstant(intResult); }
			else if (uint.TryParse(token.Text, out uint uintResult))
			{
				return new ExpressionConstant(uintResult);
			}
			else if (long.TryParse(token.Text, out long longResult))
			{
				return new ExpressionConstant(longResult);
			}
			else if (ulong.TryParse(token.Text, out ulong ulongResult))
			{
				return new ExpressionConstant(ulongResult);
			}
			else
			{
				throw new ArgumentException(
					$"Tried to make a number out of literal {token.Text} but it wasn't a number/in range. Did the tokenizer tokenize this correctly?",
					nameof(token.Text));
			}
		}

		private static ExpressionConstant ParseBasicHexadecimalLiteral(Token token)
			=> ParseBasicHexadecimalLiteral(token.Text);

		private static ExpressionConstant ParseBasicHexadecimalLiteral(string literal)
		{
			const NumberStyles style = NumberStyles.HexNumber;
			CultureInfo info = CultureInfo.InvariantCulture;
			string without0x = literal.Substring(2);

			if (int.TryParse(without0x, style, info, out int intResult))
			{
				return new ExpressionConstant(intResult);
			}
			else if (uint.TryParse(without0x, style, info, out uint uintResult))
			{
				return new ExpressionConstant(uintResult);
			}
			else if (long.TryParse(without0x, style, info, out long longResult))
			{
				return new ExpressionConstant(longResult);
			}
			else if (ulong.TryParse(without0x, style, info, out ulong ulongResult))
			{
				return new ExpressionConstant(ulongResult);
			}
			else
			{
				throw new ArgumentException(
					$"Tried to make a number out of literal {literal} but it wasn't a number/in range. Did the tokenizer tokenize this correctly?",
					nameof(literal));
			}
		}

		private static ExpressionConstant ParseSuffixedNumericLiteral(Token token, IErrorListProvider errorList)
		{
			string lowercase = token.Text.ToLowerInvariant();

			if (lowercase.EndsWith("ul"))
			{
				string withoutSuffix = lowercase.Substring(0, lowercase.Length - 3);
				if (ulong.TryParse(withoutSuffix, out ulong result)) { return new ExpressionConstant(result); }
				else
				{
					throw new ArgumentException(
						$"Tried to make a number out of literal {token.Text} but it wasn't a number/in range. Did the tokenizer tokenize this correctly?",
						nameof(token.Text));
				}
			}
			else if (lowercase.EndsWith("l"))
			{
				string withoutSuffix = lowercase.Substring(0, lowercase.Length - 1);
				if (long.TryParse(withoutSuffix, out long result)) { return new ExpressionConstant(result); }
				else
				{
					errorList.AddError(ErrorSource.ASTGenerator, 24,
						$"The numeric literal {token.Text} is supposed to be a long but is out of range or invalid.",
						token.FilePath, token.LineNumber);
					return new ExpressionConstant(-1L);
				}
			}
			else if (lowercase.EndsWith("u"))
			{
				string withoutSuffix = lowercase.Substring(0, lowercase.Length - 1);
				if (uint.TryParse(withoutSuffix, out uint result)) { return new ExpressionConstant(result); }
				else
				{
					errorList.AddError(ErrorSource.ASTGenerator, 25,
						$"The numeric literal {token.Text} is supposed to be a uint but is out of range or invalid.",
						token.FilePath, token.LineNumber);
					return new ExpressionConstant(-1U);
				}
;			}
			else
			{
				throw new ArgumentException(
					$"Tried to make a number out of literal {token.Text} but it wasn't a number/in range. Did the tokenizer tokenize this correctly?",
					nameof(token.Text));
			}
		}

		private static ExpressionConstant ParseSuffixedHexadecimalLiteral(Token token,
			IErrorListProvider errorList)
		{
			string without0x = token.Text.ToLowerInvariant().Substring(2);

			if (without0x.EndsWith("ul"))
			{
				string withoutSuffix = without0x.Substring(0, without0x.Length - 2);

				// The below line can throw if the literal isn't valid, but we'd just throw the
				// same exception anyway, so we'll let it propagate.
				return ParseBasicHexadecimalLiteral(withoutSuffix);
			}
			else if (without0x.EndsWith("l"))
			{
				string withoutSuffix = without0x.Substring(0, without0x.Length - 2);
				ExpressionConstant constant = ParseBasicHexadecimalLiteral(withoutSuffix);
				string typeName = constant.Type.Name;

				if (typeName != "long" || typeName != "uint" || typeName != "int")
				{
					errorList.AddError(ErrorSource.ASTGenerator, 24,
						$"The numeric literal {token.Text} is supposed to be a long but is out of range or invalid.",
						token.FilePath, token.LineNumber);
					return new ExpressionConstant(-1L);
				}

				return constant;
			}
			else if (without0x.EndsWith("u"))
			{
				string withoutSuffix = without0x.Substring(0, without0x.Length - 2);
				ExpressionConstant constant = ParseBasicHexadecimalLiteral(withoutSuffix);
				string typeName = constant.Type.Name;

				if (typeName != "uint")
				{
					errorList.AddError(ErrorSource.ASTGenerator, 25,
						$"The numeric literal {token.Text} is supposed to be a uint but is out of range or invalid.",
						token.FilePath, token.LineNumber);
					return new ExpressionConstant(-1U);
				}

				return constant;
			}
			else
			{
				throw new ArgumentException(
					$"Tried to make a number out of literal {token.Text} but it wasn't a number/in range. Did the tokenizer tokenize this correctly?",
					nameof(token.Text));
			}
		}

		private static ExpressionConstant ParseFloatingLiteral(Token token)
		{
			if (double.TryParse(token.Text, out double result)) { return new ExpressionConstant(result); }
			else
			{
				throw new ArgumentException(
					$"Tried to make a number out of literal {token.Text} but it wasn't a number/in range. Did the tokenizer tokenize this correctly?",
					nameof(token.Text));
			}
		}

		private static ExpressionConstant ParseSuffixedFloatingLiteral(Token token,
			IErrorListProvider errorList)
		{
			char suffix = token.Text[token.Text.Length - 1];
			string withoutSuffix = token.Text.Substring(0, token.Text.Length - 1);

			if (suffix == 'f')
			{
				if (float.TryParse(withoutSuffix, out float result)) { return new ExpressionConstant(result); }
				else
				{
					errorList.AddError(ErrorSource.ASTGenerator, 26,
						$"The numeric literal {token.Text} is supposed to be a float but is out of range or invalid.",
						token.FilePath, token.LineNumber);
					return new ExpressionConstant(float.NaN);
				}
			}
			else { return ParseFloatingLiteral(token); }
		}

		private static ExpressionConstant ParseFloatingLiteralWithExponent(Token token) =>
			ParseFloatingLiteral(token);
	}
}
