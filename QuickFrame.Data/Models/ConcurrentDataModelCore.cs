using QuickFrame.Data.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace QuickFrame.Data.Models {

	public class ConcurrentDataModelCore<TDataType> : DataModelCore<TDataType>, IConcurrentDataModelCore<TDataType> {

		[Timestamp]
		public byte[] RowVersion { get; set; }
	}
}