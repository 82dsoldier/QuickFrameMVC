using QuickFrame.Data.Interfaces.Services;
#if NETSTANDARD1_6
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
#endif

namespace QuickFrame.Data.Services {

	/// <summary>
	/// A base class for data services based on a TEntity with an int for an id.
	/// </summary>
	/// <typeparam name="TContext">The ntity framework context used to access the database</typeparam>
	/// <typeparam name="TEntity">The type of entity being used by this service.</typeparam>
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