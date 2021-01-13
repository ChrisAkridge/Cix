using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cix.Errors;
using Cix.Parser;

namespace Cix
{
	public sealed class TokenEnumerator
	{
		private readonly List<Token> tokens;
		private readonly IErrorListProvider errorList;

		public int CurrentIndex { get; private set; }

		public Token Previous
		{
			get
			{
				if (CurrentIndex - 1 < 0 || CurrentIndex - 1 >= tokens.Count)
				{ return null; }

				return tokens[CurrentIndex - 1];
			}
		}

		public Token Current
		{
			get
			{
				if (CurrentIndex < 0 || CurrentIndex >= tokens.Count)
				{
					return null;
				}

				return tokens[CurrentIndex];
			}
		}

		public Token Next
		{
			get
			{
				if (CurrentIndex + 1 < 0 || CurrentIndex + 1 >= tokens.Count)
				{ return null; }

				return tokens[CurrentIndex + 1];
			}
		}

		public Token this[int index] => tokens[index];

		public bool AtBeginning => CurrentIndex == 0;
		public bool AtEnd => CurrentIndex == (tokens.Count - 1);

		public TokenEnumerator(List<Token> tokens, IErrorListProvider errorList)
		{
			this.tokens = tokens ?? throw new ArgumentNullException(nameof(tokens), "Cannot create a token enumerator with no tokens.");
			this.errorList = errorList;
			CurrentIndex = 0;
		}

		public TokenEnumerator Subset(int startIndex, int endIndex)
		{
			if ((startIndex < 0 || startIndex >= tokens.Count) || (endIndex < 0 || endIndex >= tokens.Count))
			{
				throw new ArgumentOutOfRangeException(
					$"The start and/or end indices are out of range. Start index: {startIndex}, end index: {endIndex}, valid range is [0-{tokens.Count}).");
			}

			if (startIndex > endIndex)
			{
				throw new ArgumentOutOfRangeException(
					$"The start index of {startIndex} is greater than the end index of {endIndex}.");
			}
			else if (startIndex == endIndex)
			{
				return new TokenEnumerator(new List<Token>(), errorList);
			}

			int length = endIndex - startIndex;
			return new TokenEnumerator(tokens.Skip(startIndex).Take(length).ToList(), errorList);
		}

		public IEnumerable<List<Token>> SplitOnSemicolon()
		{
			var result = new List<List<Token>>();
			var currentStatement = new List<Token>();

			Reset();

			do
			{
				if (Current.Type == TokenType.Semicolon)
				{
					result.Add(currentStatement);
					currentStatement = new List<Token>();
				}
				else if (Current.Type == TokenType.OpenScope || Current.Type == TokenType.CloseScope)
				{
					result.Add(currentStatement);
					result.Add(new List<Token> { Current });
					currentStatement = new List<Token>();
				}
				else
				{
					currentStatement.Add(Current);
				}
			} while (MoveNext());

			if (currentStatement.Any())
			{
				result.Add(currentStatement);
			}
			return result;
		}

		public IEnumerable<List<Token>> SplitOnComma()
		{
			var result = new List<List<Token>>();
			var currentStatement = new List<Token>();

			Reset();

			do
			{
				if (Current.Type == TokenType.Comma)
				{
					result.Add(currentStatement);
					currentStatement = new List<Token>();
				}
				else
				{
					currentStatement.Add(Current);
				}
			} while (MoveNext());

			if (currentStatement.Any()) { result.Add(currentStatement); }
			return result;
		}

		public bool MoveNext()
		{
			if (CurrentIndex >= tokens.Count - 1)
			{
				return false;
			}

			CurrentIndex++;
			return true;
		}

		public bool MovePrevious()
		{
			if (CurrentIndex == 0)
			{
				return false;
			}

			CurrentIndex--;
			return true;
		}

		public List<Token> MoveNextStatement()
		{
			var result = new List<Token>();

			if (CurrentIndex >= tokens.Count)
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
			if (CurrentIndex >= tokens.Count)
			{
				return;
			}

			while (Current.Type != TokenType.OpenScope)
			{
				// Find the first openscope
				if (!MoveNext())
				{
					return;
				}
			}

			int scopeLevel = 1;
			if (!MoveNext())
			{
				return;
			}

			while (Current.Type != TokenType.CloseScope && scopeLevel > 0)
			{
				switch (Current.Type)
				{
					case TokenType.OpenScope:
						scopeLevel++;
						break;
					case TokenType.CloseScope when scopeLevel > 0:
						scopeLevel--;
						break;
				}

				if (!MoveNext())
				{
					return;
				}
			}

			MoveNext();
		}

		public void Reset()
		{
			CurrentIndex = 0;
		}

		/// <summary>
		/// Checks if the current token is of an expected type, and throws if it is not.
		/// </summary>
		/// <param name="expected">The expected type of the token.</param>
		/// <returns>The current token if it is valid.</returns>
		public bool Validate(TokenType expected)
		{
			if (Current == null)
			{
				Token adjacentExistingToken = Previous ?? Next;
				errorList.AddError(ErrorSource.ASTGenerator, 4, "Unexpected beginning/end of file.",
					adjacentExistingToken.FilePath, adjacentExistingToken.LineNumber);
				return false;
			}
			else if (Current.Type != expected)
			{
				errorList.AddError(ErrorSource.ASTGenerator, 2, $"Expected token of type {expected}, got token of type {Current.Type}.",
					Current.FilePath, Current.LineNumber);
				return false;
			}
			return true;
		}

		public bool ValidateNot(TokenType notExpected)
		{
			if (Current == null)
			{
				Token adjacentExistingToken = Previous ?? Next;
				errorList.AddError(ErrorSource.ASTGenerator, 4, "Unexpected beginning/end of file.",
					adjacentExistingToken.FilePath, adjacentExistingToken.LineNumber);
				return false;
			}
			else if (Current.Type == notExpected)
			{
				errorList.AddError(ErrorSource.ASTGenerator, 3, $"Expected a token of type anything except {notExpected}, but got it anyway.",
					Current.FilePath, Current.LineNumber);
				return false;
			}
			return true;
		}

		public bool MoveNextValidate(TokenType expected)
		{
			MoveNext();
			return Validate(expected);
		}

		public bool MoveNextValidateNot(TokenType notExpected)
		{
			MoveNext();
			return ValidateNot(notExpected);
		}

		// TODO: this should probably be in the Token class
		private static bool IsStatementTerminator(Token token) =>
			token.Type == TokenType.OpenScope || token.Type == TokenType.CloseScope || token.Type == TokenType.Semicolon;
	}
}
