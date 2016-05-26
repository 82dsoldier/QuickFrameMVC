using QuickFrame.Data.Interfaces;

namespace QuickFrame.Data {

	public class DataTransferObjectLong<TSrc, TDest>
	: DataTransferObjectCore<long, TSrc, TDest>,
		IDataTransferObjectLong<TSrc, TDest>
	where TSrc : IDataModelLong {
	}
}