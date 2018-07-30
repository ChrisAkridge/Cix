﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	/// <summary>
	/// Represents an argument in a function declaration indicating that the function may take more than one additional argument.
	/// </summary>
	public sealed class VarargsFunctionArgument : Element
	{
		// TODO: this should derive from FunctionArgument, probably

		public override void Print(StringBuilder builder, int depth)
		{
			builder.AppendLineWithIndent("Argument: ...", depth);
		}
	}
}
