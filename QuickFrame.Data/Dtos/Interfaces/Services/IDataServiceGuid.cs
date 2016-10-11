using QuickFrame.Data.Interfaces.Models;
using System;

namespace QuickFrame.Data.Interfaces.Services {

	public interface IDataServiceGuid<TEntity>
		: IDataService<TEntity, Guid>
		where TEntity : class, IDataModelGuid {
	}
}