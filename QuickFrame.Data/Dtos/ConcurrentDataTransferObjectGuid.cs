using QuickFrame.Data.Interfaces;

namespace QuickFrame.Data {

	public class ConcurrentDataTransferObjectGuid<TSrc, TDest>
		: DataTransferObjectGuid<TSrc, TDest>,
		IConcurrentDataTransferObjectGuid<TSrc, TDest>
		where TSrc : IConcurrentDataModelGuid {
		public byte[] RowVersion { get; set; }
	}
}