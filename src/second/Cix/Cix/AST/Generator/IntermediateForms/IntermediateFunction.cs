using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cix.AST.Generator.IntermediateForms
{
	public sealed class IntermediateFunction : Element
	{
		private readonly List<FunctionParameter> parameters = new List<FunctionParameter>();

		public DataType ReturnType { get; }
		public string Name { get; }
		public IReadOnlyList<FunctionParameter> Parameters => parameters.AsReadOnly();

		/// <summary>
		/// Gets the index of the function's openscope.
		/// </summary>
		public int StartTokenIndex { get; }

		/// <summary>
		/// Gets the index of the function's closescope.
		/// </summary>
		public int EndTokenIndex { get; }

		public IntermediateFunction(DataType returnType, string name,
			IEnumerable<FunctionParameter> parameters, int startTokenIndex, int endTokenIndex)
		{
			if (returnType == null)
			{ throw new ArgumentNullException(nameof(returnType), "The provided return type was null."); }
			else if (string.IsNullOrEmpty(name))
			{ throw new ArgumentException("The provided name was null or empty.", nameof(name)); }
			else if (!name.IsIdentifier())
			{ throw new ArgumentException($"The name \"{name}\" is not a valid identifier.)", nameof(name)); }
			else if (parameters == null)
			{ throw new ArgumentNullException(nameof(parameters), "The provided collection of arguments was null."); }
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
				string message = $"The start token index {startTokenIndex} occurs on or after the end token index {endTokenIndex}";
				throw new ArgumentException(message);
			}

			ReturnType = returnType;
			Name = name;
			this.parameters.AddRange(parameters);
			StartTokenIndex = startTokenIndex;
			EndTokenIndex = endTokenIndex;
		}

		public override void Print(StringBuilder builder, int depth)
		{
			builder.AppendLineWithIndent(
				$"Intermediate Function {Name}({string.Join(", ", Parameters.Select(a => a.ToString()))}) starts at {StartTokenIndex}, ends at {EndTokenIndex}",
				depth);
		}
	}
}
