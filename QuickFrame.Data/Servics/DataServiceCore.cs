using ExpressMapper;
using QuickFrame.Data.Interfaces;
using QuickFrame.Di;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Data.Services {

	public abstract class DataServiceCore<TContext, TDataType, TEntity>
		: GenericDataService<TContext, TDataType, TEntity>,
		IDataServiceCore<TDataType, TEntity>
		where TContext : DbContext
		where TEntity : class, IDataModelCore<TDataType> {

		public override long GetCount() => GetCount(false);
		public override Task<long> GetCountAsync() {
			throw new NotImplementedException();
		}
		public virtual long GetCount(bool includeDeleted = false) {
			using(var contextFactory = ComponentContainer.Component<TContext>()) {
				return includeDeleted ? contextFactory.Component.Set<TEntity>().Count() : contextFactory.Component.Set<TEntity>().Where(t => t.IsDeleted == false).Count();
			}
		}

		public virtual Task<long> GetCountAsync(bool includeDeleted = false) => Task.Run(() => GetCount(includeDeleted));

		public override void Save(TEntity model) {
			using(var contextFactory = ComponentContainer.Component<TContext>()) {
				var dbSet = contextFactory.Component.Set<TEntity>();
				if(model.Id.Equals(default(TDataType))) {
					Create(model);
					return;
				}
				dbSet.Attach(model);
				contextFactory.Component.Entry(model).State = EntityState.Modified;
				contextFactory.Component.SaveChanges();
			}
		}
	}
}