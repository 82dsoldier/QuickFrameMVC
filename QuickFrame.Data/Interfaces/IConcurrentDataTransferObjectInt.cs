using System;

namespace QuickFrame.Data.Interfaces
{

	public interface IConcurrentDataTransferObjectInt<TSrc, TDest>
		: IConcurrentDataModelCore<int>
		where TSrc : IConcurrentDataModelInt {

	}

}