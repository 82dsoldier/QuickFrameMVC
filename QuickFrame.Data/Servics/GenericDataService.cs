using ExpressMapper;
using QuickFrame.Data.Interfaces;
using QuickFrame.Di;
using System.Data.Entity;
using System.Threading.Tasks;

namespace QuickFrame.Data.Services {

	public abstract class GenericDataService<TContext, TDataType, TEntity>
		: ReadOnlyDataService<TContext, TDataType, TEntity>, IGenericDataService<TDataType, TEntity>
		where TContext : DbContext
		where TEntity : class {

		public abstract TDataType Create(TEntity model);

		public virtual Task<TDataType> CreateAsync(TEntity model) => Task.Run(() => Create(model));

		public virtual TDataType Create<TModel>(TModel model) => Create(Mapper.Map<TModel, TEntity>(model));

		public virtual Task<TDataType> CreateAsync<TModel>(TModel model) => Task.Run(() => Create(Mapper.Map<TModel, TEntity>(model)));

		public abstract bool Delete(TDataType id);

		public virtual void DeleteAsync(TDataType id) => Task.Run(() => Delete(id));

		public abstract void Save(TEntity model);

		public virtual void SaveAsync(TEntity model) => Task.Run(() => Save(model));

		public virtual void Save<TModel>(TModel model) => Save(Mapper.Map<TModel, TEntity>(model));

		public virtual void SaveAsync<TModel>(TModel model) => Task.Run(() => Save<TModel>(model));
	}
}