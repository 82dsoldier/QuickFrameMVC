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
		: DataTransferObjectCore<int, TSrc, TDest>,
		IDataTransferObjectInt<TSrc, TDest>
		where TSrc : IDataModelInt {
	}

	public class DataTransferObjectLong<TSrc, TDest>
	: DataTransferObjectCore<long, TSrc, TDest>,
		IDataTransferObjectLong<TSrc, TDest>
	where TSrc : IDataModelLong {
	}

	public class DataTransferObjectGuid<TSrc, TDest>
	: DataTransferObjectCore<Guid, TSrc, TDest>,
		IDataTransferObjectGuid<TSrc, TDest>
	where TSrc : IDataModelGuid {
	}

	public class DataTransferObject<TSrc, TDest>
	: DataTransferObjectInt<TSrc, TDest>
	where TSrc : IDataModelInt {
	}

	public class ConcurrentDataTransferObjectCore<TDataType, TSrc, TDest>
	: DataTransferObjectCore<TDataType, TSrc, TDest>,
	IConcurrentDataTransferObjectCore<TDataType, TSrc, TDest>
	where TSrc : IConcurrentDataModelCore<TDataType> {
		public byte[] RowVersion { get; set; }
	}

	public class ConcurrentDataTransferObjectInt<TSrc, TDest>
		: DataTransferObjectInt<TSrc, TDest>,
		IConcurrentDataTransferObjectInt<TSrc, TDest>
		where TSrc : IConcurrentDataModelInt {
		public byte[] RowVersion { get; set; }
	}
	public class ConcurrentDataTransferObjectLong<TSrc, TDest>
		: DataTransferObjectLong<TSrc, TDest>,
		IConcurrentDataTransferObjectLong<TSrc, TDest>
		where TSrc : IConcurrentDataModelLong {
		public byte[] RowVersion { get; set; }
	}
	public class ConcurrentDataTransferObjectGuid<TSrc, TDest>
		: DataTransferObjectGuid<TSrc, TDest>,
		IConcurrentDataTransferObjectGuid<TSrc, TDest>
		where TSrc : IConcurrentDataModelGuid {
		public byte[] RowVersion { get; set; }
	}
	public class ConcurrentDataTransferObject<TSrc, TDest>
		: DataTransferObjectInt<TSrc, TDest>,
		IConcurrentDataTransferObjectInt<TSrc, TDest>
		where TSrc : IConcurrentDataModelInt {
		public byte[] RowVersion { get; set; }
	}
}