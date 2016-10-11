using QuickFrame.Data.Interfaces.Services;
using System.Data.Entity;

namespace QuickFrame.Data.Services {

	public abstract class DataServiceCoreInt<TContext, TEntity>
		: DataServiceCore<TContext, TEntity, int>,
		IDataServiceCoreInt<TEntity>
		where TContext : DbContext
		where TEntity : class {

		public DataServiceCoreInt(TContext dbContext)
			: base(dbContext) {
		}
	}
}