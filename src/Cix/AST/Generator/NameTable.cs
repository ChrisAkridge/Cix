using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.AST.Generator
{
	/// <summary>
	/// Associates names with the thing they represent.
	/// </summary>
	public class NameTable
	{
		/// <summary>
		/// Gets a dictionary where the key is the name and the value is the thing the name
		/// represents.
		/// </summary>
		public Dictionary<string, Element> Names { get; }

		/// <summary>
		/// Gets the singleton instance of this class.
		/// </summary>
		public static NameTable Instance { get; } = new NameTable();

		/// <summary>
		/// Gets an element by its name.
		/// </summary>
		/// <param name="key">The name of the element.</param>
		/// <returns>The element identified by that name.</returns>
		public Element this[string key]
		{
			get
			{
				if (!Names.ContainsKey(key))
				{
					return null;
				}
				return Names[key];
			}
			set => Names[key] = value;
		}

		private NameTable() => Names = new Dictionary<string, Element>
		{
			{"byte", new DataType("byte", 0, 1)},
			{"sbyte", new DataType("sbyte", 0, 1)},
			{"short", new DataType("short", 0, 2)},
			{"ushort", new DataType("ushort", 0, 2)},
			{"int", new DataType("int", 0, 4)},
			{"uint", new DataType("uint", 0, 4)},
			{"long", new DataType("long", 0, 8)},
			{"ulong", new DataType("ulong", 0, 8)},
			{"float", new DataType("float", 0, 4)},
			{"double", new DataType("double", 0, 8)},
			{"char", new DataType("char", 0, 2)},
			{"void", new DataType("void", 0, 1)}
		};

		/// <summary>
		/// Checks if a given name is the name of a primitive type.
		/// </summary>
		/// <param name="typeName">The name of the type to check.</param>
		/// <returns>True if the name identifies a primitive type, false if it does not.</returns>
		public static bool IsPrimitiveType(string typeName) => typeName == "byte" ||
		                                                       typeName == "sbyte" ||
		                                                       typeName == "short" ||
		                                                       typeName == "ushort" ||
		                                                       typeName == "int" ||
		                                                       typeName == "uint" ||
		                                                       typeName == "long" ||
		                                                       typeName == "ulong" ||
		                                                       typeName == "float" ||
		                                                       typeName == "double" ||
		                                                       typeName == "char";
	}
}
