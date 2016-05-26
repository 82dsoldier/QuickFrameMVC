using QuickFrame.Data.Interfaces;

namespace QuickFrame.Data {

	public class ConcurrentDataTransferObject<TSrc, TDest>
		: DataTransferObjectInt<TSrc, TDest>,
		IConcurrentDataTransferObjectInt<TSrc, TDest>
		where TSrc : IConcurrentDataModelInt {
		public byte[] RowVersion { get; set; }
	}
}