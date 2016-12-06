using QuickFrame.Data.Interfaces.Dtos;
using QuickFrame.Data.Interfaces.Models;

namespace QuickFrame.Data.Dtos {

	public class NamedDataTransferObjectInt<TSrc, TDest>
		: DataTransferObjectInt<TSrc, TDest>,
		INamedDataTransferObjectInt
		where TSrc : class, INamedDataModelInt {
		public string Name { get; set; }
	}
}