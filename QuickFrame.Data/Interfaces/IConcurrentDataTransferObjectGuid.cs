using System;

namespace QuickFrame.Data.Interfaces {

	public interface IConcurrentDataTransferObjectGuid<TSrc, TDest>
		: IConcurrentDataModel<Guid>
		where TSrc : IConcurrentDataModelGuid {
	}
}