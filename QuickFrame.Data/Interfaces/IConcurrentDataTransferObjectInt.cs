namespace QuickFrame.Data.Interfaces {

	public interface IConcurrentDataTransferObjectInt<TSrc, TDest>
		: IConcurrentDataTransferObject<int, TSrc, TDest>
		where TSrc : IConcurrentDataModelInt {
	}
}