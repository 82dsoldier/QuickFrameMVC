using QuickFrame.Data.Interfaces;
using System.Data.Entity;

namespace QuickFrame.Data.Services {

	public class DataService<TContext, TEntity>
	: DataServiceInt<TContext, TEntity>
	where TContext : DbContext
	where TEntity : class, IDataModelInt {
	}
}