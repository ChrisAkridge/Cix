using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST
{
	public sealed class FunctionPointerType : DataType
	{
		private readonly List<DataType> parameterTypes;

		public DataType ReturnType { get; }
		public IReadOnlyList<DataType> ParameterTypes => parameterTypes.AsReadOnly();

		public override string Name
		{
			get
			{
				string parameterTypesString = (parameterTypes.Any())
					? ", " + string.Join(", ", parameterTypes.Select(p => p.ToString()))
					: "";
				return $"@funcptr<{ReturnType}{parameterTypesString}>";
			}
		}

		public override int Size => 8;

		public FunctionPointerType(DataType returnType, IEnumerable<DataType> parameterTypes,
			int pointerLevel) : base("", pointerLevel, 8)
		{
			ReturnType = returnType;

			this.parameterTypes = parameterTypes.ToList();
		}

		public override DataType WithPointerLevel(int pointerLevel) => new FunctionPointerType(ReturnType, ParameterTypes, pointerLevel);
	}
}
