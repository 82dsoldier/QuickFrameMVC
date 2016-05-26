using QuickFrame.Data.Interfaces;

namespace QuickFrame.Data {

	public class DataTransferObjectInt<TSrc, TDest>
		: DataTransferObjectCore<int, TSrc, TDest>,
		IDataTransferObjectInt<TSrc, TDest>
		where TSrc : IDataModelInt {
	}
}