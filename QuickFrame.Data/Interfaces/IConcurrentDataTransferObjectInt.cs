namespace QuickFrame.Data.Interfaces {

	public interface IConcurrentDataTransferObjectInt<TSrc, TDest>
		: IConcurrentDataTransferObjectCore<int, TSrc, TDest>
		where TSrc : IConcurrentDataModelInt {
	}
}