using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class ExpressionOperator : ExpressionElement
	{

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
		BinaryMemberAccess,
		BinaryPointerMemberAccess
	}
}
