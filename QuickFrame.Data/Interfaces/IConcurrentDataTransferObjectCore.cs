using System;

namespace QuickFrame.Data.Interfaces
{

	public interface IConcurrentDataTransferObjectCore<TDataType, TSrc, TDest>
		: IConcurrentDataModelCore<TDataType>
		where TSrc : IConcurrentDataModelCore<TDataType> {

	}

}