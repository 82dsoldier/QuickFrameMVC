using QuickFrame.Data.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace QuickFrame.Data.Models {

	public class ConcurrentDataModel : ConcurrentDataModelInt, IConcurrentDataModel {
	}

	public class ConcurrentDataModel<TDataType> : DataModel<TDataType>, IConcurrentDataModel<TDataType> {

		[Timestamp]
		public byte[] RowVersion { get; set; }
	}
}