using QuickFrame.Data.Interfaces.Dtos;
using QuickFrame.Data.Interfaces.Models;
using System;

namespace QuickFrame.Data.Dtos {

	public class DataTransferObjectGuid<TSrc, TDest>
		: DataTransferObject<TSrc, TDest, Guid>, IDataTransferObjectGuid
		where TSrc : class, IDataModel<Guid> {
	}
}