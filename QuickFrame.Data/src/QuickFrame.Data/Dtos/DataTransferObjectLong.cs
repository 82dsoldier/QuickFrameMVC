using QuickFrame.Data.Interfaces.Dtos;
using QuickFrame.Data.Interfaces.Models;

namespace QuickFrame.Data.Dtos {

	public class DataTransferObjectLong<TSrc, TDest>
		: DataTransferObject<TSrc, TDest, long>,
		IDataTransferObjectLong
		where TSrc : class, IDataModelLong {
	}
}