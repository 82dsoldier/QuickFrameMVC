using System;

namespace QuickFrame.Data.Interfaces
{

	public interface IConcurrentDataTransferObjectGuid<TSrc, TDest>
		: IConcurrentDataModelCore<Guid>
		where TSrc : IConcurrentDataModelGuid {

	}

}