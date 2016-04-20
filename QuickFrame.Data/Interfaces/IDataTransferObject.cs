using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Data.Interfaces
{
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
}
