namespace QuickFrame.Data.Interfaces {

	public interface IDataTransferObject<TSrc, TDest>
	: IDataTransferObject<int, TSrc, TDest>
	where TSrc : IDataModelInt {
	}

	public interface IDataTransferObject<TDataType, TSrc, TDest>
	: IGenericDataTransferObject<TSrc, TDest>, IDataModel<TDataType>
	where TSrc : IDataModel<TDataType> {
	}
}