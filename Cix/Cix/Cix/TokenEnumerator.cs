using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix
{
	public sealed class TokenEnumerator
	{
		private List<Token> tokens;
		private int currentIndex;
		
		public Token Current
		{
			get
			{
				if (currentIndex < 0 || currentIndex >= tokens.Count)
				{
					return null;
				}

				return tokens[currentIndex];
			}
		}

		public bool AtBeginning => currentIndex == 0;
		public bool AtEnd => currentIndex == (tokens.Count - 1);

		public TokenEnumerator(List<Token> tokens)
		{
			if (tokens == null)
			{
				throw new ArgumentNullException("Cannot create a token enumerator with no tokens.");
			}

			this.tokens = tokens;
			currentIndex = 0;
		}

		public bool MoveNext()
		{
			if (currentIndex >= tokens.Count)
			{
				return false;
			}

			currentIndex++;
			return true;
		}

		public bool MoveLast()
		{
			if (currentIndex == 0)
			{
				return false;
			}

			currentIndex--;
			return true;
		}

		public List<Token> MoveStatement()
		{
			List<Token> result = new List<Token>();

			if (currentIndex >= tokens.Count)
			{
				return result;
			}

			if (IsStatementTerminator(Current))
			{
				if (Current.Type == TokenType.Semicolon) { result.Add(Current); }
				MoveNext();
				return result;
			}

			do
			{
				result.Add(Current);
			}   while (MoveNext() && !(IsStatementTerminator(Current)));

			return result;
		}

		public void SkipBlock()
		{
			// First finds the next openscope and then skips to after the next closescope on that level
			if (currentIndex >= tokens.Count) return;

			int scopeLevel = 0;
			while (Current.Type != TokenType.OpenScope)
			{
				// Find the first openscope
				if (!MoveNext()) return;
			}

			while (Current.Type != TokenType.CloseScope && scopeLevel > 0)
			{
				if (Current.Type == TokenType.OpenScope) scopeLevel++;
				else if (Current.Type == TokenType.CloseScope && scopeLevel > 0) scopeLevel--;

				if (!MoveNext()) return;
			}
		}

		/// <summary>
		/// Checks if the current token is of an expected type, and throws if it is not.
		/// </summary>
		/// <param name="expected">The expected type of the token.</param>
		/// <returns>The current token if it is valid.</returns>
		public void Validate(TokenType expected)
		{
			if (Current == null)
			{
				throw new ArgumentOutOfRangeException(nameof(tokens), "Encountered beginning or end of token stream too early");
			}
			else if (Current.Type != expected)
			{
				throw new ArgumentException(String.Format("Invalid token type, expected type {0}, got type {1} (word: \"{2}\"", expected, Current.Type, Current.Word));
			}
		}

		public void ValidateNot(TokenType notExpected)
		{
			if (Current == null) throw new ArgumentOutOfRangeException(nameof(tokens), "Encountered beginning or end of stream too early");
			else if (Current.Type == notExpected) throw new ArgumentException($"Invalid token type, expected anything but {notExpected}, but got it (word: \"{Current.Word}\"");
		}

		public bool MoveNextValidate(TokenType expected)
		{
			bool result = MoveNext();
			Validate(expected);
			return result;
		}

		public bool MoveNextValidateNot(TokenType notExpected)
		{
			bool result = MoveNext();
			ValidateNot(notExpected);
			return result;
		}

		private static bool IsStatementTerminator(Token token)
		{
			return token.Type == TokenType.OpenScope || token.Type == TokenType.CloseScope || token.Type == TokenType.Semicolon;
		}
	}
}
