using System;

namespace QuickFrame.Data.Interfaces {

	public interface IDataTransferObjectGuid<TSrc, TDest>
	: IDataTransferObject<Guid, TSrc, TDest>
	where TSrc : IDataModelGuid {
	}
}