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
				if (this.currentIndex < 0 || this.currentIndex >= this.tokens.Count)
				{
					return null;
				}

				return this.tokens[currentIndex];
			}
		}

		public TokenEnumerator(List<Token> tokens)
		{
			if (tokens == null)
			{
				throw new ArgumentNullException("Cannot create a token enumerator with no tokens.");
			}

			this.tokens = tokens;
			this.currentIndex = 0;
		}

		public bool MoveNext()
		{
			if (this.currentIndex >= this.tokens.Count)
			{
				return false;
			}

			this.currentIndex++;
			return true;
		}

		public bool MoveLast()
		{
			if (this.currentIndex == 0)
			{
				return false;
			}

			this.currentIndex--;
			return false;
		}

		public List<Token> MoveStatement()
		{
			List<Token> result = new List<Token>();

			if (this.currentIndex >= this.tokens.Count)
			{
				return result;
			}

			if (IsStatementTerminator(this.Current))
			{
				result.Add(this.Current);
				this.MoveNext();
				return result;
			}

			do
			{
				result.Add(this.Current);
			}   while (this.MoveNext() && !(IsStatementTerminator(this.Current)));

			return result;
		}

		public void SkipBlock()
		{
			// First finds the next openscope and then skips to after the next closescope on that level
			if (this.currentIndex >= this.tokens.Count) return;

			int scopeLevel = 0;
			while (this.Current.Type != TokenType.OpenScope)
			{
				// Find the first openscope
				if (!this.MoveNext()) return;
			}

			while (this.Current.Type != TokenType.CloseScope && scopeLevel > 0)
			{
				if (this.Current.Type == TokenType.OpenScope) scopeLevel++;
				else if (this.Current.Type == TokenType.CloseScope && scopeLevel > 0) scopeLevel--;

				if (!this.MoveNext()) return;
			}
		}

		private static bool IsStatementTerminator(Token token)
		{
			return token.Type == TokenType.OpenScope || token.Type == TokenType.CloseScope || token.Type == TokenType.Semicolon;
		}
	}
}
