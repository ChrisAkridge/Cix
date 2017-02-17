using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST.Generator.IntermediateForms
{
	public sealed class IntermediateFunction : Element
	{
		public DataType ReturnType { get; }
		public string Name { get; }
		public List<FunctionArgument> Arguments { get; } = new List<FunctionArgument>();
		
		/// <summary>
		/// Gets the index of the function's openscope.
		/// </summary>
		public int StartTokenIndex { get; }

		/// <summary>
		/// Gets the index of the function's closescope.
		/// </summary>
		public int EndTokenIndex { get; }

		public IntermediateFunction(DataType returnType, string name,
			IEnumerable<FunctionArgument> args, int startTokenIndex, int endTokenIndex)
		{
			if (returnType == null)
			{ throw new ArgumentNullException(nameof(returnType), "The provided return type was null."); }
			else if (string.IsNullOrEmpty(name))
			{ throw new ArgumentException("The provided name was null or empty.", nameof(name)); }
			else if (!name.IsIdentifier())
			{ throw new ArgumentException($"The name \"{name}\" is not a valid identifier.)", nameof(name)); }
			else if (args == null)
			{ throw new ArgumentNullException(nameof(args), "The provided collection of arguments was null."); }
			else if (startTokenIndex < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(startTokenIndex),
				"The provided start token index was negative.");
			}
			else if (endTokenIndex < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(endTokenIndex),
				"The provided end token index was negative.");
			}
			else if (endTokenIndex <= startTokenIndex)
			{
				throw new ArgumentException($"The start token index {startTokenIndex} occurs on or after the end token index {endTokenIndex}");
			}

			ReturnType = returnType;
			Name = name;
			Arguments = args.ToList();
			StartTokenIndex = startTokenIndex;
			EndTokenIndex = endTokenIndex;
		}
	}
}
