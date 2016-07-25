using ExpressMapper;
using QuickFrame.Data.Interfaces;
using QuickFrame.Di;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Data.Services {

	public abstract class ReadOnlyDataService<TContext, TDataType, TEntity>
		: IReadOnlyDataService<TDataType, TEntity>
		where TContext : DbContext
		where TEntity : class {

		public abstract TEntity Get(TDataType id);

		public virtual Task<TEntity> GetAsync(TDataType id) => Task.Run(() => Get(id));

		public abstract TResult Get<TResult>(TDataType id);

		public virtual Task<TResult> GetAsync<TResult>(TDataType id) => Task.Run(() => Get<TResult>(id));

		public virtual long GetCount(bool includeDeleted = false) => GetCountBase(includeDeleted);

		public virtual Task<long> GetCountAsync(bool includeDeleted = false) => Task.Run(() => GetCount(includeDeleted));

		public virtual IEnumerable<TEntity> GetList(int start = 0, int count = 0, string columnName = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false) {
			foreach(var obj in GetListBase(start, count, columnName, sortOrder, includeDeleted))
				yield return obj;
		}

		public virtual Task<IEnumerable<TEntity>> GetListAsync(int start = 0, int count = 0, string columnName = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false)
			=> Task.Run(() => GetList(start, count, columnName, sortOrder));

		public virtual IEnumerable<TResult> GetList<TResult>(int start = 0, int count = 0, string columnName = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false)
			=> GetListBase<TResult>(start, count, columnName, sortOrder, includeDeleted);
		//	{
		//	using(var contextFactory = ComponentContainer.Component<TContext>()) {
		//		var query = sortOrder == SortOrder.Ascending ? contextFactory.Component.Set<TEntity>().OrderBy(columnName) : contextFactory.Component.Set<TEntity>().OrderByDescending(columnName);
		//		if(start > 0)
		//			query = query.Skip(start);
		//		if(count > 0)
		//			query = query.Take(count);
		//		foreach(var obj in query)
		//			yield return Mapper.Map<TEntity, TResult>(obj);
		//	}
		//}

		public virtual Task<IEnumerable<TResult>> GetListAsync<TResult>(int start = 0, int count = 0, string columnName = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false)
			=> Task.Run(() => GetList<TResult>(start, count, columnName, sortOrder));

		protected virtual IEnumerable<TEntity> GetListBase(int start = 0, int count = 0, string columnName = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false) {
			using(var contextFactory = ComponentContainer.Component<TContext>()) {
				var query = sortOrder == SortOrder.Ascending ? contextFactory.Component.Set<TEntity>().OrderBy(columnName) : contextFactory.Component.Set<TEntity>().OrderByDescending(columnName);
				if(start > 0)
					query = query.Skip(start);
				if(count > 0)
					query = query.Take(count);
				return query.ToList();
			}
		}

		protected virtual IEnumerable<TResult> GetListBase<TResult>(int start = 0, int count = 0, string columnName = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false) {
			using(var contextFactory = ComponentContainer.Component<TContext>()) {
				var query = sortOrder == SortOrder.Ascending ? contextFactory.Component.Set<TEntity>().OrderBy(columnName) : contextFactory.Component.Set<TEntity>().OrderByDescending(columnName);
				if(start > 0)
					query = query.Skip(start);
				if(count > 0)
					query = query.Take(count);
				foreach(var obj in query)
					yield return Mapper.Map<TEntity, TResult>(obj);
			}
		}
		protected virtual long GetCountBase(bool includeDeleted = false) {
			using(var contextFactory = ComponentContainer.Component<TContext>()) {
				return contextFactory.Component.Set<TEntity>().Count();
			}
		}
	}
}