namespace QuickFrame.Data.Interfaces {

	public interface IConcurrentDataTransferObjectLong<TSrc, TDest>
		: IConcurrentDataModel<long>
		where TSrc : IConcurrentDataModelLong {
	}
}