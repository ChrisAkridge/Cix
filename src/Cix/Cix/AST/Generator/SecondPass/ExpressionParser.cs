using Cix.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cix.AST.Generator.SecondPass
{
    internal static class ExpressionParser
    {
        public static Expression Parse(TokenEnumerator tokenEnumerator, IErrorListProvider errors)
        {
            return new Expression(Enumerable.Empty<ExpressionElement>());
        }
    }
}
