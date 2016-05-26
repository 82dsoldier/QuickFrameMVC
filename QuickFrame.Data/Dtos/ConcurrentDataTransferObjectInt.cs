using QuickFrame.Data.Interfaces;

namespace QuickFrame.Data {

	public class ConcurrentDataTransferObjectInt<TSrc, TDest>
		 : DataTransferObjectInt<TSrc, TDest>,
		 IConcurrentDataTransferObjectInt<TSrc, TDest>
		 where TSrc : IConcurrentDataModelInt {
		public byte[] RowVersion { get; set; }
	}
}