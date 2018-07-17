using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cix.Exceptions;

namespace Cix.AST
{
	public sealed class ConditionalBlock : Element
	{
		private Expression blockCondition;
		private List<Element> blockStatements;

		public IReadOnlyList<Element> BlockStatements => blockStatements.AsReadOnly();

		public ConditionalBlockType BlockType { get; }

		public Expression BlockCondition
		{
			get
			{
				if (BlockType == ConditionalBlockType.IfBlock || BlockType == ConditionalBlockType.ElseIfBlock)
				{
					return blockCondition;
				}
				else
				{
					throw new ASTException("There is no conditional statement for an Else block.");
				}
			}
		}

		public ConditionalBlock(ConditionalBlockType blockType, Expression blockCondition, IEnumerable<Element> statements)
		{
			BlockType = blockType;
			this.blockCondition = blockCondition;
			blockStatements = statements.ToList();
		}

		public override void Print(StringBuilder builder, int depth)
		{
			string line;

			// ReSharper disable once ConvertIfStatementToSwitchStatement
			if (BlockType == ConditionalBlockType.IfBlock) { line = "If"; }
			else if (BlockType == ConditionalBlockType.ElseIfBlock) { line = "Else If"; }
			else { line = "Else"; }

			builder.AppendLineWithIndent(depth, line);
			builder.AppendLineWithIndent(depth + 1, $"Condition: ({blockCondition.ToString()})");
			builder.AppendLineWithIndent(depth + 1, "Statements: {");

			foreach (Element statement in blockStatements)
			{
				statement.Print(builder, depth + 2);
			}

			builder.AppendLineWithIndent(depth + 1, "}");
		}
	}

	public enum ConditionalBlockType
	{
		IfBlock,
		ElseIfBlock,
		ElseBlock
	}
}
