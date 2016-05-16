using System;

namespace QuickFrame.Data.Interfaces
{

	public interface IDataTransferObjectLong<TSrc, TDest>
	: IDataTransferObjectCore<long, TSrc, TDest>
	where TSrc : IDataModelLong {
	}

}