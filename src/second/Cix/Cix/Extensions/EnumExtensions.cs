using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cix.Parser;

namespace Cix.Extensions
{
	public static class EnumExtensions
	{
		public static bool IsNumericLiteralToken(this TokenType type) =>
            (type == TokenType.BasicNumericLiteral)
            || (type == TokenType.BasicHexadecimalLiteral)
            || (type == TokenType.SuffixedNumericLiteral)
            || (type == TokenType.SuffixedHexadecimalLiteral)
            || (type == TokenType.FloatingLiteralWithDecimal)
            || (type == TokenType.SuffixedFloatingLiteral)
            || (type == TokenType.FloatingLiteralWithExponent);
	}
}
