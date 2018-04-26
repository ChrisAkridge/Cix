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
		private static NameTable instance = new NameTable();

		/// <summary>
		/// Gets a dictionary where the key is the name and the value is the thing the name
		/// represents.
		/// </summary>
		public Dictionary<string, Element> Names { get; private set; }

		/// <summary>
		/// Gets the singleton instance of this class.
		/// </summary>
		public static NameTable Instance => instance;  // ♥

		/// <summary>
		/// Gets an element by its name.
		/// </summary>
		/// <param name="key">The name of the element.</param>
		/// <returns>The element identified by that name.</returns>
		public Element this[string key]
		{
			get
			{
				if (!Names.ContainsKey(key)) return null;
				return Names[key];
			}
			set
			{
				Names[key] = value;
			}
		}

		private NameTable()
		{
			Names = new Dictionary<string, Element>();

			Names.Add("byte", new DataType("byte", 0, 1));
			Names.Add("sbyte", new DataType("sbyte", 0, 1));
			Names.Add("short", new DataType("short", 0, 2));
			Names.Add("ushort", new DataType("ushort", 0, 2));
			Names.Add("int", new DataType("int", 0, 4));
			Names.Add("uint", new DataType("uint", 0, 4));
			Names.Add("long", new DataType("long", 0, 8));
			Names.Add("ulong", new DataType("ulong", 0, 8));
			Names.Add("float", new DataType("float", 0, 4));
			Names.Add("double", new DataType("double", 0, 8));
			Names.Add("char", new DataType("char", 0, 2));
			Names.Add("void", new DataType("void", 0, 1));
		}

		/// <summary>
		/// Checks if a given name is the name of a primitive type.
		/// </summary>
		/// <param name="typeName">The name of the type to check.</param>
		/// <returns>True if the name identifies a primitive type, false if it does not.</returns>
		public static bool IsPrimitiveType(string typeName)
		{
			return typeName == "byte" ||
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
}
