using QuickFrame.Data.Interfaces.Models;

namespace QuickFrame.Data.Interfaces.Services {

	public interface IDataService<TEntity, TIdType>
		: IDataServiceCore<TEntity, TIdType>
		where TEntity : class, IDataModel<TIdType> {
	}

	public interface IDataService<TEntity>
		: IDataServiceInt<TEntity>
		where TEntity : class, IDataModelInt {
	}
}