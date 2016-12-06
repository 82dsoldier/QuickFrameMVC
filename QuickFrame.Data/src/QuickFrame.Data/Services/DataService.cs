using QuickFrame.Data.Interfaces.Models;
using QuickFrame.Data.Interfaces.Services;
#if NETSTANDARD1_6
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
#endif

namespace QuickFrame.Data.Services {

	public abstract class DataService<TContext, TEntity, TIdType>
		: DataServiceCore<TContext, TEntity, TIdType>,
		IDataService<TEntity, TIdType>
		where TContext : DbContext
		where TEntity : class, IDataModel<TIdType> {

		public DataService(TContext dbContext) : base(dbContext) {
		}
	}

	public class DataService<TContext, TEntity>
		: DataServiceInt<TContext, TEntity>
		where TContext : DbContext
		where TEntity : class, IDataModelInt {

		public DataService(TContext dbContext) : base(dbContext) {
		}
	}
}