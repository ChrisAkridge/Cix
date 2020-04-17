using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Cix.Errors
{
	public sealed class ErrorsEncounteredException : Exception
	{
		private readonly List<Error> errors;

		public IReadOnlyList<Error> Errors => errors.AsReadOnly();

		private ErrorsEncounteredException() { }

		public ErrorsEncounteredException(IEnumerable<Error> errors) : base("The compilation process encountered one or more errors.")
			=> this.errors = errors.ToList();

		public ErrorsEncounteredException(IEnumerable<Error> errors, string message) : base(message)
			=> this.errors = errors.ToList();

		public ErrorsEncounteredException(IEnumerable<Error> errors, string message,
			Exception innerException) : base(message, innerException)
			=> this.errors = errors.ToList();
	}
}
