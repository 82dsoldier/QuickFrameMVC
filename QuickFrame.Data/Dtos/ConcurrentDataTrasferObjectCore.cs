using QuickFrame.Data.Interfaces;

namespace QuickFrame.Data {

	public class ConcurrentDataTransferObjectCore<TDataType, TSrc, TDest>
	: DataTransferObjectCore<TDataType, TSrc, TDest>,
	IConcurrentDataTransferObject<TDataType, TSrc, TDest>
	where TSrc : IConcurrentDataModel<TDataType> {
		public byte[] RowVersion { get; set; }
	}
}