using QuickFrame.Data.Interfaces;
using System;

namespace QuickFrame.Data {

	public class DataTransferObjectGuid<TSrc, TDest>
	: DataTransferObjectCore<Guid, TSrc, TDest>,
		IDataTransferObjectGuid<TSrc, TDest>
	where TSrc : IDataModelGuid {
	}
}