namespace QuickFrame.Data.Interfaces {

	public interface IConcurrentDataTransferObject<TSrc, TDest>
		: IConcurrentDataTransferObjectInt<TSrc, TDest>
		where TSrc : IConcurrentDataModelInt {
	}

	public interface IConcurrentDataTransferObject<TDataType, TSrc, TDest>
	: IConcurrentDataModel<TDataType>
	where TSrc : IConcurrentDataModel<TDataType> {
	}
}