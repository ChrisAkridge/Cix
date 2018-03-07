﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class ExpressionMemberAccess : ExpressionElement
	{
		public string Name { get; }

		public ExpressionMemberAccess(string name)
		{
			Name = name;
		}
	}
}