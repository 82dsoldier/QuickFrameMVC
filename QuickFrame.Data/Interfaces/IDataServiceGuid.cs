using System;

namespace QuickFrame.Data.Interfaces {

	public interface IDataServiceGuid<TEntity>
	: IDataServiceCore<Guid, TEntity>
	where TEntity : IDataModelGuid {
	}
}