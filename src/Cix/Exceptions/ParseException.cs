﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.Exceptions
{
	[Serializable]
	public sealed class ParseException : Exception
	{
		private string sourceFile;
		private int lineNumber;
		private int charNumber;

		public string ErrorLocation
		{
			get
			{
				return $"At line {lineNumber} position {charNumber}";
			}
		}

		public ParseException(string sourceFile, int lineNumber, int charNumber) : base()
		{
			this.sourceFile = sourceFile;
			this.lineNumber = lineNumber;
			this.charNumber = charNumber;
		}

		public ParseException(string message, string sourceFile, int lineNumber, int charNumber) : base(message)
		{
			this.sourceFile = sourceFile;
			this.lineNumber = lineNumber;
			this.charNumber = charNumber;
		}
		
		public ParseException(string message, Exception innerException, string sourceFile, 
			int lineNumber, int charNumber) : base(message, innerException)
		{
			this.sourceFile = sourceFile;
			this.lineNumber = lineNumber;
			this.charNumber = charNumber;
		}
	}
}