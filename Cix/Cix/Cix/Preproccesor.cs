using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix
{
	public static class Preproccesor
	{
		public static string Preprocess(string filePath, out List<string> definedConstants)
		{
			string basePath = Path.GetDirectoryName(filePath);

			if (!File.Exists(filePath))
			{
				throw new FileNotFoundException(string.Format("The file at {0} was not found."));
			}

			definedConstants = new List<string>();
			Dictionary<string, string> substitutions = new Dictionary<string, string>();
			string[] lines = File.ReadAllLines(filePath);
			List<string> resultLines = new List<string>();

			foreach (string line in lines)
			{
				if (!line.StartsWith("#"))
				{
					continue;
				}

				string[] words = line.Split(' ');

				switch (words[0])
				{
					case "#include":
						if (words.Length != 2)
						{
							throw new Exception(string.Format("Invalid number of words in include directive."));
						}
						string fileName = words[1].Substring(1, words[1].Length - 2);
						resultLines.AddRange(LoadIncludedFile(basePath, fileName));
						break;
					case "#define":
						if (words.Length == 2)
						{
							definedConstants.Add(words[1]);
						}
						else if (words.Length == 3)
						{
							substitutions.Add(words[1], words[2]);
							// TODO: actually do the substitution
							// also validate that valid identifiers are being used
						}
						break;
					case "#undefine":
					case "#ifdef":
					case "#ifndef":
					case "#else":
					case "#endif":
					default:
						break;
				}
			}
		}

		private static string[] LoadIncludedFile(string basePath, string fileName)
		{
			if (!fileName.Contains("."))
			{
				throw new ArgumentException(string.Format("The #included file's name was not valid. Line: #include <{0}>", fileName));
			}
			else if (fileName.Substring(fileName.IndexOf(".") + 1) != "cix")
			{
				throw new ArgumentException(string.Format("The #included file's name must end with a CIX extension. Line: #include <{0}>", fileName));
			}

			string fullPath = Path.Combine(basePath, fileName);
			if (!File.Exists(fullPath))
			{
				throw new FileNotFoundException(string.Format("The #included file at {0} does not exist.", fullPath));
			}

			return File.ReadAllLines(fullPath);
		}
	}
}
