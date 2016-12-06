using QuickFrame.Data.Interfaces.Dtos;
using QuickFrame.Data.Interfaces.Services;
#if NETSTANDARD1_6
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
#endif

namespace QuickFrame.Data.Services {

	public abstract class DataServiceComposite<TContext, TEntity, TFirst, TSecond>
		: DataServiceBase<TContext, TEntity>,
		IDataServiceComposite<TEntity, TFirst, TSecond>
		where TContext : DbContext
		where TEntity : class {

		public DataServiceComposite(TContext dbContext)
			: base(dbContext) {
		}

		public abstract void Delete(TFirst firstId, TSecond secondId);

		public abstract TEntity Get(TFirst firstId, TSecond secondId);

		public abstract TResult Get<TResult>(TFirst firstId, TSecond secondId) where TResult : IDataTransferObjectCore;
	}

	public abstract class DataServiceComposite<TContext, TEntity, TFirst, TSecond, TThird>
		: DataServiceBase<TContext, TEntity>,
		IDataServiceComposite<TEntity, TFirst, TSecond, TThird>
		where TContext : DbContext
		where TEntity : class {

		public DataServiceComposite(TContext dbContext)
			: base(dbContext) {
		}

		public abstract void Delete(TFirst firstId, TSecond secondId, TThird thirdId);

		public abstract TEntity Get(TFirst firstId, TSecond secondId, TThird thirdId);

		public abstract TResult Get<TResult>(TFirst firstId, TSecond secondId, TThird thirdId) where TResult : IDataTransferObjectCore;
	}
}