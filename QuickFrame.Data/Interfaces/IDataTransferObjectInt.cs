using System;

namespace QuickFrame.Data.Interfaces
{

	public interface IDataTransferObjectInt<TSrc, TDest>
		: IDataTransferObjectCore<int, TSrc, TDest>
		where TSrc : IDataModelInt {
	}

}