using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Data.Exceptions
{
	/// <summary>
	/// An exception thrown when a record is requested that no longer exists in the database
	/// </summary>
	/// <seealso cref="System.Exception" />
	public class RecordDoesNotExistException : Exception {
		/// <summary>
		/// Initializes a new instance of the <see cref="RecordDoesNotExistException"/> class.
		/// </summary>
		public RecordDoesNotExistException() {
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="RecordDoesNotExistException"/> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public RecordDoesNotExistException(string message)
			: base(message) {
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="RecordDoesNotExistException"/> class.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
		public RecordDoesNotExistException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
