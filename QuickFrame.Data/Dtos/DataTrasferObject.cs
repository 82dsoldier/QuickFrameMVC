using QuickFrame.Data.Interfaces;

namespace QuickFrame.Data {

	public class DataTransferObject<TSrc, TDest>
	 : DataTransferObjectInt<TSrc, TDest>
	 where TSrc : IDataModelInt {
	}
}