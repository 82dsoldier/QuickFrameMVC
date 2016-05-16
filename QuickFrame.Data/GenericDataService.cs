using ExpressMapper;
using QuickFrame.Data.Interfaces;
using QuickFrame.Di;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Data
{
	public abstract class GenericDataService<TContext, TDataType, TEntity>
		: ReadOnlyDataService<TContext, TDataType, TEntity>, IGenericDataService<TDataType, TEntity>
		where TContext : DbContext
		where TEntity : class, IDataModelCore<TDataType> {

		public virtual void Create(TEntity model) {
			using (var contextFactory = ComponentContainer.Component<TContext>()) {
				contextFactory.Component.Set<TEntity>().Add(model);
				contextFactory.Component.SaveChanges();
			}
		}

		public virtual void CreateAsync(TEntity model) => Task.Run(() => Create(model));

		public virtual void Create<TModel>(TModel model) => Create(Mapper.Map<TModel, TEntity>(model));

		public virtual void CreateAsync<TModel>(TModel model) => Task.Run(() => Create(Mapper.Map<TModel, TEntity>(model)));

		public abstract void Delete(TDataType id);

		public virtual void DeleteAsync(TDataType id) => Task.Run(() => Delete(id));

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

		public virtual void Save<TModel>(TModel model) => Save(Mapper.Map<TModel, TEntity>(model));

		public virtual void SaveAsync<TModel>(TModel model) => Task.Run(() => Save<TModel>(model));
	}
}
