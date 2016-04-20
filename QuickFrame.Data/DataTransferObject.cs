using ExpressMapper;
using QuickFrame.Data.Interfaces;
using System;

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

	public class DataTransferObjectInt<TSrc, TDest>
		: DataTransferObjectCore<int, TSrc, TDest>
		where TSrc : IDataModelInt {
	}

	public class DataTransferObjectLong<TSrc, TDest>
	: DataTransferObjectCore<long, TSrc, TDest>
	where TSrc : IDataModelLong {
	}

	public class DataTransferObjectGuid<TSrc, TDest>
	: DataTransferObjectCore<Guid, TSrc, TDest>
	where TSrc : IDataModelGuid {
	}

	public class DataTransferObject<TSrc, TDest>
	: DataTransferObjectInt<TSrc, TDest>
	where TSrc : IDataModelInt {
	}
}