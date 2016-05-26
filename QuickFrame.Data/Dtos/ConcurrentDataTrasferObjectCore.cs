using QuickFrame.Data.Interfaces;

namespace QuickFrame.Data {

	public class ConcurrentDataTransferObjectCore<TDataType, TSrc, TDest>
	: DataTransferObjectCore<TDataType, TSrc, TDest>,
	IConcurrentDataTransferObjectCore<TDataType, TSrc, TDest>
	where TSrc : IConcurrentDataModelCore<TDataType> {
		public byte[] RowVersion { get; set; }
	}
}