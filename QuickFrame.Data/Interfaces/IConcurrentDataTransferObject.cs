namespace QuickFrame.Data.Interfaces {

	public interface IConcurrentDataTransferObject<TSrc, TDest>
		: IConcurrentDataTransferObjectInt<TSrc, TDest>
		where TSrc : IConcurrentDataModelInt {
	}
}