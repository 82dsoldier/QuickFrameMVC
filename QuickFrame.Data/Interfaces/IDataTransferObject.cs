using System;

namespace QuickFrame.Data.Interfaces {

	public interface IDataTransferObject<TSrc, TDest>
	: IDataTransferObjectCore<int, TSrc, TDest>
	where TSrc : IDataModelInt {
	}
}