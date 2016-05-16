using System;

namespace QuickFrame.Data.Interfaces
{

	public interface IDataTransferObjectCore<TDataType, TSrc, TDest>
		: IGenericDataTransferObject<TSrc, TDest>, IDataModelCore<TDataType>
		where TSrc : IDataModelCore<TDataType> {
	}

}