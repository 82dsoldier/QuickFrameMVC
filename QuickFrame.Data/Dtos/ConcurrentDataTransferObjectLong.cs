using QuickFrame.Data.Interfaces;

namespace QuickFrame.Data {

	public class ConcurrentDataTransferObjectLong<TSrc, TDest>
		: DataTransferObjectLong<TSrc, TDest>,
		IConcurrentDataTransferObjectLong<TSrc, TDest>
		where TSrc : IConcurrentDataModelLong {
		public byte[] RowVersion { get; set; }
	}
}