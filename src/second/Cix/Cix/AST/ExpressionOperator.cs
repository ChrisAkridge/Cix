using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class ExpressionOperator : ExpressionElement
	{
		public ExpressionOperators Operator { get; }

		public ExpressionOperator(ExpressionOperators @operator)
		{
			Operator = @operator;
		}

		public static int GetOperatorPrecedence(ExpressionOperators op)
		{
			switch (op)
			{
				case ExpressionOperators.UnaryPostincrement:
				case ExpressionOperators.UnaryPostdecrement:
				case ExpressionOperators.UnaryArrayAccess:
				case ExpressionOperators.BinaryMemberAccess:
				case ExpressionOperators.BinaryPointerMemberAccess:
					return 13;
				case ExpressionOperators.UnaryPreincrement:
				case ExpressionOperators.UnaryPredecrement:
				case ExpressionOperators.UnaryIdentity:
				case ExpressionOperators.UnaryInverse:
				case ExpressionOperators.UnaryLogicalNOT:
				case ExpressionOperators.UnaryBitwiseNOT:
				case ExpressionOperators.UnaryDerefence:
				case ExpressionOperators.UnaryAddressOf:
					return 12;
				case ExpressionOperators.BinaryMultiplication:
				case ExpressionOperators.BinaryDivision:
				case ExpressionOperators.BinaryModulus:
					return 11;
				case ExpressionOperators.BinaryAddition:
				case ExpressionOperators.BinarySubtraction:
					return 10;
				case ExpressionOperators.BinaryShiftLeft:
				case ExpressionOperators.BinaryShiftRight:
					return 9;
				case ExpressionOperators.BinaryLessThan:
				case ExpressionOperators.BinaryLessThanOrEqualTo:
				case ExpressionOperators.BinaryGreaterThan:
				case ExpressionOperators.BinaryGreaterThanOrEqualTo:
					return 8;
				case ExpressionOperators.BinaryBitwiseAND:
					return 7;
				case ExpressionOperators.BinaryBitwiseXOR:
					return 6;
				case ExpressionOperators.BinaryBitwiseOR:
					return 5;
				case ExpressionOperators.BinaryLogicalAND:
					return 4;
				case ExpressionOperators.BinaryLogicalOR:
					return 3;
				case ExpressionOperators.TernaryTrueBranch:
				case ExpressionOperators.TernaryFalseBranch:
                    return 2;
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
					return 1;
				default:
					return -1;
			}
		}

		public override string ToString()
		{
			switch (Operator)
			{
				case ExpressionOperators.UnaryIdentity: return "+";
				case ExpressionOperators.UnaryInverse: return "-";
				case ExpressionOperators.UnaryLogicalNOT: return "!";
				case ExpressionOperators.UnaryBitwiseNOT: return "~";
				case ExpressionOperators.UnaryPreincrement: return "++";
				case ExpressionOperators.UnaryPredecrement: return "--";
				case ExpressionOperators.UnaryPostincrement: return "++";
				case ExpressionOperators.UnaryPostdecrement: return "--";
				case ExpressionOperators.UnaryDerefence: return "*";
				case ExpressionOperators.UnaryAddressOf: return "&";
				case ExpressionOperators.BinaryAddition: return "+";
				case ExpressionOperators.BinarySubtraction: return "-";
				case ExpressionOperators.BinaryMultiplication: return "*";
				case ExpressionOperators.BinaryDivision: return "/";
				case ExpressionOperators.BinaryModulus: return "%";
				case ExpressionOperators.BinaryEquality: return "==";
				case ExpressionOperators.BinaryInequality: return "!=";
				case ExpressionOperators.BinaryLessThan: return "<";
				case ExpressionOperators.BinaryGreaterThan: return ">";
				case ExpressionOperators.BinaryLessThanOrEqualTo: return "<=";
				case ExpressionOperators.BinaryGreaterThanOrEqualTo: return ">=";
				case ExpressionOperators.BinaryLogicalAND: return "&&";
				case ExpressionOperators.BinaryLogicalOR: return "||";
				case ExpressionOperators.BinaryBitwiseAND: return "&";
				case ExpressionOperators.BinaryBitwiseOR: return "|";
				case ExpressionOperators.BinaryBitwiseXOR: return "^";
				case ExpressionOperators.BinaryShiftLeft: return "<<";
				case ExpressionOperators.BinaryShiftRight: return ">>";
				case ExpressionOperators.BinaryAssignment: return "=";
				case ExpressionOperators.BinaryAddAssign: return "+=";
				case ExpressionOperators.BinarySubAssign: return "-=";
				case ExpressionOperators.BinaryMultAssign: return "*=";
				case ExpressionOperators.BinaryDivAssign: return "/=";
				case ExpressionOperators.BinaryModAssign: return "%=";
				case ExpressionOperators.BinaryANDAssign: return "&=";
				case ExpressionOperators.BinaryORAssign: return "|=";
				case ExpressionOperators.BinaryXORAssign: return "^=";
				case ExpressionOperators.BinaryShiftLeftAssign: return "<<=";
				case ExpressionOperators.BinaryShiftRightAssign: return ">>=";
				case ExpressionOperators.BinaryMemberAccess: return ".";
				case ExpressionOperators.BinaryPointerMemberAccess: return "->";
				case ExpressionOperators.UnaryArrayAccess: return "[]";
				case ExpressionOperators.TernaryTrueBranch: return "?";
				case ExpressionOperators.TernaryFalseBranch: return ":";
				default: throw new ArgumentOutOfRangeException();
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
		BinaryPointerMemberAccess,
		UnaryArrayAccess,
        TernaryTrueBranch,
        TernaryFalseBranch
	}
}
