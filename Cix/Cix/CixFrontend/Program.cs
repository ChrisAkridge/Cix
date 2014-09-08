using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cix;

namespace CixFrontend
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Cix Platform");
			Console.WriteLine("Copyright © 2014 Chris Akridge.");
			Console.WriteLine("Licensed under the MIT Public License.");

			if (args.Length < 1)
			{
				Console.WriteLine("Usage: cixfrontend <file-path> /flag1 /flag2 ...");
				Console.ReadKey();
				return;
			}

			string filePath = args[0];

			if (!File.Exists(filePath))
			{
				Console.WriteLine("File Not Found: The file at {0} does not exist.", filePath);
				Console.ReadKey();
				return;
			}

			SourceFileIterator iterator = new SourceFileIterator(filePath);
			foreach (string word in iterator.EnumerateWords())
			{
				Console.WriteLine(word);
			}
			Console.ReadKey();
		}
	}
}
