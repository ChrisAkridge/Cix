﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	[Obsolete]
    public sealed class NameToBeResolved : Element
    {
		public string Name { get; }
		public ToBeResolvedType Type { get; }

		public NameToBeResolved(string name, ToBeResolvedType type)
		{
			Name = name;
			Type = type;
		}
    }

	public enum ToBeResolvedType
	{
		DataType,
		Structure,
		Function
	}
}