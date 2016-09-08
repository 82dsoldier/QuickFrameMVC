using ExpressMapper;
using QuickFrame.Data.Interfaces;
using QuickFrame.Di;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Data.Servics {

	[Obsolete("Use DataService<TContext, T1, T2, TEntity> instead")]
	public abstract class CompositeDataService<TContext, TPrimaryDataType, TSecondaryDataType, TEntity>
		: ICompositeDataService<TPrimaryDataType, TSecondaryDataType, TEntity>
		where TContext : DbContext
		where TEntity : class {

		public virtual void Create(TEntity model) {
			using(var contextFactory = ComponentContainer.Component<TContext>()) {
				contextFactory.Component.Set<TEntity>().Add(model);
				contextFactory.Component.SaveChanges();
			}
		}

		public virtual void Create<TModel>(TModel model) => Create(Mapper.Map<TModel, TEntity>(model));

		public virtual void CreateAsync(TEntity model) => Task.Run(() => Create(model));

		public virtual void CreateAsync<TModel>(TModel model) => Task.Run(() => Create(Mapper.Map<TModel, TEntity>(model)));

		public abstract bool Delete(TPrimaryDataType primaryId, TSecondaryDataType secondaryId);

		public virtual void DeleteAsync(TPrimaryDataType primaryId, TSecondaryDataType secondaryId) => Task.Run(() => Delete(primaryId, secondaryId));

		public abstract TEntity Get(TPrimaryDataType primaryId, TSecondaryDataType secondaryId);

		public virtual TResult Get<TResult>(TPrimaryDataType primaryId, TSecondaryDataType secondaryId) => Mapper.Map<TEntity, TResult>(Get(primaryId, secondaryId));

		public virtual Task<TEntity> GetAsync(TPrimaryDataType primaryId, TSecondaryDataType secondaryId) => Task.Run(() => Get(primaryId, secondaryId));

		public virtual Task<TResult> GetAsync<TResult>(TPrimaryDataType primaryId, TSecondaryDataType secondaryId) => Task.Run(() => Mapper.Map<TEntity, TResult>(Get(primaryId, secondaryId)));

		public virtual long GetCount() {
			using(var contextFactory = ComponentContainer.Component<TContext>()) {
				return contextFactory.Component.Set<TEntity>().Count();
			}
		}

		public virtual Task<long> GetCountAsync() => Task.Run(() => GetCount());

		public virtual IEnumerable<TEntity> GetList(int start = 0, int count = 0, string columnName = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false) {
			using(var contextFactory = ComponentContainer.Component<TContext>()) {
				var query = sortOrder == SortOrder.Ascending ? contextFactory.Component.Set<TEntity>().OrderBy(columnName) : contextFactory.Component.Set<TEntity>().OrderByDescending(columnName);
				if(start > 0)
					query = query.Skip(start);
				if(count > 0)
					query = query.Take(count);
				foreach(var obj in query)
					yield return obj;
			}
		}

		public virtual IEnumerable<TResult> GetList<TResult>(int start = 0, int count = 0, string columnName = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false) {
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

		public virtual Task<IEnumerable<TEntity>> GetListAsync(int start = 0, int count = 0, string columnName = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false)
			=> Task.Run(() => GetList(start, count, columnName, sortOrder));

		public virtual Task<IEnumerable<TResult>> GetListAsync<TResult>(int start = 0, int count = 0, string columnName = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false)
			=> Task.Run(() => GetList<TResult>(start, count, columnName, sortOrder));

		public abstract void Save(TEntity model);

		public virtual void Save<TModel>(TModel model) => Save(Mapper.Map<TModel, TEntity>(model));

		public virtual void SaveAsync(TEntity model) => Task.Run(() => Save(model));

		public virtual void SaveAsync<TModel>(TModel model) => Task.Run(() => Save<TModel>(model));
	}
}