using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickFrame.Data.Models {

	/// <summary>
	/// An entry in the AuditLog table of the database.  Used to track which user is making which changes to the database.
	/// </summary>
	public class AuditLog {

		/// <summary>
		/// Gets or sets the unique identifier for this record
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		[Required]
		public long Id { get; set; }

		/// <summary>
		/// Gets or sets the user id for the user requesting the change.
		/// </summary>
		/// <value>
		/// The user identifier.
		/// </value>
		[Required]
		[StringLength(32)]
		public string UserId { get; set; }

		/// <summary>
		/// Gets or sets the event date.
		/// </summary>
		/// <value>
		/// The event date.
		/// </value>
		[Required]
		public DateTime EventDate { get; set; }

		/// <summary>
		/// Gets or sets the type of the event from the EntityState enumeration.
		/// </summary>
		/// <value>
		/// The type of the event.
		/// </value>
		[Required]
		public int EventType { get; set; }

		/// <summary>
		/// Gets or sets the name of the table in which the change is being requested.
		/// </summary>
		/// <value>
		/// The name of the table.
		/// </value>
		[Required]
		public string TableName { get; set; }

		/// <summary>
		/// Gets or sets the unique identifier for the record being changed.
		/// </summary>
		/// <value>
		/// The record identifier.
		/// </value>
		/// <remarks>If this is an Add event, this field will be null.  If the Id is a number, it will be converted to string to save into this field.</remarks>
		public string RecordId { get; set; }

		/// <summary>
		/// Gets or sets the name of the column being changed.
		/// </summary>
		/// <value>
		/// The name of the column.
		/// </value>
		/// <remarks>If the event is an Add or Delete, the entire record is marked as changed and this field will be null.</remarks>
		public string ColumnName { get; set; }

		/// <summary>
		/// Gets or sets the original value of the entry being changed.
		/// </summary>
		/// <value>
		/// The original value.
		/// </value>
		/// <remarks>If this is an add event, this field will be null.  If it is a column being changed than it will contain the value of that column.
		/// If an entire entry is being changed, all of the values in the entry will be converted to a JSON string and stored.</remarks>
		public string OriginalValue { get; set; }

		/// <summary>
		/// Gets or sets the new value.
		/// </summary>
		/// <value>
		/// The new value.
		/// </value>
		/// <remarks>If a column being changed than this field will contain the value of that column.
		/// If an entire entry is being changed, all of the values in the entry will be converted to a JSON string and stored.</remarks>
		public string NewValue { get; set; }
	}
}