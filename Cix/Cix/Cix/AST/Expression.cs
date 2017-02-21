using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	/// <summary>
	/// A list of expression elements (operators and operands) stored in postfix form.
	/// For instance, (3 + 2) * 5 becomes 3 2 + 5 *
	/// </summary>
	public sealed class Expression : ExpressionElement
	{
		private List<ExpressionElement> elements;
		public IReadOnlyList<ExpressionElement> Elements => elements.AsReadOnly();

		public Expression(IEnumerable<ExpressionElement> elements)
		{
			this.elements = elements.ToList();
		}

		public static IEnumerable<ExpressionElement> FromTokenStream(TokenEnumerator enumerator)
		{
			throw new NotImplementedException();

			// NOTE: When transforming array accesses into ExpressionArrayAccess, take care to set the instance as Infix.
		}

		public static Expression Parse(IEnumerable<ExpressionElement> infixElements)
		{
			throw new NotImplementedException();
			/*
			 * WYLO: Here we go.
			 * For each infix element e,
			 *	1. If e is an operand (ExpressionMemberAccess, NumericLiteral, StringLiteral (write that)), push it onto an output queue.
			 *	2. If e is a fundamental type name or pointer to any type, it is an ExpressionTypeCast and is an operator with precedence 10. Follow operator rules below.
			 *  3. If e is an operator that is left-associative,
			 *		a. Consider the operator currently at the top of the operator stack called f. If there is no such operator, push e onto the operator stack.
			 *		b. If the precedence of e is less than or equal to the precedence of f, pop f onto the output queue and push e onto the operator stack.
			 *  4. If e is an operator that is right-associative, pop f off the operator stack iff e has precedence strictly less than f and push e onto the operator stack.
			 *	5. In all other operator cases, push e on the operator stack.
			 *	6. Push all left-parens on the stack.
			 *	7. If we encounter a right-paren, pop all operators up to but excluding the matching left-paren off the stack. Dispose of both parentheses.
			 *	8. FIGURE OUT ARRAY ACCESS
			 *	9. Functions contain all their own information within an ExpressionFunctionCall and do not have their arguments on any stack.
			 *	10. Once we run out of tokens, pop all remaining operators onto the output queue.
			 *
			 * Once more, with feeling!
			 * 1. Create a result List<ExpressionElement> and a Stack<ExpressionElement> for operators.
			 * For each infix element e,
			 *	1. If e is an operand (numeric literal or string literal), add it to the result list.
			 *	2. If e is an identifier of any kind, add it to the result list. (NOTE: Typecasts should be transformed into an ExpressionTypecast in the FromTokenStream method.)
			 *  3. If e is a left-associative operator,
			 *		a. Consider the operator currently at the top of the operator stack called f. If there is no such operator, push e onto the operator stack.
			 *		b. If the precedence of e is less than or equal to the precedence of f, pop f onto the output queue and push e onto the operator stack.
			 *	4. If e is an operator that is right-associative, pop f off of the operator stack iff f exists and e has a precedence less than f. Push e onto the operator stack.
			 *	5. In all other cases, push e onto the operator stack.
			 *	6. Push all left-parens on the stack.
			 *	7. If we encounter a right-paren, pop all operators up to but excluding the matching left-paren off the stack. Dispose of both parentheses.
			 *  8. If we encounter an array access, it has already been transformed into an infix ExpressionArrayAccess. Call this method on its elements and construct a new ExpressionArrayAccess in postfix form.
			 *	9. If we encounter a function call, its parameters have already been transformed into infix ExpressionFunctionParameters. Call this method on their elements and construct new ExpressionFunctionParamets in postfix form, setting the items in ExpressionFunctionCall.Parameters properly.
			 *	10. Once we run out of tokens, pop all remaining operators onto the output queue.
			 */
		}
	}
}
