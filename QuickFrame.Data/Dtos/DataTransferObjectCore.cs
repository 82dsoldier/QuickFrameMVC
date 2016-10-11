using ExpressMapper;
using QuickFrame.Data.Interfaces.Dtos;
using QuickFrame.Data.Interfaces.Models;

namespace QuickFrame.Data.Dtos {

	public class DataTransferObjectCore<TSrc, TDest>
		: IDataTransferObjectCore
		where TSrc : IDataModelCore {

		public virtual void Register() {
			Mapper.Register<TSrc, TDest>();
			Mapper.Register<TDest, TSrc>();
		}
	}
}