using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class ConditionalBlock
	{
		private Expression blockCondition;
		public List<Element> BlockStatements { get; private set; }

		public ConditionalBlockType BlockType { get; private set; }
		
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
					throw new InvalidOperationException("There is no conditional statement for an Else block.");
				}
			}
		}

		public ConditionalBlock(ConditionalBlockType blockType, Expression blockCondition, IEnumerable<Element> statements)
		{
			BlockType = blockType;
			this.blockCondition = blockCondition;
			BlockStatements = statements.ToList();
		}
	}

	public enum ConditionalBlockType
	{
		IfBlock,
		ElseIfBlock,
		ElseBlock
	}
}
