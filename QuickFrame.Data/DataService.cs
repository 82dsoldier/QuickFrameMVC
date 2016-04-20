using ExpressMapper;
using QuickFrame.Data.Exceptions;
using QuickFrame.Data.Interfaces;
using QuickFrame.Di;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Data {
	public class DataServiceCore<TDataType, TContext, TEntity>
		: IDataServiceCore<TDataType, TEntity>
		where TContext : DbContext
		where TEntity : class, IDataModelCore<TDataType> {

		public virtual void Create(TEntity model) {
			using (var contextFactory = ComponentContainer.Component<TContext>()) {
				contextFactory.Component.Set<TEntity>().Add(model);
				contextFactory.Component.SaveChanges();
			}
		}

		public virtual void CreateAsync(TEntity model) => Task.Run(() => Create(model));

		public virtual void Create<TModel>(TModel model)
			where TModel : IDataTransferObjectCore<TDataType, TEntity, TModel> => Create(Mapper.Map<TModel, TEntity>(model));

		public virtual void CreateAsync<TModel>(TModel model)
			where TModel : IDataTransferObjectCore<TDataType, TEntity, TModel> => Task.Run(() => Create(Mapper.Map<TModel, TEntity>(model)));

		public virtual void Delete(int id) {
			using (var contextFactory = ComponentContainer.Component<TContext>()) {
				var dbModel = contextFactory.Component.Set<TEntity>().FirstOrDefault(obj => obj.Id.Equals(id));
				if (dbModel == null)
					throw new RecordDoesNotExistException();
				dbModel.IsDeleted = true;
				contextFactory.Component.Entry(dbModel).State = EntityState.Modified;
				contextFactory.Component.SaveChanges();
			}
		}

		public virtual void DeleteAsync(int id) => Task.Run(() => Delete(id));

		public virtual TEntity Get(int id) {
			using (var contextFactory = ComponentContainer.Component<TContext>()) {
				return contextFactory.Component.Set<TEntity>().FirstOrDefault(obj => obj.Id.Equals(id));
			}
		}

		public virtual Task<TEntity> GetAsync(int id) => Task.Run(() => Get(id));
		public virtual TResult Get<TResult>(int id) where TResult : IDataTransferObjectCore<TDataType, TEntity, TResult> {
			using (var contextFactory = ComponentContainer.Component<TContext>()) {
				return Mapper.Map<TEntity, TResult>(contextFactory.Component.Set<TEntity>().FirstOrDefault(obj => obj.Id.Equals(id)));
			}
		}
		public virtual Task<TResult> GetAsync<TResult>(int id)
			where TResult : IDataTransferObjectCore<TDataType, TEntity, TResult> => Task.Run(() => Get<TResult>(id));
		public virtual int GetCount(bool includeDeleted = false) {
			using (var contextFactory = ComponentContainer.Component<TContext>()) {
				return includeDeleted ? contextFactory.Component.Set<TEntity>().Count() : contextFactory.Component.Set<TEntity>().Where(t => t.IsDeleted == false).Count();
			}
		}
		public virtual Task<int> GetCountAsync(bool includeDeleted = false) => Task.Run(() => GetCount(includeDeleted));
		public virtual IEnumerable<TEntity> GetList(int start = 0, int count = 0, string columnName = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false) {
			using (var contextFactory = ComponentContainer.Component<TContext>()) {
				var query = sortOrder == SortOrder.Ascending ? contextFactory.Component.Set<TEntity>().OrderBy(columnName) : contextFactory.Component.Set<TEntity>().OrderByDescending(columnName);
				if (!includeDeleted)
					query = query.Where(t => t.IsDeleted == false);
				if (start > 0)
					query = query.Skip(start);
				if (count > 0)
					query = query.Take(count);
				foreach (var obj in query)
					yield return obj;
			}
		}

		public virtual Task<IEnumerable<TEntity>> GetListAsync(int start = 0, int count = 0, string columnName = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false)
			=> Task.Run(() => GetList(start, count, columnName, sortOrder, includeDeleted));

		public virtual IEnumerable<TResult> GetList<TResult>(int start = 0, int count = 0, string columnName = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false)
			where TResult : IDataTransferObjectCore<TDataType, TEntity, TResult> {
			foreach (var obj in GetList(start, count, columnName, sortOrder, includeDeleted))
				yield return Mapper.Map<TEntity, TResult>(obj);
		}

		public virtual Task<IEnumerable<TResult>> GetListAsync<TResult>(int start = 0, int count = 0, string columnName = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false)
			where TResult : IDataTransferObjectCore<TDataType, TEntity, TResult>
			=> Task.Run(() => GetList<TResult>(start, count, columnName, sortOrder, includeDeleted));
		public virtual void Save(TEntity model) {
			using (var contextFactory = ComponentContainer.Component<TContext>()) {
				var dbSet = contextFactory.Component.Set<TEntity>();
				if (model.Id.Equals(default(TDataType))) {
					Create(model);
					return;
				}
				dbSet.Attach(model);
				contextFactory.Component.Entry(model).State = EntityState.Modified;
				contextFactory.Component.SaveChanges();
			}
		}

		public virtual void SaveAsync(TEntity model) => Task.Run(() => Save(model));

		public virtual void Save<TModel>(TModel model)
			where TModel : IDataTransferObjectCore<TDataType, TEntity, TModel> => Save(Mapper.Map<TModel, TEntity>(model));

		public virtual void SaveAsync<TModel>(TModel model)
			where TModel : IDataTransferObjectCore<TDataType, TEntity, TModel> => Task.Run(() => Save<TModel>(model));
	}

	public class DataServiceInt<TContext, TEntity>
		: DataServiceCore<int, TContext, TEntity>
		where TContext : DbContext
		where TEntity : class, IDataModelInt {

	}
	public class DataServiceLong<TContext, TEntity>
	: DataServiceCore<long, TContext, TEntity>
	where TContext : DbContext
	where TEntity : class, IDataModelLong {

	}
	public class DataServiceGuid<TContext, TEntity>
	: DataServiceCore<Guid, TContext, TEntity>
	where TContext : DbContext
	where TEntity : class, IDataModelGuid {

	}

	public class DataService<TContext, TEntity>
	: DataServiceCore<int, TContext, TEntity>
	where TContext : DbContext
	where TEntity : class, IDataModelInt {

	}
}
