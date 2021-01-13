using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cix.Errors;
using Cix.Text;
using Microsoft.SqlServer.Server;
using Exception = System.Exception;

namespace Cix
{
	internal sealed class IO
	{
		private readonly IErrorListProvider errorList;

		public IO(IErrorListProvider errorList) => this.errorList = errorList;

		public string LoadInputFile(string filePath)
		{
			if (string.IsNullOrEmpty(filePath))
			{
				errorList.AddError(ErrorSource.IO, 1, "Null or empty file path.", null);
				return null;
			}
			else if (!File.Exists(filePath))
			{
				errorList.AddError(ErrorSource.IO, 2,
					$"File {Path.GetFileName(filePath)} is not a valid path or the file does not exist.", null);
				return null;
			}

			try
			{
				string fileText;
				using (TextReader file = new StreamReader(File.OpenRead(filePath)))
				{
					fileText = file.ReadToEnd();
					if (string.IsNullOrEmpty(fileText))
					{
						errorList.AddError(ErrorSource.IO, 4, $"File {Path.GetFileName(filePath)} is blank or empty.",
							null);
						return null;
					}
				}
				return fileText;
			}
			catch (Exception e)
			{
				errorList.AddError(ErrorSource.IO, 5,
					$"An exception occurred while trying to read the file {Path.GetFileName(filePath)}. Message from exception: \"{e.Message}\"",
					null);
				return null;
			}
		}

		public IList<Line> SplitFileByLine(string filePath, string file)
		{
			string[] splitLines = file.Split(new[] {"\r\n"}, StringSplitOptions.None);
			return splitLines.Select((text, lineNumber) => new Line(filePath, lineNumber, text)).ToList();
		}
	}
}
