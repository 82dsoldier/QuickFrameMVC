using System;

namespace QuickFrame.Data.Interfaces {

	public interface IDataTransferObjectCore<TDataType, TSrc, TDest>
		: IDataModelCore<TDataType>
		where TSrc : IDataModelCore<TDataType> {

		void Register();
	}

	public interface IDataTransferObjectInt<TSrc, TDest>
		: IDataTransferObjectCore<int, TSrc, TDest>
		where TSrc : IDataModelInt {
	}

	public interface IDataTransferObjectLong<TSrc, TDest>
	: IDataTransferObjectCore<long, TSrc, TDest>
	where TSrc : IDataModelLong {
	}

	public interface IDataTransferObjectGuid<TSrc, TDest>
	: IDataTransferObjectCore<Guid, TSrc, TDest>
	where TSrc : IDataModelGuid {
	}

	public interface IDataTransferObject<TSrc, TDest>
	: IDataTransferObjectCore<int, TSrc, TDest>
	where TSrc : IDataModelInt {
	}
	public interface IConcurrentDataTransferObjectCore<TDataType, TSrc, TDest>
		: IConcurrentDataModelCore<TDataType>
		where TSrc : IConcurrentDataModelCore<TDataType> {

	}
	public interface IConcurrentDataTransferObjectInt<TSrc, TDest>
		: IConcurrentDataModelCore<int>
		where TSrc : IConcurrentDataModelInt {

	}
	public interface IConcurrentDataTransferObjectLong<TSrc, TDest>
		: IConcurrentDataModelCore<long>
		where TSrc : IConcurrentDataModelLong {

	}
	public interface IConcurrentDataTransferObjectGuid<TSrc, TDest>
		: IConcurrentDataModelCore<Guid>
		where TSrc : IConcurrentDataModelGuid {

	}
	public interface IConcurrentDataTransferObject<TSrc, TDest>
		: IConcurrentDataTransferObjectInt<TSrc, TDest>
		where TSrc : IConcurrentDataModelInt {

	}
}