using QuickFrame.Data.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace QuickFrame.Security.Data.Models {

	///<summary>Represents one entry in the audit log table.</summary>
	public class AuditLog : DataModelLong {

		///<summary>Gets or sets the unique identifier for the user requesting the changes.</summary>
		[Required]
		[StringLength(128)]
		public string UserId { get; set; }

		///<summary>Gets or sets the date and time when the changes were requested.</summary>
		[Required]
		public DateTime EventDate { get; set; }

		///<summary>Gets or sets an integer value determining the type of change.  Generated from the EntityState enumeration.</summary>
		[Required]
		public int EventType { get; set; }

		///<summary>Gets or sets the name of the table for which the changes are being made.</summary>
		[Required]
		public string TableName { get; set; }

		///<summary>Gets or sets a unique identifier for the record being changed.</summary>
		public string RecordId { get; set; }

		///<summary>Gets or sets the name of the column being changed.</summary>
		public string ColumnName { get; set; }

		///<summary>Gets or sets the original value (if any) of the column being changed.  Non-string values are serialized as json objects before saving.</summary>
		public string OriginalValue { get; set; }

		///<summary>Gets or sets the new value (if any) to be used in the column.  Non-string values are serialized as json objects before saving.</summary>
		public string NewValue { get; set; }
	}
}