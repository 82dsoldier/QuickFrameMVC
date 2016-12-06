using QuickFrame.Data.Interfaces.Dtos;
using QuickFrame.Data.Interfaces.Models;

namespace QuickFrame.Data.Dtos {

	public class NamedDataTransferObject<TSrc, TDest>
		: NamedDataTransferObjectInt<TSrc, TDest>,
		INamedDataTransferObject
		where TSrc : class, INamedDataModel {
	}
}