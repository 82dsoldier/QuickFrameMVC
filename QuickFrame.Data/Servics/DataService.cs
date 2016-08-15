using ExpressMapper;
using QuickFrame.Data;
using QuickFrame.Data.Interfaces;
using QuickFrame.Di;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace QuickFrame.Data.Services {

	public abstract class DataService<TContext, TEntity>
	: DataServiceInt<TContext, TEntity>
	where TContext : DbContext
	where TEntity : class, IDataModelInt {
	}

	public abstract class DataService<TContext, TDataType, TEntity>
		: GenericDataService<TContext, TDataType, TEntity>,
		IDataService<TDataType, TEntity>
		where TContext : DbContext
		where TEntity : class, IDataModel<TDataType> {

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

		protected override IEnumerable<TEntity> GetListBase(int start = 0, int count = 0, string columnName = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false) {
			using(var contextFactory = ComponentContainer.Component<TContext>()) {
				var query = includeDeleted ? contextFactory.Component.Set<TEntity>().Where(o => 1 == 1) : contextFactory.Component.Set<TEntity>().Where(o => o.IsDeleted == false);
				query = sortOrder == SortOrder.Ascending ? query.OrderBy(columnName) : query.OrderByDescending(columnName);
				if(start > 0)
					query = query.Skip(start);
				if(count > 0)
					query = query.Take(count);
				return query.ToList();
			}
		}

		protected override IEnumerable<TResult> GetListBase<TResult>(int start = 0, int count = 0, string columnName = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false) {
			using(var contextFactory = ComponentContainer.Component<TContext>()) {
				var query = includeDeleted ? contextFactory.Component.Set<TEntity>().Where(o => 1 == 1) : contextFactory.Component.Set<TEntity>().Where(o => o.IsDeleted == false);
				query = sortOrder == SortOrder.Ascending ? query.OrderBy(columnName) : query.OrderByDescending(columnName);
				if(start > 0)
					query = query.Skip(start);
				if(count > 0)
					query = query.Take(count);
				foreach(var obj in query)
					yield return Mapper.Map<TEntity, TResult>(obj);
			}
		}

		protected override long GetCountBase(bool includeDeleted = false) {
			using(var contextFactory = ComponentContainer.Component<TContext>()) {
				return includeDeleted ? contextFactory.Component.Set<TEntity>().Count() : contextFactory.Component.Set<TEntity>().Count(obj => obj.IsDeleted == false);
			}
		}
	}
}