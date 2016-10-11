using System;
using System.Data.Entity;

namespace QuickFrame.Security.Data.Dtos {

	public class DataChangedEventArgsDto {
		public string Title { get; set; }
		public string UserId { get; set; }
		public DateTime EventDate { get; set; }
		public EntityState EventType { get; set; }
		public string TableName { get; set; }
		public string RecordId { get; set; }
		public string ColumnName { get; set; }
		public string OrginalValue { get; set; }
		public string NewValue { get; set; }
	}
}