using ExpressMapper;
using QuickFrame.Data.Interfaces;
using QuickFrame.Data.Models;

namespace QuickFrame.Data {

	public class DataTransferObject<TSrc, TDest>
	 : DataTransferObjectInt<TSrc, TDest>
	 where TSrc : IDataModelInt {
	}

	public class DataTransferObject<TDataType, TSrc, TDest>
	: DataModel<TDataType>,
	IDataTransferObject<TDataType, TSrc, TDest>
	where TSrc : IDataModel<TDataType> {

		public virtual void Register() {
			Mapper.Register<TSrc, TDest>();
			Mapper.Register<TDest, TSrc>();
		}
	}
}