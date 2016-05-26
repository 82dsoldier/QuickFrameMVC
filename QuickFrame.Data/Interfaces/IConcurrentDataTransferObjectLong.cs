namespace QuickFrame.Data.Interfaces {

	public interface IConcurrentDataTransferObjectLong<TSrc, TDest>
		: IConcurrentDataModelCore<long>
		where TSrc : IConcurrentDataModelLong {
	}
}