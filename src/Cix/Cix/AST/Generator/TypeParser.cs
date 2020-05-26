using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cix.Errors;
using Cix.Parser;

namespace Cix.AST.Generator
{
	internal static class TypeParser
	{
		/// <summary>
		/// Given a token enumerator, parse in a type name.
		/// </summary>
		/// <param name="tokens">A token enumerator with the current element set to a type name.</param>
		/// <returns>A data type with pointer level.</returns>
		/// <remarks>
		/// The enumerator must have its current element on the type name of the type to parse.
		/// This method will move the enumerator to the last token of the data type (i.e. the
		/// type "@funcptr&lt;int, float&gt;" will have the enumerator on the &gt;.
		/// </remarks>
		public static bool TryParseType(TokenEnumerator tokens, IErrorListProvider errorList, out DataType result)
		{
			DataType type;

			if (tokens.Current.Text == "@funcptr")
			{
				if (!TryParseFunctionPointerType(tokens, errorList, out FunctionPointerType functionPointerType))
				{
					result = null;
					return false;
				}
				type = functionPointerType;
			}
			else
			{
				string typeName = tokens.Current.Text;

				if (NameTable.Contains(typeName) && NameTable.Instance[typeName] is DataType)
				{
					type = NameTable.Instance[typeName] as DataType;
				}
				else
				{
					type = new DataType(typeName, 0, 0);
				}
			}

			// Look for asterisks. Move the enumerator back if we don't find any.
			tokens.MoveNext();
			if (!tokens.Current.Text.All(c => c == '*'))
			{
				tokens.MovePrevious();
				result = type;
			}
			else
			{
				result = type.WithPointerLevel(tokens.Current.Text.Length);
			}
			return true;
		}

		private static bool TryParseFunctionPointerType(TokenEnumerator tokens,
			IErrorListProvider errorList, out FunctionPointerType result)
		{
			if (!tokens.MoveNextValidate(TokenType.OpenBracket))
			{
				errorList.AddError(ErrorSource.ASTGenerator, 2,
					$"Expected a token of type OpenBracket, found a token of type {tokens.Current.Type}.",
					tokens.Current.FilePath, tokens.Current.LineNumber);
				result = null;
				return false;
			}

			tokens.MoveNext();
			if (!TryParseType(tokens, errorList, out DataType returnType))
			{
				result = null;
				return false;
			}
			tokens.MoveNext();

			var parameterTypes = new List<DataType>();

			while (tokens.Current.Type == TokenType.Comma)
			{
				tokens.MoveNext();
				if (!TryParseType(tokens, errorList, out DataType parameterType))
				{
					result = null;
					return false;
				}
				parameterTypes.Add(parameterType);
				tokens.MoveNext();
			}

			if (tokens.Current.Type == TokenType.CloseBracket)
			{
				result = new FunctionPointerType(returnType, parameterTypes, 8);
				return true;
			}
			else
			{
				errorList.AddError(ErrorSource.ASTGenerator, 2,
						$"Expected a token of type CloseBracket or Comma, found a token of type {tokens.Current.Type}.",
						tokens.Current.FilePath, tokens.Current.LineNumber);
				result = null;
				return false;
			}
		}
	}
}
