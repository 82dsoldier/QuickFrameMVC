using System;
#if NETSTANDARD1_6
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
#endif

namespace QuickFrame.Security.Data.Events {

	public class DataChangedEventArgs : EventArgs {
		public int Id { get; set; }
		public string UserId { get; set; }
		public DateTime EventDate { get; set; }
		public EntityState EventType { get; set; }
		public string TableName { get; set; }
		public string RecordId { get; set; }
		public string ColumnName { get; set; }
		public string OriginalValue { get; set; }
		public string NewValue { get; set; }
	}
}