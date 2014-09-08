using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix
{
	public sealed class ParseException : Exception
	{
		private string sourceFile;
		private int position;

		public string ErrorLocation
		{
			get
			{
				string[] lines = this.sourceFile.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
				int lineNumber = 0;
				int position = this.position;
				
				for (int i = 0; i < lines.Length; i++)
				{
					if (position > lines[i].Length)
					{
						position -= lines[i].Length;
						lineNumber++;
					}
					else
					{
						break;
					}
				}

				return string.Format("At line {0} position {1}\r\n{2}", lineNumber, position, lines[lineNumber]);
			}
		}

		public ParseException(string sourceFile, int position) : base()
		{
			this.sourceFile = sourceFile;
			this.position = position;
		}

		public ParseException(string message, string sourceFile, int position) : base(message)
		{
			this.sourceFile = sourceFile;
			this.position = position;
		}
		
		public ParseException(string message, Exception innerException, string sourceFile, int position) : base(message, innerException)
		{
			this.sourceFile = sourceFile;
			this.position = position;
		}
	}
}
