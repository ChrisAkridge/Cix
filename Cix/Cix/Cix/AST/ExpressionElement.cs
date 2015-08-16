using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public class ExpressionElement : Element
	{
		// this probably should be abstract

		public static List<ExpressionElement> ToElements(IEnumerable<Token> tokens)
		{
			List<ExpressionElement> result = new List<ExpressionElement>();
			foreach (Token token in tokens)
			{
				if (token.Word.IsNumericLiteral())
				{
					result.Add(NumericLiteral.Parse(token.Word));
					continue;
				}

				switch (token.Type)
				{
					case TokenType.Identifier:
						// WYLO: TYPECASTS AND FUNCTIONS, FOOL
						/* Okay, but seriously now.
						 * Names aren't particularly difficult if we keep a nametable and pass it around.
						 * If we have a type name (including pointers), it can be resolved in two ways: 
						 * as a typecast (as LeftParen, Identifier, RightParen), or as a SizeOf operation
						 * (OperatorSizeOf, which you must add, LeftParen, Identifier, RightParen). If we
						 * know the size of our type, we can substitute in here as a numeric literal.
						 * 
						 * If we have a function name (or a local variable with type func_ptr [which you
						 * also must add]), it's either a dereference (OpDereference, Identifier) or it's
						 * a call (Identifier, LeftParen, args, RightParen). Do some syntactic matching to
						 * ensure our parentheses balance, split everything between the parens by commas,
						 * create an ExpressionFunctionCall with the arguments ToElements-ed and parsed
						 * properly.
						 *
						 * If we have a local variable, we can merely add it as a ExpressionMemberAccess.
						 * Also, scope issues need to be taken care of.
						 */
						break;
					case TokenType.OpenParen:
						result.Add(new ExpressionParentheses(ParenthesesType.Left));
						break;
					case TokenType.CloseParen:
						result.Add(new ExpressionParentheses(ParenthesesType.Right));
						break;
					case TokenType.OpIdentity:
						result.Add(new ExpressionOperator(ExpressionOperators.UnaryIdentity));
						break;
					case TokenType.OpInverse:
						result.Add(new ExpressionOperator(ExpressionOperators.UnaryInverse));
						break;
					case TokenType.OpLogicalNOT:
						result.Add(new ExpressionOperator(ExpressionOperators.UnaryLogicalNOT));
						break;
					case TokenType.OpBitwiseNOT:
						result.Add(new ExpressionOperator(ExpressionOperators.UnaryBitwiseNOT));
						break;
					case TokenType.OpPreincrement:
						result.Add(new ExpressionOperator(ExpressionOperators.UnaryPreincrement));
						break;
					case TokenType.OpPostincrement:
						result.Add(new ExpressionOperator(ExpressionOperators.UnaryPostincrement));
						break;
					case TokenType.OpPredecrement:
						result.Add(new ExpressionOperator(ExpressionOperators.UnaryPredecrement));
						break;
					case TokenType.OpPostdecrement:
						result.Add(new ExpressionOperator(ExpressionOperators.UnaryPostdecrement));
						break;
					case TokenType.OpPointerDereference:
						result.Add(new ExpressionOperator(ExpressionOperators.UnaryDerefence));
						break;
					case TokenType.OpVariableDereference:
						result.Add(new ExpressionOperator(ExpressionOperators.UnaryAddressOf));
						break;
					case TokenType.OpMemberAccess:
						result.Add(new ExpressionOperator(ExpressionOperators.BinaryMemberAccess));
						break;
					case TokenType.OpPointerMemberAccess:
						result.Add(new ExpressionOperator(ExpressionOperators.BinaryPointerMemberAccess));
						break;
					case TokenType.OpMultiply:
						result.Add(new ExpressionOperator(ExpressionOperators.BinaryMultiplication));
						break;
					case TokenType.OpDivide:
						result.Add(new ExpressionOperator(ExpressionOperators.BinaryDivision));
						break;
					case TokenType.OpModulusDivide:
						result.Add(new ExpressionOperator(ExpressionOperators.BinaryModulus));
						break;
					case TokenType.OpAdd:
						result.Add(new ExpressionOperator(ExpressionOperators.BinaryAddition));
						break;
					case TokenType.OpSubtract:
						result.Add(new ExpressionOperator(ExpressionOperators.BinarySubtraction));
						break;
					case TokenType.OpShiftLeft:
						result.Add(new ExpressionOperator(ExpressionOperators.BinaryShiftLeft));
						break;
					case TokenType.OpShiftRight:
						result.Add(new ExpressionOperator(ExpressionOperators.BinaryShiftRight));
						break;
					case TokenType.OpLessThan:
						result.Add(new ExpressionOperator(ExpressionOperators.BinaryLessThan));
						break;
					case TokenType.OpLessThanOrEqualTo:
						result.Add(new ExpressionOperator(ExpressionOperators.BinaryLessThanOrEqualTo));
						break;
					case TokenType.OpGreaterThan:
						result.Add(new ExpressionOperator(ExpressionOperators.BinaryGreaterThan));
						break;
					case TokenType.OpGreaterThanOrEqualTo:
						result.Add(new ExpressionOperator(ExpressionOperators.BinaryGreaterThanOrEqualTo));
						break;
					case TokenType.OpEqualTo:
						result.Add(new ExpressionOperator(ExpressionOperators.BinaryEquality));
						break;
					case TokenType.OpNotEqualTo:
						result.Add(new ExpressionOperator(ExpressionOperators.BinaryInequality));
						break;
					case TokenType.OpBitwiseAND:
						result.Add(new ExpressionOperator(ExpressionOperators.BinaryBitwiseAND));
						break;
					case TokenType.OpBitwiseOR:
						result.Add(new ExpressionOperator(ExpressionOperators.BinaryBitwiseOR));
						break;
					case TokenType.OpBitwiseXOR:
						result.Add(new ExpressionOperator(ExpressionOperators.BinaryBitwiseXOR));
						break;
					case TokenType.OpLogicalAND:
						result.Add(new ExpressionOperator(ExpressionOperators.BinaryLogicalAND));
						break;
					case TokenType.OpLogicalOR:
						result.Add(new ExpressionOperator(ExpressionOperators.BinaryLogicalOR));
						break;
					case TokenType.OpAssign:
						result.Add(new ExpressionOperator(ExpressionOperators.BinaryAssignment));
						break;
					case TokenType.OpAddAssign:
						result.Add(new ExpressionOperator(ExpressionOperators.BinaryAddAssign));
						break;
					case TokenType.OpSubtractAssign:
						result.Add(new ExpressionOperator(ExpressionOperators.BinarySubAssign));
						break;
					case TokenType.OpMultiplyAssign:
						result.Add(new ExpressionOperator(ExpressionOperators.BinaryMultAssign));
						break;
					case TokenType.OpDivideAssign:
						result.Add(new ExpressionOperator(ExpressionOperators.BinaryDivAssign));
						break;
					case TokenType.OpModulusDivideAssign:
						result.Add(new ExpressionOperator(ExpressionOperators.BinaryModAssign));
						break;
					case TokenType.OpShiftRightAssign:
						result.Add(new ExpressionOperator(ExpressionOperators.BinaryShiftRightAssign));
						break;
					case TokenType.OpShiftLeftAssign:
						result.Add(new ExpressionOperator(ExpressionOperators.BinaryShiftLeftAssign));
						break;
					case TokenType.OpBitwiseANDAssign:
						result.Add(new ExpressionOperator(ExpressionOperators.BinaryANDAssign));
						break;
					case TokenType.OpBitwiseORAssign:
						result.Add(new ExpressionOperator(ExpressionOperators.BinaryORAssign));
						break;
					case TokenType.OpBitwiseXORAssign:
						result.Add(new ExpressionOperator(ExpressionOperators.BinaryXORAssign));
						break;
					case TokenType.OpTernaryAfterCondition:
						throw new ArgumentException("The ternary conditional operators are not supported.");
					case TokenType.OpTernaryAfterTrueExpression:
						throw new ArgumentException("The ternary conditional operators are not supported.");
					case TokenType.Indeterminate:
						// WYLO: YOU MUST IMPLEMENT
					default:
						throw new ArgumentException($"A {token.Type} is not a valid expression element.");
				}
			}

			return result;
		}
	}
}
