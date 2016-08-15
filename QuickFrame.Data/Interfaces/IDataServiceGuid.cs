using System;

namespace QuickFrame.Data.Interfaces {

	public interface IDataServiceGuid<TEntity>
	: IDataService<Guid, TEntity>
	where TEntity : IDataModelGuid {
	}
}