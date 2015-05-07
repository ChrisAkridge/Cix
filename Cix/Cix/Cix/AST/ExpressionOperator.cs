using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class ExpressionOperator : ExpressionElement
	{
		public ExpressionOperators Operator { get; private set; }

		public ExpressionOperator(ExpressionOperators @operator)
		{
			this.Operator = @operator;
		}

		public static int GetOperatorPrecedence(ExpressionOperators op)
		{
			switch (op)
			{
				case ExpressionOperators.UnaryPostincrement:
				case ExpressionOperators.UnaryPostdecrement:
				case ExpressionOperators.BinaryMemberAccess:
				case ExpressionOperators.BinaryPointerMemberAccess:
					return 1;
				case ExpressionOperators.UnaryPreincrement:
				case ExpressionOperators.UnaryPredecrement:
				case ExpressionOperators.UnaryIdentity:
				case ExpressionOperators.UnaryInverse:
				case ExpressionOperators.UnaryLogicalNOT:
				case ExpressionOperators.UnaryBitwiseNOT:
				case ExpressionOperators.UnaryDerefence:
				case ExpressionOperators.UnaryAddressOf:
					return 2;
				case ExpressionOperators.BinaryMultiplication:
				case ExpressionOperators.BinaryDivision:
				case ExpressionOperators.BinaryModulus:
					return 3;
				case ExpressionOperators.BinaryAddition:
				case ExpressionOperators.BinarySubtraction:
					return 4;
				case ExpressionOperators.BinaryShiftLeft:
				case ExpressionOperators.BinaryShiftRight:
					return 5;
				case ExpressionOperators.BinaryLessThan:
				case ExpressionOperators.BinaryLessThanOrEqualTo:
				case ExpressionOperators.BinaryGreaterThan:
				case ExpressionOperators.BinaryGreaterThanOrEqualTo:
					return 6;
				case ExpressionOperators.BinaryBitwiseAND:
					return 7;
				case ExpressionOperators.BinaryBitwiseXOR:
					return 8;
				case ExpressionOperators.BinaryBitwiseOR:
					return 9;
				case ExpressionOperators.BinaryLogicalAND:
					return 10;
				case ExpressionOperators.BinaryLogicalOR:
					return 11;
				case ExpressionOperators.BinaryAssignment:
				case ExpressionOperators.BinaryAddAssign:
				case ExpressionOperators.BinarySubAssign:
				case ExpressionOperators.BinaryMultAssign:
				case ExpressionOperators.BinaryDivAssign:
				case ExpressionOperators.BinaryModAssign:
				case ExpressionOperators.BinaryShiftLeftAssign:
				case ExpressionOperators.BinaryShiftRightAssign:
				case ExpressionOperators.BinaryANDAssign:
				case ExpressionOperators.BinaryORAssign:
				case ExpressionOperators.BinaryXORAssign:
					return 12;
				default:
					return -1;
			}
		}
	}

	public enum ExpressionOperators
	{
		UnaryIdentity,
		UnaryInverse,
		UnaryLogicalNOT,
		UnaryBitwiseNOT,
		UnaryPreincrement,
		UnaryPredecrement,
		UnaryPostincrement,
		UnaryPostdecrement,
		UnaryDerefence,
		UnaryAddressOf,
		BinaryAddition,
		BinarySubtraction,
		BinaryMultiplication,
		BinaryDivision,
		BinaryModulus,
		BinaryEquality,
		BinaryInequality,
		BinaryLessThan,
		BinaryGreaterThan,
		BinaryLessThanOrEqualTo,
		BinaryGreaterThanOrEqualTo,
		BinaryLogicalAND,
		BinaryLogicalOR,
		BinaryBitwiseAND,
		BinaryBitwiseOR,
		BinaryBitwiseXOR,
		BinaryShiftLeft,
		BinaryShiftRight,
		BinaryAssignment,
		BinaryAddAssign,
		BinarySubAssign,
		BinaryMultAssign,
		BinaryDivAssign,
		BinaryModAssign,
		BinaryANDAssign,
		BinaryORAssign,
		BinaryXORAssign,
		BinaryShiftLeftAssign,
		BinaryShiftRightAssign,
		BinaryMemberAccess,
		BinaryPointerMemberAccess
	}
}
