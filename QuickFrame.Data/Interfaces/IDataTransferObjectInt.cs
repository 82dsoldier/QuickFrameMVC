namespace QuickFrame.Data.Interfaces {

	public interface IDataTransferObjectInt<TSrc, TDest>
		: IDataTransferObject<int, TSrc, TDest>
		where TSrc : IDataModelInt {
	}
}