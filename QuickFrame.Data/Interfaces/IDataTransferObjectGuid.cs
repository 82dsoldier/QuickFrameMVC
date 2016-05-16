using System;

namespace QuickFrame.Data.Interfaces
{

	public interface IDataTransferObjectGuid<TSrc, TDest>
	: IDataTransferObjectCore<Guid, TSrc, TDest>
	where TSrc : IDataModelGuid {
	}

}