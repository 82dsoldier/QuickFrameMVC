namespace QuickFrame.Data.Interfaces {

	public interface IDataTransferObjectLong<TSrc, TDest>
	: IDataTransferObject<long, TSrc, TDest>
	where TSrc : IDataModelLong {
	}
}