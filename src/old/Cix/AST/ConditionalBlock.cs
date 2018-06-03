using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cix.Exceptions;

namespace Cix.AST
{
	public sealed class ConditionalBlock
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
	}

	public enum ConditionalBlockType
	{
		IfBlock,
		ElseIfBlock,
		ElseBlock
	}
}
