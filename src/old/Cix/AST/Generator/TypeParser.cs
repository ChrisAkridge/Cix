using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cix.Errors;

namespace Cix.AST.Generator
{
	internal static class TypeParser
	{
		/// <summary>
		/// Given a token enumerator, parse in a type name.
		/// </summary>
		/// <param name="enumerator">A token enumerator with the current element set to a type name.</param>
		/// <returns>A data type with pointer level.</returns>
		/// <remarks>
		/// The enumerator must have its current element on the type name of the type to parse.
		/// This method will move the enumerator to the last token of the data type (i.e. the
		/// type "@funcptr&lt;int, float&gt;" will have the enumerator on the &gt;
		/// </remarks>
		public static DataType ParseType(TokenEnumerator enumerator, IErrorListProvider errorList)
		{
			if (enumerator.Current.Text == "@funcptr")
			{
				return ParseFunctionPointerType(enumerator, errorList);
			}
		}

		private static FunctionPointerType ParseFunctionPointerType(TokenEnumerator enumerator,
			IErrorListProvider errorList)
		{
			// 1. Take up tokens into a list until we hit a closing >. Keep track of our depth to
			//	  ensure that we match the < and > correctly.
			// 2. For each token in the list,
			//	  a. If the token is a comma:
			//		 i. If the last token is a comma, or this is the first/last element, raise an error.
			//		    Else, continue.
			//	     Else, call ParseType on the token. Sadly, we'll need a new TokenEnumerator.
		}
	}
}
