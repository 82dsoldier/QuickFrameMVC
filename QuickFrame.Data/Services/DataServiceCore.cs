using QuickFrame.Data.Interfaces.Dtos;
using QuickFrame.Data.Interfaces.Services;
using System.Data.Entity;

namespace QuickFrame.Data.Services {

	public abstract class DataServiceCore<TContext, TEntity, TIdType>
		: DataServiceBase<TContext, TEntity>, IDataServiceCore<TEntity, TIdType>
		where TContext : DbContext
		where TEntity : class {

		public DataServiceCore(TContext dbContext)
			: base(dbContext) {
		}

		public abstract void Delete(TIdType id);

		public abstract TEntity Get(TIdType id);

		public abstract TResult Get<TResult>(TIdType id) where TResult : IDataTransferObjectCore;
	}
}