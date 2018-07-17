using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cix.Parser;

namespace Cix.AST.Generator.IntermediateForms
{
	public sealed class IntermediateStructMember
	{
		public string TypeName { get; }
		public string Name { get; }
		public int PointerLevel { get; }
		public int ArraySize { get; }
		
		// The following is not the cleanest way for this type to remember what token it's
		// associated with. We need to know that to be able to place errors with this struct
		// member to the exact token it occurs. Due to the way we enumerate struct members, though
		// we can't just keep track of the token indices like we can with intermediate structs.
		// So we need to remember the file path and line number instead.
		public string SourceFilePath { get; }
		public int SourceLineNumber { get; }

		public IntermediateStructMember(string typeName, string name, int pointerLevel, int arraySize,
			Token typeToken)
		{
			TypeName = typeName;
			Name = name;
			PointerLevel = pointerLevel;
			ArraySize = arraySize;
			SourceFilePath = typeToken.FilePath;
			SourceLineNumber = typeToken.LineNumber;
		}
	}
}
