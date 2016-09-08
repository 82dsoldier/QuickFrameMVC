using ExpressMapper;
using QuickFrame.Data;
using QuickFrame.Data.Interfaces;
using QuickFrame.Di;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Data.Services {

	public abstract class DataService<TContext, TEntity>
	: DataServiceInt<TContext, TEntity>
	where TContext : DbContext
	where TEntity : class, IDataModelInt {
	}

	public abstract class DataService<TContext, T1, T2, TEntity>
		: IDataService<T1, T2, TEntity>
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

		public abstract bool Delete(T1 primaryId, T2 secondaryId);

		public virtual void DeleteAsync(T1 primaryId, T2 secondaryId) => Task.Run(() => Delete(primaryId, secondaryId));

		public abstract TEntity Get(T1 primaryId, T2 secondaryId);

		public virtual TResult Get<TResult>(T1 primaryId, T2 secondaryId) => Mapper.Map<TEntity, TResult>(Get(primaryId, secondaryId));

		public virtual Task<TEntity> GetAsync(T1 primaryId, T2 secondaryId) => Task.Run(() => Get(primaryId, secondaryId));

		public virtual Task<TResult> GetAsync<TResult>(T1 primaryId, T2 secondaryId) => Task.Run(() => Mapper.Map<TEntity, TResult>(Get(primaryId, secondaryId)));

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

	public abstract class DataService<TContext, T1, T2, T3, TEntity>
	: IDataService<T1, T2, T3, TEntity>
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

		public abstract bool Delete(T1 primaryId, T2 secondaryId, T3 thirdId);

		public virtual void DeleteAsync(T1 primaryId, T2 secondaryId, T3 thirdId) => Task.Run(() => Delete(primaryId, secondaryId, thirdId));

		public abstract TEntity Get(T1 primaryId, T2 secondaryId, T3 thirdId);

		public virtual TResult Get<TResult>(T1 primaryId, T2 secondaryId, T3 thirdId) => Mapper.Map<TEntity, TResult>(Get(primaryId, secondaryId, thirdId));

		public virtual Task<TEntity> GetAsync(T1 primaryId, T2 secondaryId, T3 thirdId) => Task.Run(() => Get(primaryId, secondaryId, thirdId));

		public virtual Task<TResult> GetAsync<TResult>(T1 primaryId, T2 secondaryId, T3 thirdId) => Task.Run(() => Mapper.Map<TEntity, TResult>(Get(primaryId, secondaryId, thirdId)));

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
	public abstract class DataService<TContext, TDataType, TEntity>
		: ReadOnlyDataService<TContext, TDataType, TEntity>,
		IDataService<TDataType, TEntity>
		where TContext : DbContext
		where TEntity : class, IDataModel<TDataType> {

		public abstract TDataType Create(TEntity model);

		public virtual Task<TDataType> CreateAsync(TEntity model) => Task.FromResult(Create(model));

		public virtual TDataType Create<TModel>(TModel model) => Create(Mapper.Map<TModel, TEntity>(model));

		public virtual Task<TDataType> CreateAsync<TModel>(TModel model) => Task.FromResult(Create(Mapper.Map<TModel, TEntity>(model)));

		public abstract bool Delete(TDataType id);

		public virtual void DeleteAsync(TDataType id) => Task.FromResult(Delete(id));

		public virtual void SaveAsync(TEntity model) => Task.Run(() => Save(model));

		public virtual void Save<TModel>(TModel model) => Save(Mapper.Map<TModel, TEntity>(model));

		public virtual void SaveAsync<TModel>(TModel model) => Task.Run(() => Save<TModel>(model));

		public virtual void Save(TEntity model) {
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