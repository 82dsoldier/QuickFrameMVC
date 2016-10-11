using QuickFrame.Data.Interfaces.Dtos;
using QuickFrame.Data.Interfaces.Models;

namespace QuickFrame.Data.Dtos {

	public class DataTransferObjectInt<TSrc, TDest>
		: DataTransferObject<TSrc, TDest, int>,
		IDataTransferObjectInt
		where TSrc : class, IDataModel<int> {
	}
}