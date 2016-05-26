using ExpressMapper;
using QuickFrame.Data.Interfaces;
using QuickFrame.Data.Models;

namespace QuickFrame.Data {

	public class DataTransferObjectCore<TDataType, TSrc, TDest>
		: DataModelCore<TDataType>,
		IDataTransferObjectCore<TDataType, TSrc, TDest>
		where TSrc : IDataModelCore<TDataType> {

		public virtual void Register() {
			Mapper.Register<TSrc, TDest>();
			Mapper.Register<TDest, TSrc>();
		}
	}
}