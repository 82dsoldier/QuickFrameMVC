using QuickFrame.Data.Interfaces.Models;

namespace QuickFrame.Data.Interfaces.Services {

	public interface IDataServiceInt<TEntity>
		: IDataService<TEntity, int>
		where TEntity : class, IDataModelInt {
	}
}