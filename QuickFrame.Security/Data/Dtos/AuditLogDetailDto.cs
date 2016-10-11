using ExpressMapper;
using Newtonsoft.Json;
using QuickFrame.Data.Dtos;
using QuickFrame.Security.Data.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace QuickFrame.Security.Data.Dtos {

	///<summary>Represents one entry in the audit log table.</summary>
	public class AuditLogDetailDto : DataTransferObjectLong<AuditLog, AuditLogDetailDto> {

		///<summary>Gets or sets the unique identifier for the user requesting the changes.</summary>
		public string UserId { get; set; }

		///<summary>Gets or sets the date and time when the changes were requested.</summary>
		public DateTime EventDate { get; set; }

		///<summary>Gets or sets an integer value determining the type of change.  Generated from the EntityState enumeration.</summary>
		public string EventType { get; set; }

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

		public override void Register() {
			Mapper.Register<AuditLog, AuditLogDetailDto>()
				.Function(dest => dest.EventType, src => {
					return ((EntityState)src.EventType).ToString();
				})
				.Function(dest => dest.OriginalValue, src => {
					return src.OriginalValue?.Replace("\n", "<br />");
				})
				.Function(dest => dest.NewValue, src => {
					return src.NewValue?.Replace("\n", "<br />");
				});
		}
	}
}